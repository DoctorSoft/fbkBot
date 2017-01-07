using Constants.GendersUnums;

namespace CommonModels
{
    public class AccountSettingsModel
    {
        public long AccountId { get; set; }

        public string LivesPlace { get; set; }

        public string SchoolPlace { get; set; }

        public string WorkPlace { get; set; }

        public GenderEnum? Gender { get; set; }
    }
}
