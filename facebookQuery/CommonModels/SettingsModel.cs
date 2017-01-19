using Constants.GendersUnums;

namespace CommonModels
{
    public class SettingsModel
    {
        public long GroupId { get; set; }

        //Geo options

        public string LivesPlace { get; set; }

        public string SchoolPlace { get; set; }

        public string WorkPlace { get; set; }

        public GenderEnum? Gender { get; set; }

        // Message options (for jobs)

        public long DelayTimeSendUnread { get; set; }

        public long DelayTimeSendUnanswered { get; set; }

        public long DelayTimeSendNewFriend { get; set; }

        // Message options

        public long UnansweredDelay { get; set; }
    }
}
