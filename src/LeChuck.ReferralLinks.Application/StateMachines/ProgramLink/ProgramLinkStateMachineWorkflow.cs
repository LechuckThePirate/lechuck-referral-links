using System;
using System.Collections.Generic;
using LeChuck.Stateless.StateMachine.Models;

namespace LeChuck.ReferralLinks.Application.StateMachines.ProgramLink
{
    public class ProgramLinkStateMachineWorkflow : StateMachineWorkflow
    {
        public enum StatesEnum
        {
            MenuState,
            InputUrlState,
            SelectChannelsState,
            SelectTimeSpanState,
            DoneState,
            CancelledState
        }

        public enum CommandsEnum
        {
            SaveCmd,
            CancelCmd,
            SelectUrlCmd,
            SetUrlCmd,
            SelectChannelsCmd,
            AddChannelCmd,
            RemoveChannelCmd,
            SelectTimeSpanCmd,
            SetTimeSpanCmd,
            BackCmd,
            ResetChanels
        }

        public override string InitialState => $"{StatesEnum.InputUrlState}";

        private static readonly IEnumerable<StepMachineState> _stateList = new List<StepMachineState>
        {
            new StepMachineState($"{StatesEnum.InputUrlState}")
            {
                OnNext = $"{CommandsEnum.SetUrlCmd}",
                AvailableCommands = new Dictionary<string, string>
                {
                    { $"{CommandsEnum.SetUrlCmd}", $"{StatesEnum.MenuState}" },
                    { $"{CommandsEnum.CancelCmd}", $"{StatesEnum.CancelledState}" }
                }
            },
            new StepMachineState($"{StatesEnum.MenuState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    { $"{CommandsEnum.SelectChannelsCmd}", $"{StatesEnum.SelectChannelsState}" },
                    { $"{CommandsEnum.SelectTimeSpanCmd}", $"{StatesEnum.SelectTimeSpanState}" },
                    { $"{CommandsEnum.SelectUrlCmd}", $"{StatesEnum.InputUrlState}" },
                    { $"{CommandsEnum.CancelCmd}", $"{StatesEnum.CancelledState}" },
                    { $"{CommandsEnum.SaveCmd}", $"{StatesEnum.DoneState}"}
                }
            },
            new StepMachineState($"{StatesEnum.SelectChannelsState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    { $"{CommandsEnum.AddChannelCmd}", $"{StatesEnum.SelectChannelsState}" },
                    { $"{CommandsEnum.RemoveChannelCmd}", $"{StatesEnum.SelectChannelsState}" },
                    { $"{CommandsEnum.ResetChanels}",$"{StatesEnum.SelectChannelsState}" },
                    { $"{CommandsEnum.BackCmd}", $"{StatesEnum.MenuState}" }
                }
            },
            new StepMachineState($"{StatesEnum.SelectTimeSpanState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    { $"{CommandsEnum.SetTimeSpanCmd}", $"{StatesEnum.MenuState}" },
                    { $"{CommandsEnum.CancelCmd}", $"{StatesEnum.CancelledState}" }
                }
            }
        };

        public override IEnumerable<StepMachineState> StateList => _stateList;
    }
}
