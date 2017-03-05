using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.AccountStatistics
{
    public class GetAccountStatisticsQuery : IQuery<List<AccountStatisticsData>>
    {
        public long AccountId { get; set; }
    }
}
