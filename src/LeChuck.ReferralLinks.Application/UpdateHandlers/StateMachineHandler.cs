using System;
using System.Threading.Tasks;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.UpdateHandlers
{
    public class StateMachineHandler : IUpdateHandler
    {
        private readonly IStateMachineFactory _stateMachineFactory;
        private readonly ILogger<StateMachineHandler> _logger;

        public StateMachineHandler(IStateMachineFactory stateMachineFactory, ILogger<StateMachineHandler> logger)
        {
            _stateMachineFactory = stateMachineFactory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public int Order { get; } = 1;

        public bool CanHandle(IUpdateContext update) =>
            !update.IsGroup &&
            (update.MessageType == MessageTypeEnum.ButtonCallback
        || update.MessageType == MessageTypeEnum.Message);

        public async Task<bool> HandleUpdate(IUpdateContext updateContext)
        {
            var machine = await _stateMachineFactory.Retrieve(updateContext.User.UserId.ToString());
            if (machine == null)
                return false;
            try
            {
                if (updateContext.MessageType == MessageTypeEnum.Message)
                    await machine.NextStep(updateContext);
                else
                    await machine.ExecuteCommand(updateContext.CallbackButtonData[0], updateContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling state machine\n{ex.Message}\n{ex.StackTrace}");
                return false;
            }

            return true;
        }
    }
}
