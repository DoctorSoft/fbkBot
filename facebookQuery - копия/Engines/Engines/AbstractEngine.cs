using System;

namespace Engines.Engines
{
    public abstract class AbstractEngine<TModel, TResult> : IEngine<TModel, TResult>
        where TResult : new()
    {
        protected abstract TResult ExecuteEngine(TModel model);

        public TResult Execute(TModel model)
        {
            TResult result;

            try
            {
                result = ExecuteEngine(model);
            }
            catch (Exception ex)
            {
                // todo: Log it
                result = new TResult();
            }

            return result;
        }
    
    }
}
