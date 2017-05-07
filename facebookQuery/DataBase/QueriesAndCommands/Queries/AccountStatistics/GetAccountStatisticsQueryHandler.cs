using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.AccountStatistics
{
    public class GetAccountStatisticsQueryHandler : IQueryHandler<GetAccountStatisticsQuery, List<AccountStatisticsData>>
    {
        private readonly DataBaseContext _context;

        public GetAccountStatisticsQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<AccountStatisticsData> Handle(GetAccountStatisticsQuery query)
        {
            try
            {
                var statistics =
                    _context.AccountStatistics.Where(model => model.AccountId == query.AccountId)
                        .Select(model => new AccountStatisticsData
                        {
                            AccountId = model.Id,
                            CreateDateTime = model.CreateDateTime,
                            Id = model.Id,
                            CountReceivedFriends = model.CountReceivedFriends,
                            CountRequestsSentToFriends = model.CountRequestsSentToFriends,
                            CountOrdersConfirmedFriends = model.CountOrdersConfirmedFriends,
                            DateTimeUpdateStatistics = model.DateTimeUpdateStatistics,
                            CountOfWinksBack = model.CountOfWinksBack
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
