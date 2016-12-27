using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.AccountSettings;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.AccountSettings;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class AccountSettingsManager : IAccountSettingsManager
    {
        public AccountSettingsModel GetAccountSettings(long accountId)
        {
            return new GetAccountSettingsQueryHandler(new DataBaseContext()).Handle(new GetAccountSettingsQuery
            {
                AccountId = accountId
            });
        }

        public void UpdateAccountSettings(AccountSettingsModel newSettings)
        {
            new AddOrUpdateAccountSettingsCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateAccountSettingsCommand
                {
                    AccountId = newSettings.AccountId,
                    Gender = newSettings.Gender,
                    LivesPlace = newSettings.LivesPlace,
                    SchoolPlace = newSettings.SchoolPlace,
                    WorkPlace = newSettings.WorkPlace
                });
        }
    }
}
