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
        public ActionResult Index()
        {
            var stopWords = stopWordsService.GetStopWords();
            return View(stopWords);
        }

        [HttpPost]
        public ActionResult AddStopWord(string name)
        {
            stopWordsService.AddNewStopWord(name);
            return RedirectToAction("Index", "StopWords");
        }

        public ActionResult RemoveStopWord(long stopWordId)
        {
            stopWordsService.RemoveStopWord(stopWordId);
            return RedirectToAction("Index", "StopWords");
        }

        public ActionResult UpdateStopWord(long stopWordId, string name)
        {
            stopWordsService.UpdateStopWord(stopWordId, name);
            return RedirectToAction("Index", "StopWords");
        }
    }
}