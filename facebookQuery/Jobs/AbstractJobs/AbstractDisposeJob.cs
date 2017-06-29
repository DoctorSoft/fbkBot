using Jobs.JobsServices.JobServices;
using Jobs.Models;
using Services.Models.Jobs;
using Services.ServiceTools;

namespace Jobs.AbstractJobs
{
    public abstract class AbstractDisposeJob
    {
        protected virtual void CheckAccount(RunJobModel runModel)
        {
            var account = runModel.Account;
            var forSpy = runModel.ForSpy;
            object accountModel = null;

            if (!forSpy)
            {
                accountModel = new AccountManager().GetAccountById(account.Id);
            }
            else
            {
                accountModel = new SpyAccountManager().GetSpyAccountById(account.Id);
            }


            if (accountModel == null)
            {
                new JobService().RemoveAccountJobs(new RemoveAccountJobsModel
                {
                    AccountId = account.Id,
                    Login = account.Login,
                    IsForSpy = forSpy
                });
            }
        }
    }
}
