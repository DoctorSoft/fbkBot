using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class LinksController : Controller
    {
        private readonly LinksService linksService;

        public LinksController()
        {
            this.linksService = new LinksService();
        }

        // GET: Groups
        public ActionResult Index()
        {
            var links = linksService.GetLinks();
            return View(links);
        }

        [HttpPost]
        public ActionResult AddLink(string name)
        {
            linksService.AddNewLink(name);
            return RedirectToAction("Index", "Links");
        }

        public ActionResult RemoveLink(long linkId)
        {
            linksService.RemoveLink(linkId);
            return RedirectToAction("Index", "Links");
        }

        public ActionResult UpdateLink(long linkId, string name)
        {
            linksService.UpdateLink(linkId, name);
            return RedirectToAction("Index", "Links");
        }
    }
}