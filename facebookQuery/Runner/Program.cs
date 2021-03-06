﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobs.JobsServices;
using Jobs.JobsServices.BackgroundJobServices;
using Jobs.JobsServices.JobServices;
using Runner.Models;
using Services.Services;
using Services.Services.Runners;
using Services.ViewModels.HomeModels;
using Services.ViewModels.QueueViewModels;
using Services.ViewModels.RunnerModels;

namespace Runner
{
    public class Program
    {
        private static JobQueueService _queueService;
        private static RunnerService _runnerService;
        private static RunnerPermissionService _runnerPermissionService;
        private const int Delay = 10000;
        private const int OverdueMin = 10;
        private static long _runnerId = 0;

        public static void Main(string[] args)
        {
            _queueService = new JobQueueService();
            _runnerService = new RunnerService();
            _runnerPermissionService = new RunnerPermissionService();

            var i = 1;
            AddRunnerInfo();

            while (true)
            {
                if (!_runnerPermissionService.HasPermissions(_runnerId))
                {
                    Console.WriteLine(string.Format("Запуск запрещён"));
                    Thread.Sleep(Delay);
                    continue;
                }
                try
                {
                    Console.WriteLine(string.Format("{0} запуск", i));

                    var accounts = new HomeService(new JobService(), new BackgroundJobService()).GetAccounts();
                    var spyAccounts = new SpyService(new JobService()).GetSpyAccounts();

                    var overdueQueues = _queueService.RemoveOverdueQueue(OverdueMin);
                    if (overdueQueues != 0)
                    {
                        Console.WriteLine(string.Format("Из очереди исключено {0} задач с превышением времени выполнения ({1} min.)", overdueQueues, OverdueMin));
                    }

                    var queues = _queueService.GetGroupedQueue();

                    Console.WriteLine(string.Format("{0} аккаунтов в очереди", queues.Count));
                    foreach (var queue in queues)
                    {
                        var firstElement = queue.FirstOrDefault();
                        if (firstElement == null)
                        {
                            continue;
                        }

                        Console.WriteLine(string.Format("{0} задач в очереди для аккаунта {1}", queue.Count,
                            firstElement.AccountId));

                        AccountViewModel account;
                        if (firstElement.IsForSpy)
                        {
                            var spyAccount = spyAccounts.FirstOrDefault(model => model.Id == firstElement.AccountId);
                            if (spyAccount == null)
                            {
                                continue;
                            }

                            account = new AccountViewModel
                            {
                                Id = spyAccount.Id,
                                Login = spyAccount.Login,
                                Proxy = spyAccount.Proxy,
                                FacebookId = spyAccount.FacebookId,
                                ProxyLogin = spyAccount.ProxyLogin,
                                Name = spyAccount.Name,
                                ProxyPassword = spyAccount.ProxyPassword,
                                ConformationDataIsFailed = spyAccount.ConformationIsFailed,
                                AuthorizationDataIsFailed = spyAccount.AuthorizationDataIsFailed,
                                ProxyDataIsFailed = spyAccount.ProxyDataIsFailed,
                                Cookie = spyAccount.Cookie,
                                PageUrl = spyAccount.PageUrl,
                                Password = spyAccount.Password
                            };
                        }
                        else
                        {
                            account = accounts.FirstOrDefault(model => model.Id == firstElement.AccountId);
                        }

                        foreach (var currentQueue in queue)
                        {
                            _queueService.MarkIsProcessedQueue(currentQueue); // помечаем задачу как "в процессе"
                        }

                        var queueData = queue;
                        var runnerTask = new Task(() => RunnerTask(queueData, account));

                        runnerTask.Start();
                    }

                    Console.WriteLine($"ждем {Delay / 1000} сек. для следующего запуска");
                    Thread.Sleep(Delay);
                    i++;

                    _runnerService.UpdateRunnerActivityDate(_runnerId);
                }
                catch (Exception ex)
                {
                    LogWriter.LogWriter.AddToLog(string.Format("{0} ({1})", ex.Message, ex.InnerException));
                }
            }
        }

        private static void RunnerTask(IEnumerable<JobQueueViewModel> queues, AccountViewModel account)
        {
            var runner = new ServiceRunner();

            foreach (var queue in queues)
            {
                var friend = queue.FriendId != null ? new FriendsService(new NoticeService()).GetFriendById((long)queue.FriendId) : null;
                runner.RunService(queue.FunctionName, new RunnerModel
                {
                    Account = account,
                    Friend = friend,
                    ForSpy = queue.IsForSpy
                });

                //удаляем задачу
                _queueService.RemoveQueue(queue.Id);
            }
        }

        private static void AddRunnerInfo()
        {
            var deviceName = Environment.MachineName;

            Console.WriteLine($"Регистрируем устройство {deviceName}");

            var model = new RunnerViewModel
            {
                DeviceName = deviceName
            };

            _runnerId = _runnerService.AddRunner(model);
        }
    }
}
