namespace DataBase.QueriesAndCommands.Queries.CounterCheckFriends.GetCounterCheckFriendsByAccountId
{
    public class GetCounterCheckFriendsByAccountIdCommand : ICommand<GetCounterCheckFriendsByAccountIdModel>
    {
        public long AccountId { get; set; }
    }
}
