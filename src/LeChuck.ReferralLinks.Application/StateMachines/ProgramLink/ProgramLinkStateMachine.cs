using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Strategies;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.UnitsOfWork;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.StateMachines.ProgramLink
{
    public class ProgramLinkStateMachine : StateMachine<IUpdateContext>
    {
        private readonly ILinkDataStrategySelector _strategySelector;
        private readonly ILogger<ProgramLinkStateMachine> _logger;
        private LinkData _entity = new LinkData();

        public ProgramLinkStateMachine(IStateMachineStore stateMachineStore,
            ILinkDataStrategySelector strategySelector,
            ILogger<ProgramLinkStateMachine> logger)
            : base(new ProgramLinkStateMachineWorkflow(), stateMachineStore)
        {
            _strategySelector = strategySelector ?? throw new ArgumentNullException(nameof(strategySelector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<bool> OnCommand(string command, IUpdateContext context = default)
        {
            _logger.LogInformation($"OnCommand('{command}','{context}')");
            return await RunStrategy(context, _strategySelector.GetHandlerFor(command));
        }

        protected override async Task<bool> OnNextStep(string currentState, IUpdateContext context = default)
        {
            _logger.LogInformation($"OnNextStep('{context}')");
            return await RunStrategy(context, _strategySelector.GetHandlerFor(currentState));
        }

        protected override async Task OnNewState(string newState, IUpdateContext context = default)
        {
            _logger.LogInformation($"OnNewState('{newState}')");
            await RunStrategy(context, _strategySelector.GetHandlerFor(newState));
        }

        public override void DeserializeData(string data)
        {
            base.DeserializeData(data);
            var dict = JsonSerializer.Deserialize<IDictionary<string, object>>(data);
            _entity = (dict.ContainsKey(nameof(_entity))) ? JsonSerializer.Deserialize<LinkData>(dict[nameof(_entity)].ToString()) : null;
        }

        public override string SerializeData()
        {
            var data = new
            {
                MachineId,
                State,
                _entity = JsonSerializer.Serialize(_entity)
            };
            return JsonSerializer.Serialize(data);
        }

        async Task<bool> RunStrategy(IUpdateContext context, IStrategy<IUpdateContext, LinkData> strategy)
        {
            try
            {
                if (strategy == null)
                {
                    _logger.LogError($"No strategy found");
                    return false;
                }

                return await strategy.Handle(context, _entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error executing strategy", ex);
                return false;
            }

        }
    }
}
