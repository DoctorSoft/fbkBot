using System.Linq;
using System.Web.Mvc;
using Jobs.JobsService;
using Services.Core.Interfaces.ServiceTools;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class UserEditorController : Controller
    {
        private readonly HomeService homeService;

        public UserEditorController()
        {
            this.homeService = new HomeService(new JobService(), new AccountManager());
        }

        // GET: UserEditor
        public ActionResult Index(long? accountId)
        {
            var userData = homeService.GetAccountById(accountId);

            return View(userData);
        }

        [HttpPost]
        public ActionResult UpdateUser(AccountDraftViewModel model)
        {
            var textList = new[] {model.Login, model.Name, model.PageUrl, model.Password, model.Proxy, model.ProxyLogin, model.ProxyPassword};
            if (textList.Any(string.IsNullOrWhiteSpace))
            {
                return View("Index", model);
            }

            var accountId = homeService.AddOrUpdateAccount(model);

            return RedirectToAction("Index", "Users");
        }
    }
}