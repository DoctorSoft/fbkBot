using System.Linq;
using System.Web.Mvc;
using Jobs.JobsServices;
using Jobs.JobsServices.BackgroundJobServices;
using Jobs.JobsServices.JobServices;
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
                model.Password
            };
            if (textList.Any(string.IsNullOrWhiteSpace))
            {
                return View("Index", model);
            }

            spyService.AddOrUpdateSpyAccount(model, new BackgroundJobService());

            return RedirectToAction("Index", "Spies");
        }
    }
}