using System.Linq;
using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.SpyAccountModels;

namespace WebApp.Controllers
{
    public class SpyEditorController : Controller
    {
        private readonly SpyService spyService;

        public SpyEditorController()
        {
            this.spyService = new SpyService(new JobService());
        }

        // GET: UserEditor
        public ActionResult Index(long? accountId)
        {
            var userData = spyService.GetSpyAccountById(accountId);

            return View(userData);
        }

        [HttpPost]
        public ActionResult UpdateUser(SpyAccountViewModel model)
        {
            var textList = new[]
            {
                model.Login, 
                model.Name, 
                model.PageUrl, 
                model.Password, 
                model.Proxy, 
                model.ProxyLogin, 
                model.ProxyPassword
            };
            if (textList.Any(string.IsNullOrWhiteSpace))
            {
                return View("Index", model);
            }

            spyService.AddOrUpdateSpyAccount(model);

            return RedirectToAction("Index", "Spies");
        }
    }
}