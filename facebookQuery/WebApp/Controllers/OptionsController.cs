using System;
using System.Web.Mvc;
using Services.Services;
using Services.ViewModels.OptionsModel;

namespace WebApp.Controllers
{
    public class OptionsController : Controller
    {
        private readonly MessageSettingService messageSettingService;

        public OptionsController()
        {
            this.messageSettingService = new MessageSettingService();
        }

        // GET: Option
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewMessage(MessageViewModel model)
        {
            messageSettingService.SaveNewMessage(model);
            return RedirectToAction("Index");
        }
     }
}