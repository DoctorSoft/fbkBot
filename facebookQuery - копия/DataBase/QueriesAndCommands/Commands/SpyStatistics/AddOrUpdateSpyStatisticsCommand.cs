using System;

namespace DataBase.QueriesAndCommands.Commands.SpyStatistics
{
    public class AddOrUpdateSpyStatisticsCommand : ICommand<long>
    {
        public long SpyAccountId { get; set; }

        public long CountAnalizeFriends { get; set; }

        public DateTime DateTimeUpdateStatistics { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
