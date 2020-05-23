#region using directives

using System.Collections.Generic;
using LeChuck.Stateless.StateMachine.Models;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine
{
    public class ProgramLinkStateMachineWorkflow : StateMachineWorkflow
    {
        public enum StatesEnum
        {
            MenuState,
            InputUrlState,
            SelectChannelsState,
            SelectTimeSpanState,
            ReviewMessagesState,
            ReviewOneMessageState,
            DoneState,
            CancelledState
        }

        public enum CommandsEnum
        {
            SendCmd,
            CancelCmd,
            SelectUrlCmd,
            SetUrlCmd,
            SelectChannelsCmd,
            AddChannelCmd,
            RemoveChannelCmd,
            SelectTimeSpanCmd,
            SetTimeSpanCmd,
            ReviewMessagesCmd,
            ReviewOneMessage,
            BackCmd,
            ResetChanels
        }

        public override string InitialState => $"{StatesEnum.MenuState}";

        private static readonly IEnumerable<StepMachineState> _stateList = new List<StepMachineState>
        {
            new StepMachineState($"{StatesEnum.InputUrlState}")
            {
                OnNext = $"{CommandsEnum.SetUrlCmd}",
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SetUrlCmd}", $"{StatesEnum.MenuState}"},
                    {$"{CommandsEnum.CancelCmd}", $"{StatesEnum.CancelledState}"}
                }
            },
            new StepMachineState($"{StatesEnum.MenuState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SelectChannelsCmd}", $"{StatesEnum.SelectChannelsState}"},
                    {$"{CommandsEnum.SelectTimeSpanCmd}", $"{StatesEnum.SelectTimeSpanState}"},
                    {$"{CommandsEnum.SelectUrlCmd}", $"{StatesEnum.InputUrlState}"},
                    {$"{CommandsEnum.ReviewMessagesCmd}", $"{StatesEnum.ReviewMessagesState}"},
                    {$"{CommandsEnum.CancelCmd}", $"{StatesEnum.CancelledState}"},
                    {$"{CommandsEnum.SendCmd}", $"{StatesEnum.DoneState}"}
                }
            },
            new StepMachineState($"{StatesEnum.SelectChannelsState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.AddChannelCmd}", $"{StatesEnum.SelectChannelsState}"},
                    {$"{CommandsEnum.RemoveChannelCmd}", $"{StatesEnum.SelectChannelsState}"},
                    {$"{CommandsEnum.ResetChanels}", $"{StatesEnum.SelectChannelsState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.MenuState}"}
                }
            },
            new StepMachineState($"{StatesEnum.SelectTimeSpanState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.SetTimeSpanCmd}", $"{StatesEnum.MenuState}"},
                    {$"{CommandsEnum.CancelCmd}", $"{StatesEnum.CancelledState}"}
                }
            },
            new StepMachineState($"{StatesEnum.ReviewMessagesState}")
            {
                AvailableCommands = new Dictionary<string, string>
                {
                    {$"{CommandsEnum.ReviewOneMessage}", $"{StatesEnum.ReviewMessagesState}"},
                    {$"{CommandsEnum.BackCmd}", $"{StatesEnum.MenuState}"}
                }
            },
            new StepMachineState($"{StatesEnum.CancelledState}") {EndMachine = true},
            new StepMachineState($"{StatesEnum.DoneState}") {EndMachine = true}
        };

        public override IEnumerable<StepMachineState> StateList => _stateList;
    }
}