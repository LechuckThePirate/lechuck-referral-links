
#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.Models;
using LeChuck.ReferralLinks.Application.Services;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Application.UpdateHandlers
{
    public class LinkUpdateHandler : IUpdateHandler
    {
        private readonly ILogger<LinkUpdateHandler> _logger;
        private readonly IBotService _bot;
        private readonly IStateMachineFactory _stateMachineFactory;
        private readonly IMultiLinkMessageBuilder _multiLinkMessageBuilder;

        public LinkUpdateHandler(
            ILogger<LinkUpdateHandler> logger,
            IBotService bot,
            IStateMachineFactory stateMachineFactory,
            IMultiLinkMessageBuilder multiLinkMessageBuilder
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _stateMachineFactory = stateMachineFactory ?? throw new ArgumentNullException(nameof(stateMachineFactory));
            _multiLinkMessageBuilder = multiLinkMessageBuilder ?? throw new ArgumentNullException(nameof(multiLinkMessageBuilder));
        }

        public int Order { get; } = int.MaxValue;

        public bool CanHandle(IUpdateContext update) =>
            update.Content.Any(c => c.Type == Constants.MessageContentType.Url);

        public async Task<bool> HandleUpdate(IUpdateContext updateContext)
        {
            _logger.LogTrace($"Handling update: {updateContext}");
            await _bot.DeleteMessageAsync(updateContext.ChatId, updateContext.MessageId ?? 0);

            var processingMsgId = await _bot.SendTextMessageAsync(updateContext.ChatId, "Procesando enlaces...");

            _multiLinkMessageBuilder.AddUrls(updateContext.Content);
            await _multiLinkMessageBuilder.ProcessUrls();
            var multiLink = _multiLinkMessageBuilder.Build();

            _logger.LogDebug(
                $"Generated object:\n{JsonSerializer.Serialize(multiLink, new JsonSerializerOptions {WriteIndented = true})}");

            await _bot.DeleteMessageAsync(updateContext.ChatId, processingMsgId ?? 0);

            if (!multiLink.Links.Any())
            {
                return false;
            }

            await _bot.DeleteMessageAsync(updateContext.ChatId, updateContext.MessageId ?? 0);

            var machine = await _stateMachineFactory.Create<IUpdateContext, MultiLinkMessage>(
                    updateContext.User.UserId.ToString());
            await machine.Run(ProgramLinkStateMachineWorkflow.StatesEnum.MenuState.ToString(), updateContext,
                multiLink);
            return true;
        }

    }
}