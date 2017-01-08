﻿using System;

namespace DataBase.Models
{
    public class AccountStatisticsDbModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long CountReceivedFriends { get; set; }

        public long CountRequestsSentToFriends { get; set; }

        public DateTime DateTimeUpdateStatistics { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}