using Constants.GendersUnums;

namespace DataBase.QueriesAndCommands.Models.JsonModels
{
    public class GeoOptionsDbModel
    {
        public string Cities { get; set; }

        public string Countries { get; set; }

        public GenderEnum? Gender { get; set; }
    }
}
