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
        public ActionResult Index(long? accountId, long? groupId)
        {
            var result = messageSettingService.GetMessagesList(accountId, groupId);
            return View(result);
        }

        public ActionResult AddNewMessage(MessageViewModel model)
        {
            messageSettingService.SaveNewMessage(model);
            return RedirectToAction("Index", new { accountId = model.AccountId});
            //return RedirectToAction("Index", new { accountId = model.AccountId, groupId = model.GroupId });
        }

        public ActionResult RemoveMessage(long messageId, long? accountId, long? groupId)
        {
            messageSettingService.RemoveMessage(messageId);
            return RedirectToAction("Index", new { accountId, groupId });
        }

        public ActionResult SetDefaultMessages(long accountId)
        {
            messageSettingService.SetDefaulMessages(accountId);
            return RedirectToAction("Index", new { accountId });
        }

        public ActionResult SetGroupMessages(long accountId, long groupId)
        {
            messageSettingService.SetGroupMessages(accountId, groupId);
            return RedirectToAction("Index", new { accountId });
        }
     }
}