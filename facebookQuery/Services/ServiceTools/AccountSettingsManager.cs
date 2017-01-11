using CommonModels;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.AccountSettings;
using DataBase.QueriesAndCommands.Queries.AccountSettings;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class AccountSettingsManager : IAccountSettingsManager
    {
        public AccountSettingsModel GetAccountSettings(long accountId)
        {
            var settings = new GetAccountSettingsQueryHandler(new DataBaseContext()).Handle(new GetAccountSettingsQuery
            {
                AccountId = accountId
            });

            if (settings == null)
            {
                return null;
            }

            return new AccountSettingsModel
            {
                AccountId = settings.AccountId,
                Gender = settings.Gender,
                LivesPlace = settings.LivesPlace,
                SchoolPlace = settings.SchoolPlace,
                WorkPlace = settings.WorkPlace
            };
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
