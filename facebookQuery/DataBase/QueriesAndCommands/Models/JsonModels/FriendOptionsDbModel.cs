using CommonModels;

namespace DataBase.QueriesAndCommands.Models.JsonModels
{
    public class FriendOptionsDbModel
    {
        public TimeModel RetryTimeConfirmFriendships { get; set; }

        public TimeModel RetryTimeGetNewAndRecommendedFriends { get; set; }

        public TimeModel RetryTimeRefreshFriends { get; set; }

        public TimeModel RetryTimeSendRequestFriendships { get; set; }
    }
}
