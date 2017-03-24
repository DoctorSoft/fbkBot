using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Notices;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.ScheduleDeleteFriendsModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class CreateTaskToDeleteFromFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            //получаем настройки
            var settings = new GroupService(new NoticesProxy()).GetSettings((long) account.GroupSettingsId);

            //Получаем таблицу
            var schedules = new ScheduleDeleteFriendsSerice().GetSchedules();

            //Удаляем данные из таблицы
            new ScheduleDeleteFriendsSerice().DeleteSchedules(schedules);

            foreach (var scheduleDeleteFriendsModel in schedules)
            {
                //Проверим не создана ли уже задача
                var jobsQueue = new JobQueueService().GetQueuesByAccountAndFunctionName(account.Id,
                    scheduleDeleteFriendsModel.FunctionName);

                if (jobsQueue.Count != 0)
                {
                    continue;
                }

                new BackgroundJobService().CreateBackgroundJobForDeleteFriends(new CreateBackgroundJobForDeleteFriendsModel
                {
                    Account = account,
                    FunctionName = scheduleDeleteFriendsModel.FunctionName,
                    LaunchHourTime = CalculateLauchTime(settings, scheduleDeleteFriendsModel),
                    Friend = new FriendViewModel
                    {
                        Id = scheduleDeleteFriendsModel.FriendId,
                        AddDateTime = scheduleDeleteFriendsModel.AddDateTime
                    }
                });

                //new JobStatusService().AddJobStatus(account.Id, scheduleDeleteFriendsModel.FunctionName,);
            }
        }

        private static int CalculateLauchTime(GroupSettingsViewModel settings, ScheduleDeleteFriendsModel deleteFriendModel)
        {

            var differenceTime = DateTime.Now - deleteFriendModel.AddDateTime;
            var settingTime = GetSettingsDataByFunctionName(settings, deleteFriendModel.FunctionName);
            var differenceHours = settingTime - differenceTime.Hours;

            return differenceHours;
        }

        private static int GetSettingsDataByFunctionName(GroupSettingsViewModel settings, FunctionName functionName)
        {
            switch (functionName)
            {
                case FunctionName.SendMessageToNewFriends:
                    break;
                case FunctionName.SendMessageToUnanswered:
                    break;
                case FunctionName.SendMessageToUnread:
                    break;
                case FunctionName.RefreshFriends:
                    break;
                case FunctionName.GetNewFriendsAndRecommended:
                    break;
                case FunctionName.ConfirmFriendship:
                    break;
                case FunctionName.SendRequestFriendship:
                    break;
                case FunctionName.AnalyzeFriends:
                    break;
                case FunctionName.RefreshCookies:
                    break;
                case FunctionName.JoinTheNewGroupsAndPages:
                    break;
                case FunctionName.InviteToGroups:
                    break;
                case FunctionName.InviteToPages:
                    break;
                case FunctionName.DialogIsOver:
                    return settings.DialogIsOverTimer;
                case FunctionName.IsAddedToGroupsAndPages:
                    return settings.IsAddedToGroupsAndPagesTimer;
                case FunctionName.IsWink:
                    return settings.IsWinkTimer;
                case FunctionName.IsWinkFriendsOfFriends:
                    return settings.IsWinkFriendsOfFriendsTimer;
                default:
                    throw new ArgumentOutOfRangeException("functionName");
            }
            return 0;
        }
    }
}
