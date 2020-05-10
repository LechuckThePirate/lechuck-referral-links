using System;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using LeChuck.ReferralLinks.DataAccess.Entities;
using LeChuck.ReferralLinks.DataAccess.Repositories;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.UnitsOfWork;

namespace LeChuck.ReferralLinks.DataAccess.UnitsOfWork
{
    public class LinkDataUnitOfWork : ILinkDataUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly ILinkDataRepository _repository;

        public LinkDataUnitOfWork(IMapper mapper, ILinkDataRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task AddLinkData(LinkData entity, DateTime? expires = null)
        {
            var dbEntity = _mapper.Map<LinkDataDbEntity>(entity);
            if (expires.HasValue)
                dbEntity.TimeToLive = expires.Value;

            await _repository.SaveItemAsync(dbEntity);
        }

    }
}
