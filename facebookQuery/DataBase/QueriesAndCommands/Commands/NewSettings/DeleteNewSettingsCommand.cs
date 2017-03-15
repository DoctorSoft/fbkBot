namespace DataBase.QueriesAndCommands.Commands.NewSettings
{
    public class DeleteNewSettingsCommand : IVoidCommand
    {
        public long GroupId { get; set; }

        public long AccountId { get; set; }
    }
}
