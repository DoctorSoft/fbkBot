using Services.ViewModels.GroupModels;

namespace Services.Interfaces.ServiceTools
{
    public interface IAccountSettingsManager
    {
        GroupSettingsViewModel GetSettings(long groupSettingsId);

        string GetCronByMinutes(long min);

        void UpdateSettings(GroupSettingsViewModel newSettings);
    }
}
