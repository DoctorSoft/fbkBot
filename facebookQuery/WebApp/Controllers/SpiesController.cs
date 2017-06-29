using System.Web.Mvc;
using Jobs.JobsServices;
using Jobs.JobsServices.JobServices;
using Services.Services;

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

        public ActionResult RemoveSpyAccount(long spyAccountId)
        {
            spyService.RemoveSpyAccount(spyAccountId);
            return RedirectToAction("Index", "Spies");
        } 
    }
}