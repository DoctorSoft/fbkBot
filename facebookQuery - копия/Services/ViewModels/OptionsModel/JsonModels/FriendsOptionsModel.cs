namespace Services.ViewModels.OptionsModel.JsonModels
{
    public class FriendsOptionsModel
    {
        public long RetryTimeConfirmFriendships { get; set; }

        public long RetryTimeGetNewAndRecommendedFriends { get; set; }

        public long RetryTimeRefreshFriends { get; set; }

        public long RetryTimeSendRequestFriendships { get; set; }
    }
}
