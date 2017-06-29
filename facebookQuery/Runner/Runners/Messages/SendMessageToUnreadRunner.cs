using Runner.Interfaces;
using Services.Services;
using Services.Services.FacebookMessagesService;

namespace Runner.Runners.Messages
{
    public class SendMessageToUnreadRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
            new FacebookMessagesService(new NoticeService()).SendMessageToUnread(account);
        }
    }
}
