using CommonModels;

namespace Services.ViewModels.HomeModels
{
    public class AccountSettingsViewModel
    {
        public AccountViewModel Account { get; set; }

        public AccountSettingsModel Settings{get; set;}

        public DetailedStatisticsModel Statistics { get; set; }
    }
}