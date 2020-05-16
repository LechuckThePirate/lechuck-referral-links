#region using directives

using System.Threading.Tasks;

#endregion

namespace LeChuck.ReferralLinks.Application.Views
{
    public interface IView<TEntity> where TEntity : class
    {
        Task SendView(long chatId, TEntity data);
    }
}