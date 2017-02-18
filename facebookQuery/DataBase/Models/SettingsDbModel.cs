﻿using Constants.GendersUnums;

namespace DataBase.Models
{
    public class SettingsDbModel
    {
        public long Id { get; set; }

        public GroupSettingsDbModel SettingsGroup { get; set; }

        //GEO options
        public string Cities { get; set; }

        public string Countries { get; set; }
        
        public GenderEnum? Gender { get; set; }
        
        // jobs
        // Message options

        public long RetryTimeSendUnread { get; set; }

        public long RetryTimeSendUnanswered { get; set; }

        public long RetryTimeSendNewFriend { get; set; }

        // Friends options

        public long RetryTimeConfirmFriendships { get; set; }

        public long RetryTimeGetNewAndRecommendedFriends { get; set; }

        public long RetryTimeRefreshFriends { get; set; }

        public long RetryTimeSendRequestFriendships { get; set; }

        // Message options

        public long UnansweredDelay { get; set; }

    }
}
