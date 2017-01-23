namespace DataBase.QueriesAndCommands.Commands.Friends.MarkRemovedFriendCommand
{
    public class MarkRemovedFriendCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
