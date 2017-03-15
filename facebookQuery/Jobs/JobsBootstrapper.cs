using Jobs.JobsService;
using Runner.Notices;
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
                //new JobService().AddOrUpdateAccountJobs(account);
                new BackgroundJobService().AddOrUpdateAccountJobs(account, settings, null);
            }
        }
    }
}
