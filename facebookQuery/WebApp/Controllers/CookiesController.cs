using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class CookiesController : Controller
    {
        private readonly HomeService homeService;

        public CookiesController()
        {
            this.homeService = new HomeService(new JobService(), new AccountManager(), new AccountSettingsManager());
        }

        // GET: Cookies
        public ActionResult Index(long accountId)
        {
            var cookies = homeService.GetAccountCookies(accountId);
            return View(cookies);
        }

        [HttpPost]
        public ActionResult UpdateCookies(CookiesViewModel model)
        {
            homeService.UpdateCookies(model);
            return RedirectToAction("Index", "Cookies", new {accountId = model.AccountId});
        }
    }
}