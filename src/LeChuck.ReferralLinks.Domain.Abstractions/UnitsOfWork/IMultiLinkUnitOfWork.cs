using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain.UnitsOfWork
{
    public interface IMultiLinkUnitOfWork : IUnitOfWork
    {
        Task AddLinkData(MultiLink entity, DateTime? expires = null);
    }
}