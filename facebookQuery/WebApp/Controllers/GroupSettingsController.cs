using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.GroupModels;

namespace WebApp.Controllers
{
    public class GroupSettingsController : Controller
    {       
        private readonly GroupService _groupService;

        public GroupSettingsController()
        {
            this._groupService = new GroupService(null);
        }
        // GET: GroupSettings
        public ActionResult Index(long groupId)
        {
            var groups = _groupService.GetSettings(groupId);
            return View(groups);
        }

        [HttpPost]
        public ActionResult UpdateGroupSettings(GroupSettingsViewModel settings)
        {
            _groupService.UpdateSettings(settings, new BackgroundJobService());
            return RedirectToAction("Index", "Groups", new { groupId = settings.GroupId });
        }
    }
}