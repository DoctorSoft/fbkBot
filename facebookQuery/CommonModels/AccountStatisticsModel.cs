using System;

namespace CommonModels
{
    public class AccountStatisticsModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long CountReceivedFriends { get; set; }

        public long CountRequestsSentToFriends { get; set; }

        public long CountOrdersConfirmedFriends { get; set; }

        public long CountOfWinksBack { get; set; }

        public DateTime DateTimeUpdateStatistics { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
