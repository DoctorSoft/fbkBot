using System;
using Constants.FunctionEnums;
using Runner.Context;
using Runner.Interfaces;
using Runner.Runners.Community;
using Runner.Runners.Cookies;
using Runner.Runners.Friends;
using Runner.Runners.Messages;
using Runner.Runners.Spy;
using Runner.Runners.Winks;

namespace Runner
{
    public class ServiceRunner
    {
        public void RunService(FunctionName functionName, IRunnerModel model)
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
                case FunctionName.JoinTheNewGroupsAndPages:
                {
                    context = new RunnerContext(new JoinTheNewGroupsAndPagesRunner());
                    break;
                }
                case FunctionName.InviteToGroups:
                {
                    context = new RunnerContext(new InvaitTheNewGroupRunner());
                    break;
                }
                case FunctionName.InviteToPages:
                {
                    context = new RunnerContext(new InvaitTheNewPageRunner());
                    break;
                }
                case FunctionName.RemoveFromFriends:
                {
                    context = new RunnerContext(new InvaitTheNewPageRunner());
                    break;
                }
                case FunctionName.Wink:
                {
                    context = new RunnerContext(new WinkFriendsRunner());
                    break;
                }
                case FunctionName.AnalyzeFriends:
                {
                    context = new RunnerContext(new AnalyzeFriendsRunner());
                    break;
                }
                case FunctionName.WinkFriendFriends:
                {
                    context = new RunnerContext(new WinkFriendsFriendsRunner());
                    break;
                }
                case FunctionName.CheckFriendsAtTheEndTimeConditions:
                {
                    context = new RunnerContext(new CheckFriendsAtTheEndTimeConditionsRunner());
                    break;
                }
                case FunctionName.WinkBack:
                {
                    context = new RunnerContext(new WinkBackRunner());
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException("functionName");
                }
            }

            context.Execute(model);
        }
    }
}
