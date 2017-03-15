namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddToPageFriendCommand
{
    public class MarkAddToPageFriendCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
