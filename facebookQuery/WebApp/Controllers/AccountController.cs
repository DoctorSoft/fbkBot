﻿using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HomeService homeService;

        public AccountController()
        {
            this.homeService = new HomeService();
        }
        // GET: Account
        public ActionResult Index(long accountId)
        {
            var currentAccount = homeService.GetAccountByUserId(accountId);

            var status = homeService.GetNewNotices(currentAccount.UserId);

            currentAccount.NumberNewFriends = status.NumberNewFriends;
            currentAccount.NumberNewMessages = status.NumberNewMessages;
            currentAccount.NumberNewNotifications = status.NumberNewNotifications;
            currentAccount.NewMessagesList = homeService.GetUnreadMessages(currentAccount.UserId);
            
            return View(currentAccount);
        }
    }
}