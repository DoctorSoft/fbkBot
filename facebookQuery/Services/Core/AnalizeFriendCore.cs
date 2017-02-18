using System.Threading;
using CommonModels;
using Constants.FriendTypesEnum;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendStatusCommand;
using DataBase.QueriesAndCommands.Commands.Friends.RemoveAnalyzedFriendCommand;
using DataBase.QueriesAndCommands.Commands.SpyStatistics;
using Services.Core.Interfaces;
using Services.Core.Models;
using Services.ViewModels.FriendsModels;

namespace Services.Core
{
    public class AnalizeFriendCore : IAnalyzeFriendCore
    {
        public void StartAnalyze(AnalyzeModel model)
        {
            if (model.GenderIsSuccess && model.InfoIsSuccess && model.Settings.Gender != null && (model.Settings.Countries != null || model.Settings.Cities != null))
            {
                new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(
                    new ChangeAnalysisFriendStatusCommand
                    {
                        AccountId = model.AnalysisFriend.AccountId,
                        FriendFacebookId = model.AnalysisFriend.FacebookId,
                        NewStatus = StatusesFriend.ToAdd

                    });
            }
            if (model.GenderIsSuccess && model.Settings.Gender != null && model.Settings.Countries == null && model.Settings.Cities == null)
            {
                new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(
                    new ChangeAnalysisFriendStatusCommand
                    {
                        AccountId = model.AnalysisFriend.AccountId,
                        FriendFacebookId = model.AnalysisFriend.FacebookId,
                        NewStatus = StatusesFriend.ToAdd
                    });
            }

            if (model.InfoIsSuccess && model.Settings.Gender == null && (model.Settings.Countries != null || model.Settings.Cities != null))
            {
                new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(
                    new ChangeAnalysisFriendStatusCommand
                    {
                        AccountId = model.AnalysisFriend.AccountId,
                        FriendFacebookId = model.AnalysisFriend.FacebookId,
                        NewStatus = StatusesFriend.ToAdd

                    });
            }
            else
            {
                new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(
                    new ChangeAnalysisFriendStatusCommand
                    {
                        AccountId = model.AnalysisFriend.AccountId,
                        FriendFacebookId = model.AnalysisFriend.FacebookId,
                        NewStatus = StatusesFriend.ToDelete
                    });

                new RemoveAnalyzedFriendCommandHandler(new DataBaseContext()).Handle(
                    new RemoveAnalyzedFriendCommand
                    {
                        AccountId = model.AnalysisFriend.AccountId,
                        FriendId = model.AnalysisFriend.Id,
                    });
            }


            new AddOrUpdateSpyStatisticsCommandHandler(new DataBaseContext()).Handle(
            new AddOrUpdateSpyStatisticsCommand
            {
                CountAnalizeFriends = 1,
                SpyAccountId = model.SpyAccountId
            });

            Thread.Sleep(5000);
        }
    }
}
