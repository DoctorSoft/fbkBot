using System.Web.Mvc;
using Jobs.JobsServices;
using Jobs.JobsServices.JobServices;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class SpyCookiesController : Controller
    {
        private readonly SpyService spyService;

        public SpyCookiesController()
        {
            this.spyService = new SpyService(new JobService());
        }

        // GET: Cookies
        public ActionResult Index(long accountId)
        {
            var cookies = spyService.GetSpyAccountCookies(accountId);
            return View(cookies);
        }

        [HttpPost]
        public ActionResult UpdateCookies(CookiesViewModel model)
        {
            spyService.UpdateCookies(model);
            return RedirectToAction("Index", "SpyCookies", new {accountId = model.AccountId});
        }
    }
}