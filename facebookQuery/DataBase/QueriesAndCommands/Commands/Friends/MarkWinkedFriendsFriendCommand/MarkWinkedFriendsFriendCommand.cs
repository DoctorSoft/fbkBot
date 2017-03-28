namespace DataBase.QueriesAndCommands.Commands.Friends.MarkWinkedFriendsFriendCommand
{
    public class MarkWinkedFriendsFriendCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
