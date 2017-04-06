using Jobs.JobsService;
using Jobs.Notices;
using Services.Models.BackgroundJobs;
using Services.Services;

namespace Jobs
{
    public static class JobsBootstrapper
    {
        public static void SetUpJobs()
        {
            //todo: uncomment it back
            var accountModels = new HomeService(new JobService(), new BackgroundJobService()).GetAccounts();
            var groupService = new GroupService(new NoticesProxy());

            foreach (var account in accountModels)
            {
                var settings = account.GroupSettingsId != null ? groupService.GetSettings((long)account.GroupSettingsId) : null;

                var model = new AddOrUpdateAccountModel
                {
                    Account = account,
                    NewSettings = settings,
                    OldSettings = null
                };

                new BackgroundJobService().AddOrUpdateAccountJobs(model);

                new JobService().AddOrUpdateAccountJobs(model);
            }
        }
    }
}
