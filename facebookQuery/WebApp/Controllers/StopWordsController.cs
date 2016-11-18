using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class StopWordsController : Controller
    {
        private readonly StopWordsService stopWordsService;

        public StopWordsController()
        {
            this.stopWordsService = new StopWordsService();
        }

        // GET: Groups
        /*public ActionResult Index()
        {
            var groups = stopWordsService.GetGroups();
            return View(groups);
        }

        [HttpPost]
        public ActionResult AddStopWord(string name)
        {
            stopWordsService.AddNewGroup(name);
            return RedirectToAction("Index", "Groups");
        }

        public ActionResult RemoveStopWord(long groupId)
        {
            stopWordsService.RemoveGroup(groupId);
            return RedirectToAction("Index", "Groups");
        }

        public ActionResult UpdateGroup(long groupId, string name)
        {
            stopWordsService.UpdateGroup(groupId, name);
            return RedirectToAction("Index", "Groups");
        }*/
    }
}