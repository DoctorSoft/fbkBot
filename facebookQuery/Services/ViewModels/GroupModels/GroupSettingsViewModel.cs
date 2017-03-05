using Constants.GendersUnums;

namespace Services.ViewModels.GroupModels
{
    public class GroupSettingsViewModel
    {
        public long GroupId { get; set; }

        //Limits options
        public long CountMinFriends { get; set; }

        public long CountMaxFriends { get; set; }
       
        //Community options

        public string FacebookGroups { get; set; }

        public string FacebookPages { get; set; }

        //Geo options

        public string Cities { get; set; }

        public string Countries { get; set; }

        public GenderEnum? Gender { get; set; }
        
        // Message options

        public int RetryTimeSendUnreadHour { get; set; }

        public int RetryTimeSendUnreadMin { get; set; }

        public int RetryTimeSendUnreadSec { get; set; }


        public int RetryTimeSendUnansweredHour { get; set; }

        public int RetryTimeSendUnansweredMin { get; set; }

        public int RetryTimeSendUnansweredSec { get; set; }


        public int RetryTimeSendNewFriendHour { get; set; }

        public int RetryTimeSendNewFriendMin { get; set; }

        public int RetryTimeSendNewFriendSec { get; set; }


        public int UnansweredDelay { get; set; }
        
        // Friends options

        public int RetryTimeConfirmFriendshipsHour { get; set; }

        public int RetryTimeConfirmFriendshipsMin { get; set; }

        public int RetryTimeConfirmFriendshipsSec { get; set; }


        public int RetryTimeGetNewAndRecommendedFriendsHour { get; set; }

        public int RetryTimeGetNewAndRecommendedFriendsMin { get; set; }

        public int RetryTimeGetNewAndRecommendedFriendsSec { get; set; }


        public int RetryTimeRefreshFriendsHour{ get; set; }

        public int RetryTimeRefreshFriendsMin { get; set; }

        public int RetryTimeRefreshFriendsSec { get; set; }


        public int RetryTimeSendRequestFriendshipsHour { get; set; }

        public int RetryTimeSendRequestFriendshipsMin { get; set; }

        public int RetryTimeSendRequestFriendshipsSec { get; set; }

    }
}
