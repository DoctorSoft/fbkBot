using System;

namespace DataBase.QueriesAndCommands.Queries.CommunityStatistics
{
    public class CommunityStatisticsData
    {
        public long Id { get; set; }
        public long AccountId { get; set; }

        public long CountOfGroupInvitations { get; set; }

        public long CountOfPageInvitations { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}
