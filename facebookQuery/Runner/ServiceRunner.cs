using System;
using Constants.FunctionEnums;
using Runner.Context;
using Runner.Runners.Cookies;
using Runner.Runners.Friends;
using Runner.Runners.Messages;
using Runner.Runners.Settings;
using Services.ViewModels.HomeModels;

namespace Runner
{
    public class ServiceRunner
    {
        public void RunService(FunctionName functionName, AccountViewModel account)
        {
            RunnerContext context;

            switch (functionName)
            {
                case FunctionName.SendMessageToNewFriends:
                {
                    context = new RunnerContext(new SendMessageToNewFriendsRunner());
                    break;
                }
                case FunctionName.SendMessageToUnanswered:
                {
                    context = new RunnerContext(new SendMessageToUnansweredRunner());
                    break;
                }
                case FunctionName.SendMessageToUnread:
                {
                    context = new RunnerContext(new SendMessageToUnreadRunner());
                    break;
                }
                case FunctionName.RefreshFriends:
                {
                    context = new RunnerContext(new RefreshFriendsRunner());
                    break;
                }
                case FunctionName.GetNewFriendsAndRecommended:
                {
                    context = new RunnerContext(new GetNewFriendsAndRecommendedRunner());
                    break;
                }
                case FunctionName.ConfirmFriendship:
                {
                    context = new RunnerContext(new ConfirmFriendshipRunner());
                    break;
                }
                case FunctionName.SendRequestFriendship:
                {
                    context = new RunnerContext(new SendRequestFriendshipRunner());
                    break;
                }
                case FunctionName.RefreshCookies:
                {
                    context = new RunnerContext(new RefreshCookiesRunner());
                    break;
                }
                case FunctionName.JoinTheNewGroup:
                {
                    context = new RunnerContext(new JoinTheNewGroupRunner());
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException("functionName");
                }
            }

            context.Execute(account);
        }
    }
}
