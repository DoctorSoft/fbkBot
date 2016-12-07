using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Services;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class DeletedUsersController : Controller
    {
        private readonly HomeService homeService;

        public DeletedUsersController()
        {
            this.homeService = new HomeService();
        }

        // GET: Users
        public ActionResult Index()
        {
            var accounts = homeService.GetDeletedAccounts();
            return View(accounts);
        }

        public ActionResult RecoverAccount(long accountId)
        {
            homeService.RecoverAccount(accountId);
            return RedirectToAction("Index", "DeletedUsers");
        } 
    }
}