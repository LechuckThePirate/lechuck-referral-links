using System;
using System.Linq;
using System.Threading.Tasks;
using Centvrio.Emoji;
using LeChuck.ReferralLinks.Application.Views;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.UpdateHandlers
{
    public class LinkUpdateHandler : IUpdateHandler
    {
        private readonly ILogger<LinkUpdateHandler> _logger;
        private readonly IBotService _bot;
        private readonly ILinkService _linkService;
        private readonly ILinkView _linkView;

        public LinkUpdateHandler(
            ILogger<LinkUpdateHandler> logger, 
            IBotService bot, 
            ILinkService linkService,
            ILinkView linkView)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _linkService = linkService ?? throw new ArgumentNullException(nameof(linkService));
            _linkView = linkView ?? throw new ArgumentNullException(nameof(linkView));
        }

        public bool CanHandle(IUpdateContext update) => false; // update.Content.Any(c => c.Type == Constants.MessageContentType.Url);

        public async Task HandleUpdate(IUpdateContext updateContext)
        {
            _logger.LogTrace($"Handling update: {updateContext}");

            var url = updateContext.Content.FirstOrDefault(c => c.Type == Constants.MessageContentType.Url)?.Value;
            if (string.IsNullOrEmpty(url))
                return;

            var message = await _linkService.BuildMessage(url);

            await _bot.DeleteMessageAsync(updateContext.ChatId, updateContext.MessageId ?? 0);
            await _linkView.SendView(updateContext.ChatId, message);
        }
    }
}
