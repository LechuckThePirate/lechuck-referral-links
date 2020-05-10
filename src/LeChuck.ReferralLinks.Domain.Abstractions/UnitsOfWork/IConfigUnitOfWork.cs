using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain.UnitsOfWork
{
    public interface IConfigUnitOfWork : IUnitOfWork
    {
        Task SaveConfig(AppConfiguration config);
        Task<AppConfiguration> LoadConfig();
    }
}
