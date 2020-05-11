using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.DataAccess.Entities;
using LeChuck.ReferralLinks.DataAccess.Repositories;
using LeChuck.Stateless.StateMachine;

namespace LeChuck.ReferralLinks.Application.StateMachines
{
    public class StateMachineStore : IStateMachineStore
    {
        private readonly IStateMachineRepository _repository;

        public StateMachineStore(IStateMachineRepository repository)
        {
            _repository = repository;
        }

        public async Task<(Type type, string data)> RetrieveMachine(string machineId)
        {
            var entity = await _repository.LoadItemAsync(machineId);
            if (entity != null)
            {
                var type = Type.GetType(entity.Type);
                return (type, entity.Data);
            }

            return (null,null);
        }

        public async Task StoreMachine(string machineId, Type type, string machineData)
        {
            var entity = new StateMachineDbEntity
            {
                MachineId = machineId,
                Type = type.AssemblyQualifiedName,
                Data = machineData,
                TimeToLive = DateTime.Now.AddHours(1) // Todo : set by config
            };
            await _repository.SaveItemAsync(entity);
        }

        public async Task DeleteMachine(string machineId)
        {
            await _repository.DeleteItemByIdAsync(machineId);
        }
    }
}
