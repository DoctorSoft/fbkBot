using DataBase.QueriesAndCommands.Models.JsonModels;

namespace DataBase.QueriesAndCommands.Commands.NewSettings
{
    public class SaveNewSettingsCommand : IVoidCommand
    {
        public long GroupId { get; set; }

        public long AccountId { get; set; }

        public CommunityOptionsDbModel CommunityOptions { get; set; } 
    }
}
