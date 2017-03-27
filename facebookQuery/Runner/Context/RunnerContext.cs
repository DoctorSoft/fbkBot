using Runner.Interfaces;

namespace Runner.Context
{
    public class RunnerContext
    {
        private readonly IRunner _runner;

        public RunnerContext(IRunner runner)
        {
            _runner = runner;
        }

        public void Execute(IRunnerModel model)
        {
            _runner.Run(model);
        }
    }
}
