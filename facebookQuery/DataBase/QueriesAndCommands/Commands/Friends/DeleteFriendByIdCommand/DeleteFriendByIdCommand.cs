namespace DataBase.QueriesAndCommands.Commands.Friends.DeleteFriendByIdCommand
{
    public class DeleteFriendByIdCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }
    }
}
