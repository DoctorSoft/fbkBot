using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly HomeService homeService;

        public UsersController()
        {
            this.homeService = new HomeService(new JobService(), new AccountManager());
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