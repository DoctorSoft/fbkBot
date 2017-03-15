namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddToGroupFriendCommand
{
    public class MarkAddToGroupFriendCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
