using System;
using System.Threading.Tasks;
using AutoMapper;
using LeChuck.ReferralLinks.DataAccess.Entities;
using LeChuck.ReferralLinks.DataAccess.Repositories;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Contracts.UnitsOfWork;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.DataAccess.UnitsOfWork
{
    public class ConfigUnitOfWork : IConfigUnitOfWork
    {
        private readonly IConfigRepository _repository;
        private readonly IMapper _mapper;

        public ConfigUnitOfWork(IConfigRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task SaveConfig(AppConfiguration config)
        {
            await _repository.SaveItemAsync(_mapper.Map<AppConfigDbEntity>(config));
        }

        public async Task<AppConfiguration> LoadConfig()
        {
            return _mapper.Map<AppConfiguration>(await _repository.LoadItemAsync(Constants.ConfigKey));
        }
    }
}
