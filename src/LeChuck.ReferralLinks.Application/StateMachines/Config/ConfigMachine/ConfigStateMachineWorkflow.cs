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
            VendorsState,
            InputClientIdState,
            InputClientSecretState,
            DoneState,
            CancelledState,
            SelectedVendorState,
            InputVendorCustomState
        }

        public enum CommandsEnum
        {
            VendorsCmd,
            BackCmd,
            SaveConfigCmd,
            CancelConfigCmd,
            SelectVendorCmd,
            InputVendorGotoLinkCmd,
            SetVendorGotoLinkCmd
        }

        public override string InitialState => $"{StatesEnum.HomeState}";

        private static readonly List<StepMachineState> _stateList = new List<StepMachineState>
        {
            new StepMachineState($"{StatesEnum.HomeState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.VendorsCmd}", $"{StatesEnum.VendorsState}"},
                    {$"{CommandsEnum.SaveConfigCmd}", $"{StatesEnum.DoneState}"},
                    {$"{CommandsEnum.CancelConfigCmd}", $"{StatesEnum.CancelledState}"}
                }
            },

            // Vendors
            new StepMachineState($"{StatesEnum.VendorsState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SelectVendorCmd}", $"{StatesEnum.SelectedVendorState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.HomeState}"}
                }
            },
            new StepMachineState($"{StatesEnum.SelectedVendorState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.InputVendorGotoLinkCmd}", $"{StatesEnum.InputVendorCustomState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.HomeState}"}
                }
            },
            new StepMachineState($"{StatesEnum.InputVendorCustomState}")
            {
                OnNext = $"{CommandsEnum.SetVendorGotoLinkCmd}",
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SetVendorGotoLinkCmd}", $"{StatesEnum.SelectedVendorState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.VendorsState}"}
                }
            },


            new StepMachineState($"{StatesEnum.DoneState}") {EndMachine = true},
            new StepMachineState($"{StatesEnum.CancelledState}") {EndMachine = true}
        };

        public override IEnumerable<StepMachineState> StateList => _stateList;

        public class Params
        {
            public static string SelectedVendor = nameof(SelectedVendor);
        }
    }
}