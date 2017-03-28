namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddedToRemoveTimeCommand
{
    public class MarkAddedToRemoveTimeCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
