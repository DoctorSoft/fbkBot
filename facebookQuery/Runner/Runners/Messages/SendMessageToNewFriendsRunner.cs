using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Messages
{
    public class SendMessageToNewFriendsRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
            new FacebookMessagesService().SendMessageToNewFriends(account);
        }
    }
}
