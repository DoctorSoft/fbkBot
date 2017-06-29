using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CommonInterfaces.Interfaces.Models;
using Jobs.JobsServices;
using Jobs.JobsServices.BackgroundJobServices;
using Jobs.JobsServices.JobServices;
using Services.Models.Jobs;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;
using Services.ViewModels.OptionsModel;
using AddOrUpdateAccountModel = Services.Models.BackgroundJobs.AddOrUpdateAccountModel;

namespace WebApp.Controllers
{
    public class OptionsController : Controller
    {
        private readonly MessageSettingService _messageSettingService;
        private readonly FriendsService _friendService;
        private readonly AccountManager _accountManager;

        public OptionsController()
        {
            _messageSettingService = new MessageSettingService();
            _friendService = new FriendsService(new NoticeService());
            _accountManager = new AccountManager();
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
            if (account == null)
            {
                return RedirectToAction("Index", new {accountId});
            }

            _messageSettingService.SetGroupMessages(accountId, groupId);

            if (account.ProxyDataIsFailed || account.AuthorizationDataIsFailed || account.ConformationDataIsFailed || account.IsDeleted)
            {
                return RedirectToAction("Index", new {accountId});
            }

            DeleteOldJobs(accountId);
            RefreshJobs(accountId, groupId);
            RefreshFriends(accountId);

            return RedirectToAction("Index", new { accountId });
        }

        private void RefreshFriends(long accountId)
        {
            var refreshFriendsTask = new Task<bool>(() => _friendService.GetCurrentFriends(new AccountViewModel { Id = accountId }));
            refreshFriendsTask.Start();
        }

        private static void DeleteOldJobs(long accountId)
        {
            new BackgroundJobService().RemoveAllBackgroundJobs(new RemoveAccountJobsModel
                {
                    IsForSpy = false,
                    AccountId = accountId
                });
        }

        private void RefreshJobs(long accountId, long groupId)
        {
            var account = _accountManager.GetAccountById(accountId);
            var settings = new GroupService(null).GetSettings(groupId);
            var model = new AddOrUpdateAccountModel
            {
                Account = account,
                OldSettings = null,
                NewSettings = settings
            };

            UpdateSettings(model);
        }

        private static bool UpdateSettings(IAddOrUpdateAccountJobs model)
        {
            new BackgroundJobService().AddOrUpdateAccountJobs(model);
           
            return true;
        }
     }
}