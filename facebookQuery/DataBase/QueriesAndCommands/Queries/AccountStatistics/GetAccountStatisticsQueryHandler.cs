using System;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.AccountStatistics
{
    public class GetAccountStatisticsQueryHandler : IQueryHandler<GetAccountStatisticsQuery, AccountStatisticsData>
    {
        private readonly DataBaseContext context;

        public GetAccountStatisticsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public AccountStatisticsData Handle(GetAccountStatisticsQuery query)
        {
            try
            {
                var settings =
                    context.AccountStatistics.Where(model => model.Id == query.AccountId)
                        .Select(model => new AccountStatisticsData
                        {
                            AccountId = model.Id,
                            CreateDateTime = model.CreateDateTime,
                            Id = model.Id,
                            CountReceivedFriends = model.CountReceivedFriends,
                            CountRequestsSentToFriends = model.CountRequestsSentToFriends,
                            DateTimeUpdateStatistics = model.DateTimeUpdateStatistics
                        }).FirstOrDefault();

                return settings;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
