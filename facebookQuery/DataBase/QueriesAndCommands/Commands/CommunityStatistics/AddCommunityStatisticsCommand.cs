using System;

namespace DataBase.QueriesAndCommands.Commands.CommunityStatistics
{
    public class AddCommunityStatisticsCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public long GroupId { get; set; }

        public long? CountOfGroupInvitations { get; set; }

        public long? CountOfPageInvitations { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}
