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
        public ActionResult Index(long? accountId)
        {
            var result = messageSettingService.GetMessagesList(accountId);
            return View(result);
        }

        public ActionResult AddNewMessage(MessageViewModel model)
        {
            messageSettingService.SaveNewMessage(model);
            return RedirectToAction("Index", new { accountId = model.AccountId });
        }

        public ActionResult RemoveMessage(long messageId, long? accountId)
        {
            messageSettingService.RemoveMessage(messageId);
            return RedirectToAction("Index", new { accountId });
        }

        public ActionResult SetDefaultMessages(long accountId)
        {
            messageSettingService.SetDefaulMessages(accountId);
            return RedirectToAction("Index", new { accountId });
        }
     }
}