using Jobs.JobsService;
using Jobs.Notices;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.HomeModels;

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

            var spyAccounts = new SpyService(new JobService()).GetSpyAccounts();

            foreach (var spyAccount in spyAccounts)
            {
                var model = new AddOrUpdateAccountModel
                {
                    Account = new AccountViewModel
                    {
                        Id = spyAccount.Id,
                        FacebookId = spyAccount.FacebookId,
                        Login = spyAccount.Login,
                        Name = spyAccount.Name,
                        Proxy = spyAccount.Proxy,
                        ProxyLogin = spyAccount.ProxyLogin,
                        ProxyPassword = spyAccount.ProxyPassword,
                        Password = spyAccount.Password,
                        PageUrl = spyAccount.PageUrl,
                        Cookie = spyAccount.Cookie,
                        ConformationDataIsFailed = spyAccount.ConformationIsFailed,
                        AuthorizationDataIsFailed = spyAccount.AuthorizationDataIsFailed,
                        ProxyDataIsFailed = spyAccount.ProxyDataIsFailed
                    },
                    IsForSpy = true
                };

                new BackgroundJobService().AddOrUpdateAccountJobs(model);

                new JobService().AddOrUpdateSpyAccountJobs(model);
            }
        }
    }
}
