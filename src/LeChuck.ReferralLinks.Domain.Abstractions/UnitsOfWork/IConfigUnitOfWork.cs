#region using directives

using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

namespace LeChuck.ReferralLinks.Domain.UnitsOfWork
{
    public interface IConfigUnitOfWork : IUnitOfWork
    {
        Task SaveConfig(AppConfiguration config);
        Task<AppConfiguration> LoadConfig();
    }
}