using CommonModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IStatisticsManager
    {
        AccountStatisticsList GetAccountStatistics(long accountId);

        AccountStatisticsModel GetLastHourAccountStatistics(AccountStatisticsList allStatistics);

        AccountStatisticsModel GetAllTimeAccountStatistics(AccountStatisticsList allStatistics);

        SpyStatisticsList GetSpyStatistics(long spyAccountId);

        SpyStatisticsModel GetLastHourSpyStatistics(SpyStatisticsList allStatistics);

        SpyStatisticsModel GetAllTimeSpyStatistics(SpyStatisticsList allStatistics);

        void UpdateAccountStatistics(AccountStatisticsModel newStatistics);
    }
}
