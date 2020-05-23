#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Commands
{
    public class DefaultCommandStrategy : IConfigStrategy
    {
        private readonly IBotService _bot;

        public DefaultCommandStrategy(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) =>
            new[]
            {
                ConfigStateMachineWorkflow.CommandsEnum.CancelConfigCmd.ToString()
            }.Contains(key);

        public async Task<bool> Handle(IUpdateContext context,
            AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            return true;
        }
    }
}