namespace DataBase.QueriesAndCommands.Commands.Friends.MarkWinkedFriendCommand
{
    public class MarkWinkedFriendCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
