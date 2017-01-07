using CommonModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAccountStatisticsManager
    {
        AccountStatisticsModel GetAccountStatistics(long accountId);

        void UpdateAccountStatistics(AccountStatisticsModel newStatistics);
    }
}
