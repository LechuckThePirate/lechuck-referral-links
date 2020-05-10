namespace LeChuck.ReferralLinks.Application.StateMachines.Strategies
{
    public interface IResponseStrategySelector<TContext, TEntity> where TEntity : class where TContext : class
    {
        bool CanHandle(string key);
        IStrategy<TContext, TEntity> GetResponseHandlerFor(string key);
    }
}