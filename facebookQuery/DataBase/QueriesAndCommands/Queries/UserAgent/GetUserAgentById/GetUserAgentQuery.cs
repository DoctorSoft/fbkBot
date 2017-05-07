namespace DataBase.QueriesAndCommands.Queries.UserAgent.GetUserAgentById
{
    public class GetUserAgentQuery : IQuery<UserAgentData>
    {
        public long? UserAgentId { get; set; }
    }
}
