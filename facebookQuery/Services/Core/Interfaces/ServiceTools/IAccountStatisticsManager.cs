using CommonModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAccountStatisticsManager
    {
        AccountStatisticsList GetAccountStatistics(long accountId);

        AccountStatisticsModel GetLastHourAccountStatistics(AccountStatisticsList allStatistics);

        AccountStatisticsModel GetAllTimeAccountStatistics(AccountStatisticsList allStatistics);

        void UpdateAccountStatistics(AccountStatisticsModel newStatistics);
    }
}
