using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class GroupsController : Controller
    {
        private readonly GroupService _groupService;

        public GroupsController()
        {
            this._groupService = new GroupService(null);
        }

        // GET: Groups
        public ActionResult Index()
        {
            var groups = _groupService.GetGroups();
            return View(groups);
        }

        [HttpPost]
        public ActionResult AddGroup(string name)
        {
            _groupService.AddNewGroup(name);
            return RedirectToAction("Index", "Groups");
        }

        public ActionResult RemoveGroup(long groupId)
        {
            _groupService.RemoveGroup(groupId);
            return RedirectToAction("Index", "Groups");
        }

        public ActionResult UpdateGroup(long groupId, string name)
        {
            _groupService.UpdateGroup(groupId, name);
            return RedirectToAction("Index", "Groups");
        }
    }
}