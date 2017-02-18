using Services.ViewModels.GroupModels;

namespace Services.ViewModels.HomeModels
{
    public class AccountSettingsViewModel
    {
        public AccountViewModel Account { get; set; }

        public GroupSettingsViewModel Settings { get; set; }

        public DetailedStatisticsModel Statistics { get; set; }
    }
}