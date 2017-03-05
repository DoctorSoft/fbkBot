using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class ExtraMessagesController : Controller
    {
        // GET: ExtraMessages
        private readonly ExtraMessagesService extraMessagesService;

        public ExtraMessagesController()
        {
            this.extraMessagesService = new ExtraMessagesService();
        }

        // GET: StopWords
        public ActionResult Index()
        {
            var stopWords = extraMessagesService.GetExtraMessages();
            return View(stopWords);
        }

        [HttpPost]
        public ActionResult AddExtraMessage(string name)
        {
            extraMessagesService.AddNewExtraMessage(name);
            return RedirectToAction("Index", "ExtraMessages");
        }

        public ActionResult RemoveExtraMessage(long extraMessageId)
        {
            extraMessagesService.RemoveExtraMessage(extraMessageId);
            return RedirectToAction("Index", "ExtraMessages");
        }

        public ActionResult UpdateExtraMessage(long extraMessageId, string name)
        {
            extraMessagesService.UpdateExtraMessage(extraMessageId, name);
            return RedirectToAction("Index", "ExtraMessages");
        }
    }
}