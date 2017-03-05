using Hangfire;
using Runner;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.RunnerJob
{
    public class RunnerJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            var runner = new ServiceRunner();
            var queueService = new JobQueueService();

            var queues = queueService.GetQueuesByAccountId(account.Id);

            foreach (var queue in queues)
            {
                runner.RunService(queue.FunctionName, account);
                queueService.RemoveQueue(queue.Id);
            }
        }
    }
}
