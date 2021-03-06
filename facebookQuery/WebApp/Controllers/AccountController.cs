﻿using System.Web.Mvc;
using Jobs.JobsServices;
using Jobs.JobsServices.BackgroundJobServices;
using Jobs.JobsServices.JobServices;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HomeService _homeService;

        public AccountController()
        {
            this._homeService = new HomeService(new JobService(), new BackgroundJobService());
        }
        // GET: Account
        public ActionResult Index(long accountId)
        {
            var currentAccount = _homeService.GetAccountSettings(accountId);
            
            return View(currentAccount);
        }

        [HttpPost]
        public ActionResult UpdateOptionsSettings(AccountSettingsViewModel options)
        {
            return RedirectToAction("Index", "Users", new { accountId = options.Account.Id });
        }
    }
}