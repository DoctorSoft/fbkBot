namespace DataBase.QueriesAndCommands.Commands.Messages
{
    public class RemoveMessageCommand : IVoidCommand
    {
        public long MessageId { get; set; }
    }
}
