using CommonModels;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.AccountStatistics;
using DataBase.QueriesAndCommands.Queries.AccountStatistics;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class AccountStatisticsManager : IAccountStatisticsManager
    {
        public AccountStatisticsModel GetAccountStatistics(long accountId)
        {
            var statisticData = new GetAccountStatisticsQueryHandler(new DataBaseContext()).Handle(new GetAccountStatisticsQuery
            {
                AccountId = accountId
            });

            return new AccountStatisticsModel
            {
                AccountId = statisticData.AccountId,
                CreateDateTime = statisticData.CreateDateTime,
                CountReceivedFriends = statisticData.CountReceivedFriends,
                Id = statisticData.Id,
                CountRequestsSentToFriends = statisticData.CountRequestsSentToFriends,
                DateTimeUpdateStatistics = statisticData.DateTimeUpdateStatistics
            };
        }

        public void UpdateAccountStatistics(AccountStatisticsModel newStatistics)
        {
            new AddOrUpdateAccountStatisticsCommandHandler(new DataBaseContext()).Handle(
                new AddOrUpdateAccountStatisticsCommand
                {
                    AccountId = newStatistics.AccountId,
                    CountReceivedFriends = newStatistics.CountReceivedFriends,
                    CountRequestsSentToFriends = newStatistics.CountRequestsSentToFriends,
                    CreateDateTime = newStatistics.CreateDateTime,
                    DateTimeUpdateStatistics = newStatistics.DateTimeUpdateStatistics
                });
        }
    }
}
