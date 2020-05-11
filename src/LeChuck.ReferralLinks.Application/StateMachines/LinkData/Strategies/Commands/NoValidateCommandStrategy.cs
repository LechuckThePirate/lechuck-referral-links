using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.Strategies.Commands
{
    public class NoValidateCommandStrategy : IMultiLinkStrategy
    {
        private readonly IBotService _bot;
        public NoValidateCommandStrategy(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) =>
            new string[]
            {
                ProgramLinkStateMachineWorkflow.CommandsEnum.SelectUrlCmd.ToString(),
                ProgramLinkStateMachineWorkflow.CommandsEnum.SelectChannelsCmd.ToString(),
                ProgramLinkStateMachineWorkflow.CommandsEnum.SelectTimeSpanCmd.ToString(),
                ProgramLinkStateMachineWorkflow.CommandsEnum.BackCmd.ToString(),
                ProgramLinkStateMachineWorkflow.CommandsEnum.CancelCmd.ToString(),
            }.Contains(key);

        public async Task<bool> Handle(IUpdateContext context, Domain.Models.MultiLink entity)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            return true;
        }
    }
}
