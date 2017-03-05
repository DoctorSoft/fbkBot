using Runner.Interfaces;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Messages
{
    public class SendMessageToNewFriendsRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new FacebookMessagesService().SendMessageToNewFriends(account);
        }
    }
}
