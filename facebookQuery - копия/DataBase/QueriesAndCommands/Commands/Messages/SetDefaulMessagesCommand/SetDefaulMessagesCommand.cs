namespace DataBase.QueriesAndCommands.Commands.Messages.SetDefaulMessagesCommand
{
    public class SetDefaulMessagesCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public long? GroupSettingsId { get; set; }
    }
}
