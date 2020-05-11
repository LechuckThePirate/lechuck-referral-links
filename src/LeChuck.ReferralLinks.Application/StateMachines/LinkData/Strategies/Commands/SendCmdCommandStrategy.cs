using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.Stateless.StateMachine.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.Strategies.Commands
{
    public class SendCmdCommandStrategy : IMultiLinkStrategy
    {
        public bool CanHandle(string key) => key == ProgramLinkStateMachineWorkflow.CommandsEnum.SendCmd.ToString();

        public Task<bool> Handle(IUpdateContext context, Domain.Models.MultiLink entity)
        {
            throw new NotImplementedException();
        }
    }
}
