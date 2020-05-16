#region using directives

using System.Collections.Generic;
using LeChuck.Stateless.StateMachine.Models;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine
{
    public class ConfigStateMachineWorkflow : StateMachineWorkflow
    {
        public enum StatesEnum
        {
            HomeState,
            AffiliatesState,
            AddAffiliateState,
            RemoveAffiliateState,
            ConfigureAffiliateState,
            InputClientIdState,
            InputClientSecretState,
            DoneState,
            CancelState
        }

        public enum CommandsEnum
        {
            AffiliatesCmd,
            AddAffiliateCmd,
            RemoveAffiliateCmd,
            SelectAffiliateCmd,
            ConfigureAffiliateCmd,
            ClientIdCmd,
            ClientSecretCmd,
            SetClientIdCmd,
            SetClientSecretCmd,
            BackCmd,
            SaveConfigCmd,
            CancelConfigCmd
        }

        public override string InitialState => $"{StatesEnum.HomeState}";

        private static readonly List<StepMachineState> _stateList = new List<StepMachineState>
        {
            new StepMachineState($"{StatesEnum.HomeState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.AffiliatesCmd}", $"{StatesEnum.AffiliatesState}"},

                    {$"{CommandsEnum.SaveConfigCmd}", $"{StatesEnum.DoneState}"},
                    {$"{CommandsEnum.CancelConfigCmd}", $"{StatesEnum.CancelState}"}
                }
            },
            new StepMachineState($"{StatesEnum.AffiliatesState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.AddAffiliateCmd}", $"{StatesEnum.AddAffiliateState}"},
                    {$"{CommandsEnum.RemoveAffiliateCmd}", $"{StatesEnum.RemoveAffiliateState}"},
                    {$"{CommandsEnum.ConfigureAffiliateCmd}", $"{StatesEnum.ConfigureAffiliateState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.HomeState}"}
                }
            },
            new StepMachineState($"{StatesEnum.AddAffiliateState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SelectAffiliateCmd}", $"{StatesEnum.AffiliatesState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
                }
            },
            new StepMachineState($"{StatesEnum.RemoveAffiliateState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SelectAffiliateCmd}", $"{StatesEnum.AffiliatesState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
                }
            },
            new StepMachineState($"{StatesEnum.ConfigureAffiliateState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.ClientIdCmd}", $"{StatesEnum.InputClientIdState}"},
                    {$"{CommandsEnum.ClientSecretCmd}", $"{StatesEnum.InputClientSecretState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
                }
            },
            new StepMachineState($"{StatesEnum.InputClientIdState}")
            {
                OnNext = $"{CommandsEnum.SetClientIdCmd}",
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SetClientIdCmd}", $"{StatesEnum.ConfigureAffiliateState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
                }
            },
            new StepMachineState($"{StatesEnum.InputClientSecretState}")
            {
                OnNext = $"{CommandsEnum.SetClientSecretCmd}",
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SetClientSecretCmd}", $"{StatesEnum.ConfigureAffiliateState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
                }
            },
            new StepMachineState($"{StatesEnum.DoneState}") {EndMachine = true},
            new StepMachineState($"{StatesEnum.CancelState}") {EndMachine = true}
        };

        public override IEnumerable<StepMachineState> StateList => _stateList;
    }
}