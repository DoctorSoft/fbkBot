using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class GroupsController : Controller
    {
        private readonly GroupService groupService;

        public GroupsController()
        {
            this.groupService = new GroupService();
        }

        // GET: Groups
        public ActionResult Index()
        {
            var groups = groupService.GetGroups();
            return View(groups);
        }

        [HttpPost]
        public ActionResult AddGroup(string name)
        {
            groupService.AddNewGroup(name);
            return RedirectToAction("Index", "Groups");
        }

        public ActionResult RemoveGroup(long groupId)
        {
            groupService.RemoveGroup(groupId);
            return RedirectToAction("Index", "Groups");
        }

        public ActionResult UpdateGroup(long groupId, string name)
        {
            groupService.UpdateGroup(groupId, name);
            return RedirectToAction("Index", "Groups");
        }
    }
}