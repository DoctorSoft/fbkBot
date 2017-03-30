using Jobs.Notices;
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Messages
{
    public class SendMessageToUnansweredRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
            new FacebookMessagesService(new NoticesProxy()).SendMessageToUnanswered(account);
        }
    }
}
