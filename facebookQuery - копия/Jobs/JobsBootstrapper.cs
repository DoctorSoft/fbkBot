using Jobs.JobsService;
using Services.Services;

namespace Jobs
{
    public static class JobsBootstrapper
    {
        public static void SetUpJobs()
        {
            //todo: uncomment it back
            var accounts = new HomeService().GetAccounts();
            var groupService = new GroupService();

            foreach (var account in accounts)
            {
                //not creating jobs for the account without group
                if (account.GroupSettingsId == null)
                {
                    continue;
                }

                var settings = groupService.GetSettings((long)account.GroupSettingsId);
                new BackgroundJobService().AddOrUpdateAccountJobs(account, settings, null);
            }
        }
    }
}
