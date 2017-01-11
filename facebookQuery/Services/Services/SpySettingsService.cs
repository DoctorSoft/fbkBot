using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Commands.SpyOptions;
using DataBase.QueriesAndCommands.Queries.Functions;
using DataBase.QueriesAndCommands.Queries.Groups;
using DataBase.QueriesAndCommands.Queries.SpyFunctions;
using Services.Core.Interfaces.ServiceTools;
using Services.Interfaces;
using Services.ServiceTools;
using Services.ViewModels.SpySettingsViewModels;

namespace Services.Services
{
    public class SpySettingsService
    {        
        private readonly ISpyAccountManager _spyAccountManager;
        private readonly IAccountManager _accountManager;
        private readonly IAccountSettingsManager _accountSettingsManager;
        private IJobService _jobService;

        public SpySettingsService(IJobService jobService)
        {
            _jobService = jobService;
            _spyAccountManager = new SpyAccountManager();
            _accountManager = new AccountManager();
            _accountSettingsManager = new AccountSettingsManager();
        }
        public SpySettingsViewModel GetSpySettings(long spyAccountId)
        {
            /*var statistics = _accountStatisticsManager.GetAccountStatistics(accountId);

            var detailedStatistic = new DetailedStatisticsModel()
            {
                AllTimeStatistic = _accountStatisticsManager.GetLastHourAccountStatistics(statistics),
                LastHourStatistic = _accountStatisticsManager.GetAllTimeAccountStatistics(statistics)
            };*/
            var functions = new GetFunctionsQueryHandler(new DataBaseContext()).Handle(new GetFunctionsQuery
            {
                ForSpy = true
            });
            var spyFunctions = new GetAllSpyFunctionByIdQueryHandler(new DataBaseContext()).Handle(new GetAllSpyFunctionByIdQuery
            {
                SpyId = spyAccountId
            });
            
            var result = new SpySettingsViewModel
            {
                SpyId = spyAccountId,
                SpyFunctions = functions.Select(func => new SpyFunctionViewModel
                {
                    Name = func.Name,
                    FunctionName = func.FunctionName,
                    FunctionId = func.FunctionId,
                    Assigned = spyFunctions.Any(spyFunction => spyFunction.FunctionId == func.FunctionId && spyFunction.SpyId == spyAccountId),
                    FunctionTypeName = func.FunctionTypeName,
                    TypeName = func.TypeName
                }).ToList()
            };

            return result;
        }

        public void UpdateSpyAccountSettings(NewSpySettingsViewModel newSettings)
        {
            new SaveSpyFunctionsCommandHandler(new DataBaseContext()).Handle(new SaveSpyFunctionsCommand
            {
                SpyId = newSettings.SpyId,
                Functions = newSettings.functions
            });
        }


    }
}
