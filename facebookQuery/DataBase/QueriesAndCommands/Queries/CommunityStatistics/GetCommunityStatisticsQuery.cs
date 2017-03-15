using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.CommunityStatistics
{
    public class GetCommunityStatisticsQuery : IQuery<List<CommunityStatisticsData>>
    {
        public long GroupId { get; set; }
    }
}
