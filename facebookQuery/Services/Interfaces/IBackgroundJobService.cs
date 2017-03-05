using System;
using Constants.FunctionEnums;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;

namespace Services.Interfaces
{
    public interface IBackgroundJobService
    {
        bool AddOrUpdateAccountJobs(AccountViewModel account, GroupSettingsViewModel newSettings,
            GroupSettingsViewModel oldSettings);

        void CreateBackgroundJob(AccountViewModel account, FunctionName functionName, TimeSpan launchTime, bool checkPermissions);
    }
}
