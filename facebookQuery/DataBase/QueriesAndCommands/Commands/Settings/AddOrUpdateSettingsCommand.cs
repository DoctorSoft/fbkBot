using Constants.GendersUnums;

namespace DataBase.QueriesAndCommands.Commands.Settings
{
    public class AddOrUpdateSettingsCommand : ICommand<long>
    {
        public long GroupId { get; set; }

        //Geo options

        public string LivesPlace { get; set; }

        public string SchoolPlace { get; set; }

        public string WorkPlace { get; set; }

        public GenderEnum? Gender { get; set; }

        // Message options

        public long DelayTimeSendUnread { get; set; }

        public long DelayTimeSendUnanswered { get; set; }

        public long DelayTimeSendNewFriend { get; set; }
    }
}
