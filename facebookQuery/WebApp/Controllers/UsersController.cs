using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using Jobs.JobsServices;
using Jobs.JobsServices.BackgroundJobServices;
using Jobs.JobsServices.JobServices;
using Newtonsoft.Json;
using Services.Services;
using Services.ViewModels.HomeModels;
using Services.ViewModels.NoticeModels;

namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly HomeService _homeService;

        public UsersController()
        {
            this._homeService = new HomeService(new JobService(), new BackgroundJobService());
        }
        
        public ActionResult Index(bool? showWorking, bool? showWithErrors)
        {
            List<AccountViewModel> accounts;
            var acountsData = new List<AccountDataViewModel>();

            if (showWorking == null && showWithErrors == null)
            {
                accounts = _homeService.GetAccounts();
                acountsData = _homeService.GetDataAccounts(accounts);

                return View(acountsData);
            }

            if (showWorking != null && (bool)showWorking)
            {
                accounts = _homeService.GetWorkAccounts();
                acountsData = _homeService.GetDataAccounts(accounts);
            }

            if (showWithErrors != null && (bool)showWithErrors)
            {
                accounts = _homeService.GetAccountsWithErrors();
                acountsData = _homeService.GetDataAccounts(accounts);
            }

            return View(acountsData);
        }

        public ActionResult RemoveAccount(long accountId)
        {
            _homeService.RemoveAccount(accountId);
            return RedirectToAction("Index", "Users");
        }

        public JsonResult GetPushMessages()
        {
            var messages = new NoticeService().GetLastNotices(5);

            var result = JsonConvert.SerializeObject(messages);

            return new JsonResult
            {
                Data = result
            };
        }
    }
}