using CommonModels;

namespace DataBase.QueriesAndCommands.Models.JsonModels
{
    public class WinkFriendsOptionsDbModel
    {
        public TimeModel RetryTimeForWinkFriends { get; set; }

        public bool ConsiderGeoForWinkFriends { get; set; }

        public TimeModel RetryTimeForWinkFriendsFriends { get; set; }

        public bool ConsiderGeoForWinkFriendsFriends { get; set; }

        public TimeModel RetryTimeForWinkBack { get; set; }
    }
}
