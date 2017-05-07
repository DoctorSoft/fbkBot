namespace DataBase.QueriesAndCommands.Commands.CounterCheckFriends.MarkCounterCheckFriendsCommand
{
    public class MarkCounterCheckFriendsCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public int NewRetryCount { get; set; }

        public bool ResetCounter { get; set; } 
    }
}
