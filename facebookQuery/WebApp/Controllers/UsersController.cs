using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Services;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly HomeService homeService;

        public UsersController()
        {
            this.homeService = new HomeService();
        }

        // GET: Users
        public ActionResult Index()
        {
            var accounts = homeService.GetAccounts();
            return View(accounts);
        }

        public ActionResult RemoveAccount(long accountId)
        {
            return RedirectToAction("Index", "Users");
        } 
    }
}