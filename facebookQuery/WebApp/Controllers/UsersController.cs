using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ServiceTools;

namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly HomeService homeService;

        public UsersController()
        {
            this.homeService = new HomeService(new JobService(), new AccountManager(), new AccountSettingsManager());
        }

        // GET: Users
        public ActionResult Index()
        {
            var accounts = homeService.GetAccounts();
            return View(accounts);
        }

        public ActionResult RemoveAccount(long accountId)
        {
            homeService.RemoveAccount(accountId);
            return RedirectToAction("Index", "Users");
        } 
    }
}