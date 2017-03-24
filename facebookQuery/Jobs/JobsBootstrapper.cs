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
            var accounts = new HomeService().GetAccounts();
            var groupService = new GroupService(new NoticesProxy());

            foreach (var account in accounts)
            {
                //not creating jobs for the account without group
                if (account.GroupSettingsId == null)
                {
                    continue;
                }

                var settings = groupService.GetSettings((long)account.GroupSettingsId);

                var model = new AddOrUpdateAccountModel
                {
                    Account = account,
                    NewSettings = settings,
                    OldSettings = null
                };

                new JobService().AddOrUpdateAccountJobs(model);

                new BackgroundJobService().AddOrUpdateAccountJobs(model);
            }
        }
    }
}
