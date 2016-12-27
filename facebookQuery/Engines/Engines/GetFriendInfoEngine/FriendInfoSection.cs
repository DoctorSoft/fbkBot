using Constants.GendersUnums;

namespace Engines.Engines.GetFriendInfoEngine
{
    public class FriendInfoSection
    {
        public string WorkSection { get; set; }

        public string SchoolSection { get; set; }

        public string LivesSection { get; set; }

        public string RelationsSection { get; set; }

        public GenderEnum? Gender { get; set; }
    }
}
