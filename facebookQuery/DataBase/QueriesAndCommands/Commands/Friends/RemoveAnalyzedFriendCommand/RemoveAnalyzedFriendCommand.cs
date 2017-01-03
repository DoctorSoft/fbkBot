namespace DataBase.QueriesAndCommands.Commands.Friends.RemoveAnalyzedFriendCommand
{
    public class RemoveAnalyzedFriendCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
