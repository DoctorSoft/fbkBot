using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.SpySettingsViewModels;

namespace WebApp.Controllers
{
    public class SpyOptionsController : Controller
    {
        private readonly SpyService spyService;
        private readonly SpySettingsService spySettingsService;

        public SpyOptionsController()
        {
            this.spyService = new SpyService(new JobService());
            this.spySettingsService = new SpySettingsService(new JobService());
        }
        // GET: Option
        public ActionResult Index(long accountId)
        {
            var spySettings = spySettingsService.GetSpySettings(accountId);
            return View(spySettings);
        }
        
        [HttpPost]
        public ActionResult UpdateSpyOptions(NewSpySettingsViewModel newSettings)
        {
            spySettingsService.UpdateSpyAccountSettings(newSettings);
            return RedirectToAction("Index", "SpyOptions", new { accountId = newSettings.SpyId });
        }
     }
}