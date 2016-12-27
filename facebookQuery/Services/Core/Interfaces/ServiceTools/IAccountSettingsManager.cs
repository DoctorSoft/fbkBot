using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAccountSettingsManager
    {
        AccountSettingsModel GetAccountSettings(long accountId);

        void UpdateAccountSettings(AccountSettingsModel newSettings);
    }
}
