using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain.UnitsOfWork
{
    public interface ILinkDataUnitOfWork : IUnitOfWork
    {
        Task AddLinkData(LinkData entity, DateTime? expires = null);
    }
}