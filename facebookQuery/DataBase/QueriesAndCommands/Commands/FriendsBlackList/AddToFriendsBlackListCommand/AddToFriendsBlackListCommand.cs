namespace DataBase.QueriesAndCommands.Commands.FriendsBlackList.AddToFriendsBlackListCommand
{
    public class AddToFriendsBlackListCommand : IVoidCommand
    {
        public long GroupSettingsId { get; set; }

        public long FriendFacebookId { get; set; }

        public string FriendName { get; set; }
    }
}

