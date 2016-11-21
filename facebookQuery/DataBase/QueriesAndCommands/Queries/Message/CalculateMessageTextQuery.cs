namespace DataBase.QueriesAndCommands.Queries.Message
{
    public class CalculateMessageTextQuery : IQuery<string>
    {
        public string TextPattern { get; set; }

        public long? AccountId { get; set; }

        public long? FriendId { get; set; }
    }
}
