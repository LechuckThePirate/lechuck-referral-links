using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.ReferralLinks.Application.Views;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.Stateless.StateMachine;
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
        private readonly IStateMachineFactory _stateMachineFactory;

        public LinkUpdateHandler(
            ILogger<LinkUpdateHandler> logger,
            IBotService bot,
            ILinkService linkService,
            IStateMachineFactory stateMachineFactory
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _linkService = linkService ?? throw new ArgumentNullException(nameof(linkService));
            _stateMachineFactory = stateMachineFactory ?? throw new ArgumentNullException(nameof(stateMachineFactory));
        }

        public int Order { get; } = int.MaxValue;

        public bool CanHandle(IUpdateContext update) => update.Content.Any(c => c.Type == Constants.MessageContentType.Url);

        public async Task<bool> HandleUpdate(IUpdateContext updateContext)
        {
            _logger.LogTrace($"Handling update: {updateContext}");
            await _bot.DeleteMessageAsync(updateContext.ChatId, updateContext.MessageId ?? 0);

            var processingMsgId = await _bot.SendTextMessageAsync(updateContext.ChatId, "Procesando enlace...");

            var urls = updateContext.Content
                .Where(c => c.Type == Constants.MessageContentType.Url)
                .Select(c => c.Value);

            if (!urls.Any())
            {
                await _bot.DeleteMessageAsync(updateContext.ChatId, processingMsgId ?? 0);
                return false;
            }

            var multiLink = new MultiLink();
            Parallel.ForEach(urls,
                url => { multiLink.Links.Add(_linkService.BuildMessage(url).GetAwaiter().GetResult()); });
            multiLink.Links = multiLink.Links
                .Select((m, i) =>
                {
                    m.Number = i + 1;
                    return m;
                }).ToList();

            _logger.LogDebug($"Generated object:\n{JsonSerializer.Serialize(multiLink, new JsonSerializerOptions{WriteIndented = true})}");

            await _bot.DeleteMessageAsync(updateContext.ChatId, processingMsgId ?? 0);

            if (!multiLink.Links.Any())
            {
                return false;
            }

            await _bot.DeleteMessageAsync(updateContext.ChatId, updateContext.MessageId ?? 0);
            var machine = await _stateMachineFactory.Create<IUpdateContext, MultiLink>(updateContext.User.UserId.ToString());
            await machine.Run(ProgramLinkStateMachineWorkflow.StatesEnum.MenuState.ToString(), updateContext, multiLink);
            return true;

        }
    }
}
