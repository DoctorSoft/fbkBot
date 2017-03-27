namespace DataBase.QueriesAndCommands.Queries.Friends.CheckAllConditions
{
    public class CheckAllConditionsQuery : IQuery<bool>
    {
        public long AccountId { get; set; }

        public long FriendId { get; set; }
    }
}
