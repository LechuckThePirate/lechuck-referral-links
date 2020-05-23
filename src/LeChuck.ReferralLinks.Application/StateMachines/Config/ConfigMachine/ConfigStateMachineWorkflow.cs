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
            VendorsState,
            InputClientIdState,
            InputClientSecretState,
            DoneState,
            CancelledState,
            SelectedtAffiliateState,
            SelectAffiliateSpaceState,
            SelectedVendorState,
            InputVendorGotoLinkState
        }

        public enum CommandsEnum
        {
            AffiliatesCmd,
            VendorsCmd,
            SelectAffiliateCmd,
            SetClientIdCmd,
            SetClientSecretCmd,
            BackCmd,
            SaveConfigCmd,
            CancelConfigCmd,
            SetAffiliateCredentialsCmd,
            ToggleActiveAffiliateCmd,
            SelectAffiliateSpaceCmd,
            SetAffiliateSpaceCmd,
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
                    {$"{CommandsEnum.AffiliatesCmd}", $"{StatesEnum.AffiliatesState}"},
                    {$"{CommandsEnum.VendorsCmd}", $"{StatesEnum.VendorsState}"},
                    {$"{CommandsEnum.SaveConfigCmd}", $"{StatesEnum.DoneState}"},
                    {$"{CommandsEnum.CancelConfigCmd}", $"{StatesEnum.CancelledState}"}
                }
            },
            new StepMachineState($"{StatesEnum.AffiliatesState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SelectAffiliateCmd}", $"{StatesEnum.SelectedtAffiliateState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.HomeState}"}
                }
            },
            new StepMachineState($"{StatesEnum.SelectedtAffiliateState}")
            {
                AvailableCommands = new Dictionary<string, string> {
                    {$"{CommandsEnum.SetAffiliateCredentialsCmd}", $"{StatesEnum.InputClientIdState}"},
                    {$"{CommandsEnum.SelectAffiliateSpaceCmd}", $"{StatesEnum.SelectAffiliateSpaceState}"},
                    {$"{CommandsEnum.ToggleActiveAffiliateCmd}", $"{StatesEnum.SelectedtAffiliateState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
                }
            },
            new StepMachineState($"{StatesEnum.SelectAffiliateSpaceState}")
            {
                AvailableCommands = new Dictionary<string, string> {
                    {$"{CommandsEnum.SetAffiliateSpaceCmd}", $"{StatesEnum.SelectedtAffiliateState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
                }
            },
            new StepMachineState($"{StatesEnum.InputClientIdState}")
            {
                OnNext = $"{CommandsEnum.SetClientIdCmd}",
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SetClientIdCmd}", $"{StatesEnum.InputClientSecretState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
                }
            },
            new StepMachineState($"{StatesEnum.InputClientSecretState}")
            {
                OnNext = $"{CommandsEnum.SetClientSecretCmd}",
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SetClientSecretCmd}",$"{StatesEnum.SelectedtAffiliateState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
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
                    {$"{CommandsEnum.InputVendorGotoLinkCmd}", $"{StatesEnum.InputVendorGotoLinkState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.HomeState}"}
                }
            },
            new StepMachineState($"{StatesEnum.InputVendorGotoLinkState}")
            {
                OnNext = $"{CommandsEnum.SetVendorGotoLinkCmd}",
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SetVendorGotoLinkCmd}", $"{StatesEnum.SelectedVendorState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.AffiliatesState}"}
                }
            },


            new StepMachineState($"{StatesEnum.DoneState}") {EndMachine = true},
            new StepMachineState($"{StatesEnum.CancelledState}") {EndMachine = true}
        };

        public override IEnumerable<StepMachineState> StateList => _stateList;

        public class Params
        {
            public static string SelectedAffiliate = nameof(SelectedAffiliate);
            public static string SelectedVendor = nameof(SelectedVendor);
        }
    }
}