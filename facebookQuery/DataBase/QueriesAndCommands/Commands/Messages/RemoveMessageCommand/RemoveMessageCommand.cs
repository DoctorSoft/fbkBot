namespace DataBase.QueriesAndCommands.Commands.Messages.RemoveMessageCommand
{
    public class RemoveMessageCommand : IVoidCommand
    {
        public long MessageId { get; set; }
    }
}
