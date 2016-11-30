using System.Linq;
using System.Web.Mvc;
using Services.Services;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class UserEditorController : Controller
    {
        private readonly HomeService homeService;

        public UserEditorController()
        {
            this.homeService = new HomeService();
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
            var textList = new[] {model.Login, model.Name, model.PageUrl, model.Password};
            if (textList.Any(string.IsNullOrWhiteSpace))
            {
                return View("Index", model);
            }

            var accountId = homeService.AddOrUpdateAccount(model);

            return RedirectToAction("Index", "Users");
        }
    }
}