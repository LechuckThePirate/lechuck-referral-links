using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.Models;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Providers;
using LeChuck.Telegram.Bot.Framework.Models;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.Services
{
    public interface IMultiLinkMessageBuilder
    {
        IMultiLinkMessageBuilder AddUrls(IEnumerable<MessageContent> content);
        Task<IMultiLinkMessageBuilder> ProcessUrls();
        MultiLinkMessage Build();
    }

    public class MultiLinkMessageBuilder : IMultiLinkMessageBuilder
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IAffiliateProvider _affiliateProvider;
        private readonly ILinkParserProvider _linkParserProvider;
        private readonly IUrlShortenerProvider _urlShortenerProvider;
        private readonly ILogger<MultiLinkMessageBuilder> _logger;
        private readonly MultiUrlContext _context = new MultiUrlContext();

        public MultiLinkMessageBuilder(
            IHttpClientFactory clientFactory,
            IAffiliateProvider affiliateProvider,
            ILinkParserProvider linkParserProvider,
            IUrlShortenerProvider urlShortenerProvider,
            ILogger<MultiLinkMessageBuilder> logger)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _affiliateProvider = affiliateProvider ?? throw new ArgumentNullException(nameof(affiliateProvider));
            _linkParserProvider = linkParserProvider ?? throw new ArgumentNullException(nameof(linkParserProvider));
            _urlShortenerProvider = urlShortenerProvider ?? throw new ArgumentNullException(nameof(urlShortenerProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IMultiLinkMessageBuilder AddUrls(IEnumerable<MessageContent> content)
        {
            _context.UrlContexts.AddRange(content
                .Where(c => c.Type == Constants.MessageContentType.Url)
                .Select((c, i) => new UrlContext { OriginalUrl = c.Value, Number = i+1 }));

            return this;
        }

        public async Task<IMultiLinkMessageBuilder> ProcessUrls()
        {
            //foreach (var ctx in _context.UrlContexts)
            //{
            //    await GetContent(ctx);
            //    await ResolveHelpers(ctx);
            //    await GetDeepLink(ctx);
            //    await ShortenUrl(ctx);
            //    await BuildMessage(ctx);
            //    if (ctx.Message != null)
            //    {
            //        ctx.Message.Number = ctx.Number;
            //        ctx.Message.Url = ctx.Url;
            //    }
            //}

            foreach (var ctx in _context.UrlContexts)
            {
                await GetContent(ctx)
                    .ContinueWith(a => ResolveHelpers(ctx))
                    .ContinueWith(a => GetDeepLink(ctx))
                    .ContinueWith(a => ShortenUrl(ctx))
                    .ContinueWith(a => BuildMessage(ctx))
                    .ContinueWith(a =>
                    {
                        if (ctx.Message != null)
                        {
                            ctx.Message.Number = ctx.Number;
                            ctx.Message.Url = ctx.Url;
                        }
                    });
            }

            return await Task.FromResult(this);
        }

        public MultiLinkMessage Build()
        {
            return new MultiLinkMessage
            {
                Links = _context.UrlContexts.Select(l => l.Message).ToList()
            };
        }

        // SINGLE-LINK METHODS

        private async Task GetContent(UrlContext ctx)
        {
            try
            {
                _logger.LogInformation($"Getting content for link {ctx.Number}");
                using var client = _clientFactory.CreateClient();
                ctx.Content = await client.GetStringAsync(ctx.OriginalUrl);
                _logger.LogInformation($"Got content for link {ctx.Number} ({ctx.Content?.Length} ?? 0 characters)");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting content for link {ctx.Number}\n" +
                                 $"Exception: {ex.Message}\n" +
                                 $"StackTrace: {ex.StackTrace}");
            }
        }

        private async Task ResolveHelpers(UrlContext ctx)
        {
            ctx.Parser = _linkParserProvider.GetParserFor(ctx.Content);
            ctx.Affiliate = _affiliateProvider.GetAffiliateFor(ctx.Parser?.Name);
            // TODO: Find out which shortener do I need
            ctx.Shortener = _urlShortenerProvider.GetServiceOrDefault(UrlShortenersEnum.BitLy);

            _logger.LogInformation($"Resolved helpers for link {ctx.Number}:\n" +
                                   $"  Parser: {ctx.Parser?.Name ?? "None"}\n" +
                                   $"  Affiliate: {ctx.Affiliate?.Name}");
            await Task.CompletedTask;
        }

        private async Task GetDeepLink(UrlContext ctx)
        {
            if (ctx.Affiliate == null || ctx.Parser == null)
            {
                _logger.LogWarning($"No affiliate for link {ctx.Number}");
                return;
            }

            try
            {
                ctx.DeepLink = await ctx.Affiliate.GetDeepLink(ctx.Parser.Name, ctx.Url);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting deep link for {ctx.Number}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private async Task ShortenUrl(UrlContext ctx)
        {
            if (ctx.Shortener == null)
            {
                _logger.LogWarning($"No shortener for link {ctx.Number}");
                return ;
            }

            try
            {
                ctx.ShortenedUrl = await ctx.Shortener.ShortenUrl(ctx.Url);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error shortening url for {ctx.Number}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private async Task BuildMessage(UrlContext ctx)
        {
            if (ctx.Parser == null)
            {
                _logger.LogWarning($"No parser for link {ctx.Number}");
                return;
            }

            if (string.IsNullOrWhiteSpace(ctx.Content))
            {
                _logger.LogWarning($"No content for link {ctx.Number}");
                return;
            }

            try
            {
                ctx.Message = await ctx.Parser.ParseContent(ctx.Content);
                ctx.Message.Url = ctx.Url;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error building message for {ctx.Number}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // MULTI-LINK METHODS

        private void GetDeepLinks()
        {
            // Group urls by Affiliate/Vendor (parser)
            var afililiates =
                _context.UrlContexts.GroupBy(ctx => new
                    {
                        AffiliateName = ctx.Affiliate?.Name,
                        ParserName = ctx.Parser?.Name
                    })
                    .Where(group => group.Key.AffiliateName != null
                                    && group.Key.ParserName != null)
                    .ToList();

            Parallel.ForEach(afililiates, async group =>
            {
                await GetDeepLinksFrom(group.Select(ctx => ctx).ToList());
            });
        }

        private async Task GetDeepLinksFrom(List<UrlContext> ctxGroup)
        {
            var affiliate = ctxGroup.First().Affiliate;
            var parser = ctxGroup.First().Parser;
            var urls = ctxGroup.Select(ctx => ctx.Url);
            var deepLinks = (await affiliate.GetDeepLinks(parser.Name, urls))
                .Where(d => d.DeepLinkUrl != null)
                .ToList();
            _logger.LogInformation(
                $"Resolved {deepLinks.Count()} deep-links for {affiliate.Name}/{parser.Name}");
            foreach (var deepLink in deepLinks)
            {
                var ctx = ctxGroup.FirstOrDefault(ctx => ctx.Url == deepLink.Url);
                if (ctx != null)
                {
                    _logger.LogInformation($"DeepLink generated for link {ctx.Number}");
                    ctx.DeepLink = deepLink.DeepLinkUrl;
                    // TODO: Find out the shortener for this
                    ctx.Shortener = _urlShortenerProvider.GetServiceOrDefault(UrlShortenersEnum.BitLy);
                }
            }
        }

        private void ShortenUrls()
        {
            Parallel.ForEach(_context.UrlContexts, async ctx =>
            {
                if (ctx.Shortener == null)
                {
                    _logger.LogInformation($"No shortener selected for link {ctx.Number}");
                }
                else
                {
                    ctx.ShortenedUrl = await ctx.Shortener.ShortenUrl(ctx.Url);
                    _logger.LogInformation($"Shortened url for link {ctx.Number} to '{ctx.ShortenedUrl}");
                }
            });
        }

        private void BuildAllMessages()
        {
            Parallel.ForEach(_context.UrlContexts, async ctx =>
            {
                await BuildMessage(ctx);
            });
        }

    }
}
