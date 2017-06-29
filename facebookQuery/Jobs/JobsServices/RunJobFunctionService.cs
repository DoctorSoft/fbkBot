using System;
using Constants.FunctionEnums;
using Jobs.Contexts;
using Jobs.Jobs.CommunityJobs;
using Jobs.Jobs.DeleteFriendsJobs;
using Jobs.Jobs.FriendJobs;
using Jobs.Jobs.MessageJobs;
using Jobs.Jobs.SpyJobs;
using Jobs.Jobs.WinksJobs;
using Jobs.Models;

namespace Jobs.JobsServices
{
    public class RunJobFunctionService
    {
        public string RunService(FunctionName functionName, RunJobModel model)
        {
            RunJobContext context;

            switch (functionName)
            {
                case FunctionName.SendMessageToNewFriends:
                {
                    context = new RunJobContext(new SendMessageToNewFriendsJob());
                    break;
                }
                case FunctionName.SendMessageToUnanswered:
                {
                    context = new RunJobContext(new SendMessageToUnansweredJob());
                    break;
                }
                case FunctionName.SendMessageToUnread:
                {
                    context = new RunJobContext(new SendMessageToUnreadJob());
                    break;
                }
                case FunctionName.RefreshFriends:
                {
                    context = new RunJobContext(new RefreshFriendsJob());
                    break;
                }
                case FunctionName.GetNewFriendsAndRecommended:
                {
                    context = new RunJobContext(new GetNewFriendsAndRecommendedJob());
                    break;
                }
                case FunctionName.ConfirmFriendship:
                {
                    context = new RunJobContext(new ConfirmFriendshipJob());
                    break;
                }
                case FunctionName.SendRequestFriendship:
                {
                    context = new RunJobContext(new SendRequestFriendshipJob());
                    break;
                }/*
                case FunctionName.RefreshCookies:
                {
                    context = new RunJobContext(new RefreshCookies());
                    break;
                }*/
                case FunctionName.JoinTheNewGroupsAndPages:
                {
                    context = new RunJobContext(new JoinTheNewGroupsAndPagesJob());
                    break;
                }
                case FunctionName.InviteToGroups:
                {
                    context = new RunJobContext(new InviteTheNewGroupJob());
                    break;
                }
                case FunctionName.InviteToPages:
                {
                    context = new RunJobContext(new InviteTheNewPageJob());
                    break;
                }
                case FunctionName.RemoveFromFriends:
                {
                    context = new RunJobContext(new RemoveFromFriendsJob());
                    break;
                }
                case FunctionName.Wink:
                {
                    context = new RunJobContext(new WinkFriendsJob());
                    break;
                }/*
                case FunctionName.AnalyzeFriends:
                {
                    context = new RunJobContext(new AnalyzeFriendsJob());
                    break;
                }*/
                case FunctionName.WinkFriendFriends:
                {
                    context = new RunJobContext(new WinkFriendsFriendsJob());
                    break;
                }/*
                case FunctionName.CheckFriendsAtTheEndTimeConditions:
                {
                    context = new RunJobContext(new C());
                    break;
                }*/
                case FunctionName.WinkBack:
                {
                    context = new RunJobContext(new WinkFriendsJob());
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException("functionName");
                }
            }

            context.Execute(model);

            return functionName.ToString();
        }
    }
}
