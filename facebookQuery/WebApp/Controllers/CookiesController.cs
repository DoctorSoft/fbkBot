using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Services;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class CookiesController : Controller
    {
        private readonly HomeService homeService;

        public CookiesController()
        {
            this.homeService = new HomeService();
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