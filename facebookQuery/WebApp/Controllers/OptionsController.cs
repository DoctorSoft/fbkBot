using System;
using System.Web.Mvc;
using Services.ViewModels.OptionsModel;

namespace WebApp.Controllers
{
    public class OptionsController : Controller
    {
        // GET: Option
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewMessage(MessageViewModel model)
        {
            return RedirectToAction("Index");
        }
     }
}