using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Runner.Notices;
using Services.Services;
using Services.ViewModels.HomeModels;
using Services.ViewModels.QueueViewModels;

namespace Runner
{
    public class Program
    {
        private static JobQueueService _queueService;
        private const int Delay = 5000;

        public static void Main(string[] args)
        {
            var proxyHub = new NoticesProxy();
            _queueService = new JobQueueService();

            int i = 1;

            while (true)
            {
                proxyHub.AddNotice(1, i);
                Console.WriteLine(string.Format("{0} запуск", i));
                var accounts = new HomeService().GetAccounts();

                var queues = _queueService.GetGroupedQueue();

                Console.WriteLine(string.Format("{0} аккаунтов в очереди", queues.Count));
                foreach (var queue in queues)
                {
                    var firstElement = queue.FirstOrDefault();
                    if (firstElement == null)
                    {
                        continue;
                    }

                    Console.WriteLine(string.Format("{0} задач в очереди для аккаунта {1}", queues.Count, firstElement.AccountId));

                    var account = accounts.FirstOrDefault(model => model.Id == firstElement.AccountId);

                    foreach (var currentQueue in queue)
                    {
                        _queueService.RemoveQueue(currentQueue.Id);
                    }

                    var queueData = queue;
                    var runnerTask = new Task(() => RunnerTask(queueData, account));

                    runnerTask.Start();
                }

                Console.WriteLine(string.Format("ждем {0} сек. для следующего запуска", Delay/1000));
                Thread.Sleep(Delay);
                i++;
            }
        }

        private static void RunnerTask(IEnumerable<QueueViewModel> queues, AccountViewModel account)
        {
            var runner = new ServiceRunner();

            foreach (var queue in queues)
            {
                runner.RunService(queue.FunctionName, account);
            }
        }
    }
}
