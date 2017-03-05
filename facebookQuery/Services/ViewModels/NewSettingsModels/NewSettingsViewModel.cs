namespace Services.ViewModels.NewSettingsModels
{
    public class NewSettingsViewModel
    {
        public long Id { get; set; }

        public long SettingsGroupId { get; set; }

        public long AccountId { get; set; }

        public CommunityOptionsViewModel CommunityOptions { get; set; }
    }
}
