using System;

namespace DataBase.Models
{
    public class CommunityStatisticsDbModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long GroupId { get; set; }

        public long CountOfGroupInvitations { get; set; }

        public long CountOfPageInvitations { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}
