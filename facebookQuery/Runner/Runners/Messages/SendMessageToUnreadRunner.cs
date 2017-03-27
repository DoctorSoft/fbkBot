using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Messages
{
    public class SendMessageToUnreadRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
            new FacebookMessagesService().SendMessageToUnread(account);
        }
    }
}
