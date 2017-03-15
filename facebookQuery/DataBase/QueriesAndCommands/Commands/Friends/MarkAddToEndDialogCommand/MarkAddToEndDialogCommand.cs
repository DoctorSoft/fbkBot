namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddToEndDialogCommand
{
    public class MarkAddToEndDialogCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
