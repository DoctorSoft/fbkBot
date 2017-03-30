namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddToRemovedFriendCommand
{
    public class MarkAddToRemovedFriendCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
