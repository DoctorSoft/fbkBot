using System.Web.Mvc;
using Services.Services;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class CookiesController : Controller
    {
        private readonly HomeService _homeService;

        public CookiesController()
        {
            this._homeService = new HomeService();
        }

        // GET: Cookies
        public ActionResult Index(long accountId)
        {
            var cookies = _homeService.GetAccountCookies(accountId);
            return View(cookies);
        }

        [HttpPost]
        public ActionResult UpdateCookies(CookiesViewModel model)
        {
            _homeService.UpdateCookies(model);
            return RedirectToAction("Index", "Cookies", new {accountId = model.AccountId});
        }
    }
}