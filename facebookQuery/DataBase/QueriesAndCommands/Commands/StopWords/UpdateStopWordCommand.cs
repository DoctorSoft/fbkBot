namespace DataBase.QueriesAndCommands.Commands.StopWords
{
    public class UpdateStopWordCommand : IVoidCommand
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
