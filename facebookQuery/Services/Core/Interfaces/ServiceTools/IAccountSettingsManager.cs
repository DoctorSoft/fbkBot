using CommonModels;
using Services.ViewModels.HomeModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAccountSettingsManager
    {
        AccountSettingsModel GetAccountSettings(long accountId);

        void UpdateAccountSettings(AccountSettingsModel newSettings);
    }
}
