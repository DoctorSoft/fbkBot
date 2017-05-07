using Constants.GendersUnums;

namespace Services.ViewModels.GroupModels
{
    public class GroupSettingsViewModel
    {
        public long GroupId { get; set; }

        //Wink options

        public int RetryTimeForWinkFriendsHour { get; set; }

        public int RetryTimeForWinkFriendsMin { get; set; }

        public int RetryTimeForWinkFriendsSec { get; set; }

        public bool ConsiderGeoForWinkFriends { get; set; }

        public int RetryTimeForWinkFriendsFriendsHour { get; set; }

        public int RetryTimeForWinkFriendsFriendsMin { get; set; }

        public int RetryTimeForWinkFriendsFriendsSec { get; set; }

        public bool ConsiderGeoForWinkFriendsFriends { get; set; }

        public int RetryTimeForWinkBackHour { get; set; }

        public int RetryTimeForWinkBackMin { get; set; }

        public int RetryTimeForWinkBackSec { get; set; }

        //Limits options
        public long CountMinFriends { get; set; }

        public long CountMaxFriends { get; set; }

        //Delete friends options

        public bool EnableDialogIsOver { get; set; }

        public bool EnableIsAddedToGroupsAndPages { get; set; }

        public bool EnableIsWink { get; set; }

        public bool EnableIsWinkFriendsOfFriends { get; set; }

        public int DialogIsOverTimer { get; set; }

        public int IsAddedToGroupsAndPagesTimer { get; set; }

        public int IsWinkTimer { get; set; }

        public int IsWinkFriendsOfFriendsTimer { get; set; }

        public int DeletionFriendTimer { get; set; } 

       
        //Community options

        public bool IsJoinToAllGroups { get; set; }

        public int RetryTimeInviteTheGroupsHour { get; set; }

        public int RetryTimeInviteTheGroupsMin { get; set; }

        public int RetryTimeInviteTheGroupsSec { get; set; }

        public int RetryTimeInviteThePagesHour { get; set; }

        public int RetryTimeInviteThePagesMin { get; set; }

        public int RetryTimeInviteThePagesSec { get; set; }
        
        public string FacebookGroups { get; set; }

        public string FacebookPages { get; set; }

        public int MinFriendsJoinGroupInDay { get; set; }

        public int MaxFriendsJoinGroupInDay { get; set; }

        public int MinFriendsJoinGroupInHour { get; set; }

        public int MaxFriendsJoinGroupInHour { get; set; }

        public int MinFriendsJoinPageInDay { get; set; }

        public int MaxFriendsJoinPageInDay { get; set; }

        public int MinFriendsJoinPageInHour { get; set; }

        public int MaxFriendsJoinPageInHour { get; set; }

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

        public int AllowedRemovalPercentage { get; set; }
    }
}
