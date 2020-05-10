using System.Collections.Generic;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.ProgramLink;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.StateMachines.Strategies.Commands
{
    public class ResetChannelsCommandStrategy : ILinkDataStrategy
    {
        public bool CanHandle(string key) =>
            key == ProgramLinkStateMachineWorkflow.CommandsEnum.ResetChanels.ToString();

        public Task<bool> Handle(IUpdateContext context, LinkData entity)
        {
            entity.Channels = new List<Channel>();
            return Task.FromResult(true);
        }
    }
}
