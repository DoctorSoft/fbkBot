using System;

namespace DataBase.QueriesAndCommands.Queries.SpyStatistics
{
    public class SpyStatisticsData
    {
        public long Id { get; set; }

        public long SpyAccountId { get; set; }

        public long CountAnalizeFriends { get; set; }
        
        public DateTime DateTimeUpdateStatistics { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
