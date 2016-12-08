namespace DataBase.QueriesAndCommands.Commands.ExtraMessages
{
    public class UpdateExtraMessageCommand : IVoidCommand
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
