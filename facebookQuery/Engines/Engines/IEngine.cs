namespace Engines.Engines
{
    public interface IEngine<TModel, TResult>
    {
        TResult Execute(TModel model);
    }
}
