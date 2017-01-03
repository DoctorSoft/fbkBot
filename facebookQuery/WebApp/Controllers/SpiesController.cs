using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ServiceTools;

namespace WebApp.Controllers
{
    public class SpiesController : Controller
    {
        private readonly SpyService spyService;

        public SpiesController()
        {
            this.spyService = new SpyService(new JobService());
        }

        // GET: Users
        public ActionResult Index()
        {
            var accounts = spyService.GetSpyAccounts();
            return View(accounts);
        }

//        public ActionResult RemoveAccount(long accountId)
//        {
//            homeService.RemoveAccount(accountId);
//            return RedirectToAction("Index", "Users");
//        } 
    }
}