using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Commands
{
    public class NoValidateCommandStrategy : IConfigStrategy
    {
        private readonly IBotService _bot;
        public NoValidateCommandStrategy(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) =>
            new string[]
            {
                ConfigStateMachineWorkflow.CommandsEnum.CancelCmd.ToString()
            }.Contains(key);

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            return true;
        }
    }
}
