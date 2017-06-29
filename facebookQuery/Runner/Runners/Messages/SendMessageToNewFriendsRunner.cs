using Runner.Interfaces;
using Services.Services;
using Services.Services.FacebookMessagesService;

namespace Runner.Runners.Messages
{
    public class SendMessageToNewFriendsRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
            new FacebookMessagesService(new NoticeService()).SendMessageToNewFriends(account);
        }
    }
}
