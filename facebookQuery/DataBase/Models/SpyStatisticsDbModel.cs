using System;

namespace DataBase.Models
{
    public class SpyStatisticsDbModel
    {
        public long Id { get; set; }

        public long SpyId { get; set; }

        public long CountAnalizeFriends { get; set; }

        public DateTime DateTimeUpdateStatistics { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
