﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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
        private readonly AppConfiguration _config;
        private MultiUrlContext _context;

        public MultiLinkMessageBuilder(
            IHttpClientFactory clientFactory,
            IAffiliateProvider affiliateProvider,
            ILinkParserProvider linkParserProvider,
            IUrlShortenerProvider urlShortenerProvider,
            ILogger<MultiLinkMessageBuilder> logger,
            AppConfiguration config)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _affiliateProvider = affiliateProvider ?? throw new ArgumentNullException(nameof(affiliateProvider));
            _linkParserProvider = linkParserProvider ?? throw new ArgumentNullException(nameof(linkParserProvider));
            _urlShortenerProvider = urlShortenerProvider ?? throw new ArgumentNullException(nameof(urlShortenerProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public IMultiLinkMessageBuilder AddUrls(IEnumerable<MessageContent> content)
        {
            _context = new MultiUrlContext
            {
                UrlContexts = content
                    .Where(c => c.Type == Constants.MessageContentType.Url)
                    .Select((c, i) => new UrlContext { OriginalUrl = c.Value, Number = i + 1 })
                    .ToList()
            };

            return this;
        }

        public async Task<IMultiLinkMessageBuilder> ProcessUrls()
        {
            foreach (var ctx in _context.UrlContexts)
            {
                await GetContent(ctx);
                ResolveHelpers(ctx);
                await GetDeepLink(ctx);
                await ShortenUrl(ctx);
                await BuildMessage(ctx);
                if (ctx.Message != null)
                {
                    ctx.Message.Number = ctx.Number;
                    ctx.Message.Url = ctx.Url;
                }
            }

            _logger.LogTrace($"Generated context:\n{JsonSerializer.Serialize(_context)}");

            return this;
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
                _logger.LogInformation($"Got content for link {ctx.Number} ({ctx.Content?.Length ?? 0} characters)");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting content for link {ctx.Number}\n" +
                                 $"Exception: {ex.Message}\n" +
                                 $"StackTrace: {ex.StackTrace}");
            }
        }

        private void ResolveHelpers(UrlContext ctx)
        {
            ctx.Parser = _linkParserProvider.GetParserFor(ctx.Content);
            ctx.Affiliate = _affiliateProvider.GetAffiliateFor(ctx.Parser?.Name);
            if (ctx.Affiliate != null)
            {
                var shortenerName = _config.AffiliateServices
                    .FirstOrDefault(aff => aff.Name == ctx.Affiliate.Name)?
                    .ShortenerName ?? _config.DefaultShortener;
                
                ctx.Shortener = _urlShortenerProvider.GetShortenerByName(shortenerName);
            }

            _logger.LogInformation($"Resolved helpers for link {ctx.Number}:\n" +
                                   $"  Parser: {ctx.Parser?.Name ?? "None"}\n" +
                                   $"  Affiliate: {ctx.Affiliate?.Name ?? "None"}");
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
                return;
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


    }
}
