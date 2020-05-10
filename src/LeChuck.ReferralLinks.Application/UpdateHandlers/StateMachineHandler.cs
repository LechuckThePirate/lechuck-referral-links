using System.Threading.Tasks;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.UpdateHandlers
{
    public class StateMachineHandler : IUpdateHandler
    {
        private readonly IStateMachineFactory _stateMachineFactory;

        public StateMachineHandler(IStateMachineFactory stateMachineFactory)
        {
            _stateMachineFactory = stateMachineFactory;
        }

        public bool CanHandle(IUpdateContext update) =>
            !update.IsGroup &&
            (update.MessageType == MessageTypeEnum.ButtonCallback
        || update.MessageType == MessageTypeEnum.Message);

        public async Task HandleUpdate(IUpdateContext updateContext)
        {
            var machine = await _stateMachineFactory.Retrieve<IUpdateContext>(updateContext.UserId.ToString());
            if (machine == null)
                return;

            if (updateContext.MessageType == MessageTypeEnum.Message)
                await machine.NextStep(updateContext);
            else
                await machine.ExecuteCommand(updateContext.CallbackButtonData[0], updateContext);
        }
    }
}
