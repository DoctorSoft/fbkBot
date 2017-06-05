using System.Linq;
using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.AccountModels;

namespace WebApp.Controllers
{
    public class UserEditorController : Controller
    {
        private readonly HomeService _homeService;

        public UserEditorController()
        {
            this._homeService = new HomeService(new JobService(), new BackgroundJobService());
        }

        // GET: UserEditor
        public ActionResult Index(long? accountId)
        {
            var userData = _homeService.GetAccountById(accountId);

            return View(userData);
        }

        [HttpPost]
        public ActionResult UpdateUser(AccountDraftViewModel model)
        {
            var textList = new[] {model.Login, model.Name, model.Password};
            if (textList.Any(string.IsNullOrWhiteSpace))
            {
                return View("Index", model);
            }
            
            var accountId = _homeService.AddOrUpdateAccount(model, new BackgroundJobService());

            return RedirectToAction("Index", "Users");
        }
    }
}