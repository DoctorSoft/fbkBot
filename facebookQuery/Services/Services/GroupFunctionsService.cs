using System.Collections.Generic;
using System.Linq;
using CommonInterfaces.Interfaces.Services;
using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Functions;
using DataBase.QueriesAndCommands.Queries.Groups;
using Services.Models.BackgroundJobs;
using Services.ServiceTools;
using Services.ViewModels.GroupFunctionsModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.JobStatusModels;

namespace Services.Services
{
    public class GroupFunctionsService
    {
        private readonly SettingsManager _settingsManager;

        public GroupFunctionsService()
        {
            _settingsManager = new SettingsManager();
        }

        public List<GroupFunctionViewModel> GetGroupFunctions()
        {
            var functions = new GetFunctionsQueryHandler(new DataBaseContext()).Handle(new GetFunctionsQuery
            {
                ForSpy = false
            });
            var groups = new GetGroupsQueryHandler(new DataBaseContext()).Handle(new GetGroupsQuery());
            var groupFunctions = new GroupFunctionsQueryHandler(new DataBaseContext()).Handle(new GroupFunctionsQuery());

            var result = groups.Select(group => new GroupFunctionViewModel
            {
                GroupId = group.Id,
                GroupName = group.Name,
                Functions = functions.Select(function => new FunctionViewModel
                {
                    Name = function.Name,
                    FunctionName = function.FunctionName,
                    FunctionId = function.FunctionId,
                    Assigned = groupFunctions.Any(groupFunction => groupFunction.FunctionId == function.FunctionId && groupFunction.GroupId == group.Id),
                    FunctionTypeName = function.FunctionTypeName,
                    TypeName = function.TypeName
                }).ToList()
            }).ToList();

            return result;
        }

        public void SaveGroupFunctions(long groupId, List<long> funtions, IBackgroundJobService backgroundJobService)
        {
            var functionsIdForRun = new List<FunctionName>();

            var oldFuntions = new GetGroupFunctionsByGroupIdQueryHandler(new DataBaseContext()).Handle(new GetGroupFunctionsByGroupIdQuery
            {
                GroupId = groupId
            });

            new SaveGroupFunctionsCommandHandler(new DataBaseContext()).Handle(new SaveGroupFunctionsCommand
            {
                GroupId = groupId,
                Functions = funtions
            });

            var newFunctions = new GetGroupFunctionsByGroupIdQueryHandler(new DataBaseContext()).Handle(new GetGroupFunctionsByGroupIdQuery
            {
                GroupId = groupId
            });

            foreach (var funtion in newFunctions)
            {
                if (oldFuntions.Any(data => data.FunctionId == funtion.FunctionId))
                {
                    continue;
                }

                if (oldFuntions.All(data => data.FunctionId != funtion.FunctionId))
                {
                    functionsIdForRun.Add(funtion.FunctionName);
                }
            }

            //if (functionsIdForRun.Count != 0)
            //{
                var accounts = new GetAccountsByGroupSettingsIdQueryHandler(new DataBaseContext()).Handle(new GetAccountsByGroupSettingsIdQuery
                {
                    GroupSettingsId = groupId
                });

                var accountsViewModel = accounts.Select(model => new AccountViewModel
                {
                    Id = model.Id,
                    PageUrl = model.PageUrl,
                    Login = model.Login,
                    Password = model.Password,
                    FacebookId = model.FacebookId,
                    Proxy = model.Proxy,
                    ProxyLogin = model.ProxyLogin,
                    ProxyPassword = model.ProxyPassword,
                    Cookie = model.Cookie.CookieString,
                    Name = model.Name,
                    GroupSettingsId = model.GroupSettingsId,
                    AuthorizationDataIsFailed = model.AuthorizationDataIsFailed,
                    ProxyDataIsFailed = model.ProxyDataIsFailed,
                    IsDeleted = model.IsDeleted,
                    UserAgentId = model.UserAgentId
                }).ToList();

                foreach (var accountModel in accountsViewModel)
                {
                    //удаляем выключенные задачи
                    foreach (var oldFuntion in oldFuntions) 
                    {
                        if (newFunctions.All(data => data.FunctionId != oldFuntion.FunctionId))
                        {
                            var statuses = new JobStatusManager().GetJobStatusesByAccountAndFunctionId(accountModel.Id,
                                oldFuntion.FunctionName, false);

                            foreach (var jobStatusViewModel in statuses)
                            {
                                new JobStatusService().DeleteJobStatusesByAccountId(new JobStatusViewModel
                                {
                                    AccountId = accountModel.Id,
                                    IsForSpy = false,
                                    FunctionName = oldFuntion.FunctionName
                                });
                                backgroundJobService.RemoveJobById(jobStatusViewModel.JobId);
                            }
                        }
                    }
                    foreach (var function in functionsIdForRun)
                    {
                        var delayTime = _settingsManager.GetTimeSpanByFunctionName(function, groupId);

                        var model = new CreateBackgroundJobModel()
                        {
                            Account = accountModel,
                            CheckPermissions = true,
                            FunctionName = function,
                            LaunchTime = delayTime
                        };

                        backgroundJobService.CreateBackgroundJob(model);
                    }
                //}
            }
        }
    }
}
