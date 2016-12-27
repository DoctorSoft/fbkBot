using Constants.GendersUnums;

namespace DataBase.Models
{
    public class AccountSettingsDbModel
    {
        public long Id { get; set; }

        //GEO options
        public string LivesPlace { get; set; }

        public string SchoolPlace { get; set; }

        public string WorkPlace { get; set; }

        public GenderEnum Gender { get; set; }

        public AccountDbModel Account { get; set; }
    }
}
