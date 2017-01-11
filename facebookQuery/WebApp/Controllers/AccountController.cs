using System.Web.Mvc;
using CommonModels;
using Jobs.JobsService;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HomeService homeService;

        public AccountController()
        {
            this.homeService = new HomeService(new JobService(), new AccountManager(), new AccountSettingsManager());
        }
        // GET: Account
        public ActionResult Index(long accountId)
        {
            var currentAccount = homeService.GetAccountSettings(accountId);
            
            return View(currentAccount);
        }

        [HttpPost]
        public ActionResult UpdateOptionsSettings(AccountSettingsViewModel options)
        {
            options.Settings.AccountId = options.Account.Id;

            homeService.UpdateAccountSettings(options.Settings);
            return RedirectToAction("Index", "Users", new { accountId = options.Account.Id });
        }
    }
}