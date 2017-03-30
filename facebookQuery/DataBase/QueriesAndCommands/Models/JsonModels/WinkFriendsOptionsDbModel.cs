using CommonModels;

namespace DataBase.QueriesAndCommands.Models.JsonModels
{
    public class WinkFriendsOptionsDbModel
    {
        public TimeModel RetryTimeForWinkFriends { get; set; }

        public bool ConsiderGeoForWinkFriends { get; set; }
    }
}
