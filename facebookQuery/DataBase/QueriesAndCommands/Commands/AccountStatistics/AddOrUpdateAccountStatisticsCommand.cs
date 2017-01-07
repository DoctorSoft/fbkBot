using System;

namespace DataBase.QueriesAndCommands.Commands.AccountStatistics
{
    public class AddOrUpdateAccountStatisticsCommand : ICommand<long>
    {
        public long AccountId { get; set; }

        public long CountReceivedFriends { get; set; }

        public long CountRequestsSentToFriends { get; set; }

        public DateTime DateTimeUpdateStatistics { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
