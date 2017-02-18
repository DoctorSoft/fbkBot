namespace DataBase.QueriesAndCommands.Queries.FriendsBlackList.CheckForFriendBlacklisted
{
    public class CheckForFriendBlacklistedQuery : IQuery<bool>
    {
        public long FriendFacebookId { get; set; }

        public long GroupSettingsId { get; set; }
    }
}
