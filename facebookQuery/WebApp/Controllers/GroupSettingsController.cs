using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.GroupModels;

namespace WebApp.Controllers
{
    public class GroupSettingsController : Controller
    {       
        private readonly GroupService groupService;

        public GroupSettingsController()
        {
            this.groupService = new GroupService();
        }
        // GET: GroupSettings
        public ActionResult Index(long groupId)
        {
            var groups = groupService.GetSettings(groupId);
            return View(groups);
        }

        [HttpPost]
        public ActionResult UpdateGroupSettings(GroupSettingsViewModel settings)
        {
            groupService.UpdateSettings(settings, new JobService());
            return RedirectToAction("Index", "Groups", new { groupId = settings.GroupId });
        }
    }
}