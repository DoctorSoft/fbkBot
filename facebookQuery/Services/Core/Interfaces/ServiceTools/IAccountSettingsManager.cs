using CommonModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAccountSettingsManager
    {
        SettingsModel GetSettings(long groupSettingsId);

        void UpdateSettings(SettingsModel newSettings);
    }
}
