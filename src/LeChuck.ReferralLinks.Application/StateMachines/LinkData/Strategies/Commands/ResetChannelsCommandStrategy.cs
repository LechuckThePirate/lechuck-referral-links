#region using directives

using System.Collections.Generic;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.Strategies.Commands
{
    public class ResetChannelsCommandStrategy : IMultiLinkStrategy
    {
        public bool CanHandle(string key) =>
            key == ProgramLinkStateMachineWorkflow.CommandsEnum.ResetChanels.ToString();

        public Task<bool> Handle(IUpdateContext context, MultiLinkMessage entity,
            IStateMachine<IUpdateContext, MultiLinkMessage> stateMachine)
        {
            entity.Channels = new List<Channel>();
            return Task.FromResult(true);
        }
    }
}