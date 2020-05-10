using System.Threading.Tasks;

namespace LeChuck.ReferralLinks.Application.StateMachines
{
    public interface IStrategy<TContext, TEntity> where TEntity : class where TContext : class
    {
        bool CanHandle(string key);
        Task<bool> Handle(TContext context, TEntity entity);
    }
}