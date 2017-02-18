using Services.ViewModels.GroupModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAccountSettingsManager
    {
        GroupSettingsViewModel GetSettings(long groupSettingsId);

        string GetCronByMinutes(long min);

        void UpdateSettings(GroupSettingsViewModel newSettings);
    }
}
