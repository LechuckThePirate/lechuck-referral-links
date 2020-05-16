#region using directives

using System;
using System.Threading.Tasks;
using AutoMapper;
using LeChuck.ReferralLinks.DataAccess.Entities;
using LeChuck.ReferralLinks.DataAccess.Repositories;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.UnitsOfWork;

#endregion

namespace LeChuck.ReferralLinks.DataAccess.UnitsOfWork
{
    public class MultiLinkUnitOfWork : IMultiLinkUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly IMultiLinkRepository _repository;

        public MultiLinkUnitOfWork(IMapper mapper, IMultiLinkRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task AddLinkData(MultiLink entity, DateTime? expires = null)
        {
            var dbEntity = _mapper.Map<MultiLinkDbEntity>(entity);
            if (expires.HasValue)
                dbEntity.TimeToLive = expires.Value;

            await _repository.SaveItemAsync(dbEntity);
        }
    }
}