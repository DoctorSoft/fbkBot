using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.AccountStatistics
{
    public class GetAccountStatisticsQueryHandler : IQueryHandler<GetAccountStatisticsQuery, List<AccountStatisticsData>>
    {
        private readonly DataBaseContext context;

        public GetAccountStatisticsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<AccountStatisticsData> Handle(GetAccountStatisticsQuery query)
        {
            try
            {
                var statistics =
                    context.AccountStatistics.Where(model => model.AccountId == query.AccountId)
                        .Select(model => new AccountStatisticsData
                        {
                            AccountId = model.Id,
                            CreateDateTime = model.CreateDateTime,
                            Id = model.Id,
                            CountReceivedFriends = model.CountReceivedFriends,
                            CountRequestsSentToFriends = model.CountRequestsSentToFriends,
                            DateTimeUpdateStatistics = model.DateTimeUpdateStatistics
                        }).ToList();

                return statistics;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
