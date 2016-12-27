using Constants.GendersUnums;

namespace DataBase.QueriesAndCommands.Commands.AccountSettings
{
    public class AddOrUpdateAccountSettingsCommand : ICommand<long>
    {
        public long AccountId { get; set; }

        public string LivesPlace { get; set; }

        public string SchoolPlace { get; set; }

        public string WorkPlace { get; set; }

        public GenderEnum Gender { get; set; }
    }
}
