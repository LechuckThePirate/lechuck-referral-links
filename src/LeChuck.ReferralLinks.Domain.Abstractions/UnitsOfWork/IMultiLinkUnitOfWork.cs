#region using directives

using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

namespace LeChuck.ReferralLinks.Domain.UnitsOfWork
{
    public interface IMultiLinkUnitOfWork : IUnitOfWork
    {
        Task AddLinkData(MultiLinkMessage entity, DateTime? expires = null);
    }
}