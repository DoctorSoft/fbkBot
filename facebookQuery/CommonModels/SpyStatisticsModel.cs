using System;

namespace CommonModels
{
    public class SpyStatisticsModel
    {
        public long Id { get; set; }

        public long SpyAccountId { get; set; }

        public long CountAnalizeFriends { get; set; }

        public DateTime DateTimeUpdateStatistics { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
