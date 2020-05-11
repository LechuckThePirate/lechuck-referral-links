using System;
using System.Collections.Generic;
using System.Text;
using LeChuck.Stateless.StateMachine.Models;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine
{
    public class ConfigStateMachineWorkflow : StateMachineWorkflow
    {
        public enum StatesEnum
        {
            MenuState,
            ChannelsState,
            RemoveChannelsState,
            InputChannelState,
            DefaultShortenerState,
            DoneState,
            CancelledState
        }

        public enum CommandsEnum
        {
            SaveCmd,
            BackCmd,
            CancelCmd,
            ChannelsCmd,
            AddChannelsCmd,
            SetChannelCmd,
            RemoveChannelsCmd,
            RemoveOneChannelCmd,
            DefaultShortenerCmd,
            SetDefaultShortenerCmd
        }

        public override string InitialState => $"{StatesEnum.MenuState}";

        private static List<StepMachineState> _stateList = new List<StepMachineState>
        {
            new StepMachineState($"{StatesEnum.MenuState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SaveCmd}", $"{StatesEnum.DoneState}"},
                    {$"{CommandsEnum.CancelCmd}", $"{StatesEnum.CancelledState}"},
                    {$"{CommandsEnum.ChannelsCmd}", $"{StatesEnum.ChannelsState}"},
                    {$"{CommandsEnum.DefaultShortenerCmd}", $"{StatesEnum.DefaultShortenerState}"}
                }
            },
            new StepMachineState($"{StatesEnum.ChannelsState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.AddChannelsCmd}", $"{StatesEnum.InputChannelState}"},
                    {$"{CommandsEnum.RemoveChannelsCmd}", $"{StatesEnum.RemoveChannelsState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.MenuState}"}
                }
            },
            new StepMachineState($"{StatesEnum.RemoveChannelsState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.RemoveOneChannelCmd}", $"{StatesEnum.RemoveChannelsState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.RemoveChannelsState}"}
                }
            },
            new StepMachineState($"{StatesEnum.InputChannelState}")
            {
                OnNext = $"{CommandsEnum.SetChannelCmd}",
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.ChannelsState}"}
                }
            },
            new StepMachineState($"{StatesEnum.DefaultShortenerState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SetDefaultShortenerCmd}", $"{StatesEnum.DefaultShortenerState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.MenuState}"},
                }
            },
            new StepMachineState($"{StatesEnum.DoneState}") { EndMachine = true },
            new StepMachineState($"{StatesEnum.CancelledState}") { EndMachine = true }
        };

        public override IEnumerable<StepMachineState> StateList => _stateList;
    }
}
