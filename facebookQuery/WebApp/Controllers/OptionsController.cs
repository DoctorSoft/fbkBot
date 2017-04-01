using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CommonInterfaces.Interfaces.Models;
using Jobs.JobsService;
using Jobs.Notices;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.HomeModels;
using Services.ViewModels.OptionsModel;

namespace WebApp.Controllers
{
    public class OptionsController : Controller
    {
        private readonly MessageSettingService _messageSettingService;
        private readonly FriendsService _friendService;

        public OptionsController()
        {
            _messageSettingService = new MessageSettingService();
            _friendService = new FriendsService(new NoticesProxy());
        }

        // GET: Option
        public ActionResult Index(long? accountId, long? groupId)
        {
            var result = _messageSettingService.GetMessagesList(accountId, groupId);
            return View(result);
        }

        public ActionResult AddNewMessage(MessageViewModel model)
        {
            _messageSettingService.SaveNewMessage(model);

            return model.AccountId != null 
                ? RedirectToAction("Index", new {accountId = model.AccountId}) 
                : RedirectToAction("Index", new { groupId = model.GroupId });
        }

        public ActionResult RemoveMessage(long messageId, long? accountId, long? groupId)
        {
            _messageSettingService.RemoveMessage(messageId);
            return accountId != null
            ? RedirectToAction("Index", new { accountId })
            : RedirectToAction("Index", new { groupId });
        }

        public ActionResult SetDefaultMessages(long accountId)
        {
            _messageSettingService.SetDefaulMessages(accountId);

            return RedirectToAction("Index", new { accountId });
        }

        public ActionResult SetGroupMessages(long accountId, long groupId)
        {
            var account = new HomeService(new JobService(), new BackgroundJobService()).GetAccounts().FirstOrDefault(model => model.Id == accountId);

            _messageSettingService.SetGroupMessages(accountId, groupId);

            if (account != null && !account.ProxyDataIsFailed && !account.AuthorizationDataIsFailed && !account.IsDeleted)
            {
                RefreshFriends(accountId);
                RefreshJobs(account, groupId);
            }

            return RedirectToAction("Index", new { accountId });
        }

        private void RefreshFriends(long accountId)
        {
            var refreshFriendsTask = new Task<bool>(() => _friendService.GetFriendsOfFacebook(new AccountViewModel { Id = accountId }));
            refreshFriendsTask.Start();
        }
        private static void RefreshJobs(AccountViewModel account, long groupId)
        {
            var settings = new GroupService(null).GetSettings(groupId);
            var model = new AddOrUpdateAccountModel
            {
                Account = account,
                OldSettings = null,
                NewSettings = settings
            };

            var refreshJobsTask = new Task<bool>(() => UpdateSettings(model));
            refreshJobsTask.Start();
        }

        private static bool UpdateSettings(IAddOrUpdateAccountJobs model)
        {
            new BackgroundJobService().AddOrUpdateAccountJobs(model);
           
            return true;
        }
     }
}