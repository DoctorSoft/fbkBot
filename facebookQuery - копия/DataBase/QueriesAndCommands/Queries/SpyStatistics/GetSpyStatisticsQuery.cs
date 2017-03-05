using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.SpyStatistics
{
    public class GetSpyStatisticsQuery : IQuery<List<SpyStatisticsData>>
    {
        public long SpyId { get; set; }
    }
}
