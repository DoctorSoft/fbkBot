namespace DataBase.QueriesAndCommands.Commands.Friends.MarkWinksFriendsFriendsCommand
{
    public class MarkWinksFriendsFriendsCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
