namespace LeChuck.ReferralLinks.Application.StateMachines.Strategies
{
    public interface IStepStrategySelector<TContext, TEntity> where TEntity : class where TContext : class
    {
        IStrategy<TContext, TEntity> GetHandlerFor(string selectKey);
    }
}