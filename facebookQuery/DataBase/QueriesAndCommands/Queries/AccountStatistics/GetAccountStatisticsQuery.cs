namespace DataBase.QueriesAndCommands.Queries.AccountStatistics
{
    public class GetAccountStatisticsQuery : IQuery<AccountStatisticsData>
    {
        public long AccountId { get; set; }
    }
}
