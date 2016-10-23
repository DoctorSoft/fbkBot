using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService homeService;

        public HomeController()
        {
            this.homeService = new HomeService();
        }

        public ActionResult Index()
        {
            var accounts = homeService.GetAccounts();
            homeService.SendMessage();
            return View(accounts);
        }
    }
}