using Runner.Interfaces;
using Services.ViewModels.HomeModels;

namespace Runner.Context
{
    public class RunnerContext
    {
        private readonly IRunner _runner;

        public RunnerContext(IRunner runner)
        {
            _runner = runner;
        }

        public void Execute(AccountViewModel account)
        {
            _runner.Run(account);
        }
    }
}
