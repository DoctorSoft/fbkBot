using Constants.FriendTypesEnum;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendStatusCommand;
using DataBase.QueriesAndCommands.Commands.Friends.RemoveAnalyzedFriendCommand;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using Services.Core.Interfaces;
using Services.ViewModels.FriendsModels;

namespace Services.Core
{
    public class AnalizeFriendCore : IAnalyzeFriendCore
    {
        public void StartAnalyze(AccountSettingsModel settings, FriendInfoViewModel friendInfo)
        {
            if (friendInfo.LivesSection != null && friendInfo.LivesSection.Contains(settings.LivesPlace))
            {
                new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(
                    new ChangeAnalysisFriendStatusCommand
                    {
                        AccountId = settings.AccountId,
                        FriendFacebookId = friendInfo.FacebookId,
                        NewStatus = StatusesFriend.ToAdd
                    });
            }
            else if (friendInfo.SchoolSection != null && friendInfo.SchoolSection.Contains(settings.SchoolPlace))
            {
                new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(
                new ChangeAnalysisFriendStatusCommand
                {
                    AccountId = settings.AccountId,
                    FriendFacebookId = friendInfo.FacebookId,
                    NewStatus = StatusesFriend.ToAdd
                });
            }
            else if (friendInfo.WorkSection != null && friendInfo.WorkSection.Contains(settings.WorkPlace))
            {
                new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(
                new ChangeAnalysisFriendStatusCommand
                {
                    AccountId = settings.AccountId,
                    FriendFacebookId = friendInfo.FacebookId,
                    NewStatus = StatusesFriend.ToAdd
                });
            }
            else if (friendInfo.Gender!= null && friendInfo.Gender == settings.Gender)
            {
                new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(
                    new ChangeAnalysisFriendStatusCommand
                    {
                        AccountId = settings.AccountId,
                        FriendFacebookId = friendInfo.FacebookId,
                        NewStatus = StatusesFriend.ToAdd
                    });
            }
            //FINALLY
            else
            {
                new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(
                    new ChangeAnalysisFriendStatusCommand
                    {
                        AccountId = settings.AccountId,
                        FriendFacebookId = friendInfo.FacebookId,
                        NewStatus = StatusesFriend.ToDelete
                    });

                new RemoveAnalyzedFriendCommandHandler(new DataBaseContext()).Handle(new RemoveAnalyzedFriendCommand
                {

                    AccountId = settings.AccountId,
                    FriendId = friendInfo.Id,
                });
            }
        }
    }
}
