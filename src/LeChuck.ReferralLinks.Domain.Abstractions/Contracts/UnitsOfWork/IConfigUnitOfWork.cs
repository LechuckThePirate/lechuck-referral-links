using System.Threading.Tasks;
using LeChuck.DataAccess.DynamoDb;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain.Contracts.UnitsOfWork
{
    public interface IConfigUnitOfWork : IUnitOfWork
    {
        Task SaveConfig(AppConfiguration config);
        Task<AppConfiguration> LoadConfig();
    }
}
