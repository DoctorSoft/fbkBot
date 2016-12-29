using Constants.GendersUnums;

namespace Services.ViewModels.FriendsModels
{
    public class FriendInfoViewModel
    {
        public long FacebookId { get; set; }

        public string WorkSection { get; set; }

        public string SchoolSection { get; set; }

        public string LivesSection { get; set; }

        public string RelationsSection { get; set; }

        public GenderEnum? Gender { get; set; }
    }
}
