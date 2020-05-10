using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.ProgramLink;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.Strategies.Commands
{
    public class NoValidateCommandStrategy : ILinkDataStrategy
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
                ProgramLinkStateMachineWorkflow.CommandsEnum.CancelCmd.ToString(),
                ProgramLinkStateMachineWorkflow.CommandsEnum.BackCmd.ToString(),
            }.Contains(key);

        public async Task<bool> Handle(IUpdateContext context, LinkData entity)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            return true;
        }
    }
}
