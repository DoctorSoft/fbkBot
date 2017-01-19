using Constants.GendersUnums;

namespace DataBase.Models
{
    public class SettingsDbModel
    {
        public long Id { get; set; }

        public GroupSettingsDbModel SettingsGroup { get; set; }

        //GEO options
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
