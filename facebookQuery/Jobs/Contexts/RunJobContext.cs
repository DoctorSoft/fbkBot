using Jobs.Interfaces;

namespace Jobs.Contexts
{
    public class RunJobContext
    {
        private readonly IRunJob _runJob;

        public RunJobContext(IRunJob runJob)
        {
            _runJob = runJob;
        }

        public void Execute(IRunJobModel model)
        {
            _runJob.Run(model);
        }
    }
}
