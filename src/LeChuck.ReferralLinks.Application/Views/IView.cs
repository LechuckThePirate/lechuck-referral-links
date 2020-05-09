using System.Threading.Tasks;

namespace LeChuck.ReferralLinks.Application.Views
{
    public interface IView<TEntity> where TEntity : class
    {
        Task SendView(long chatId, TEntity data);
    }
}