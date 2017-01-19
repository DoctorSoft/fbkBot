using CommonModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAccountSettingsManager
    {
        SettingsModel GetSettings(long groupSettingsId);

        string GetCronByMinutes(long min);

        void UpdateSettings(SettingsModel newSettings);
    }
}
