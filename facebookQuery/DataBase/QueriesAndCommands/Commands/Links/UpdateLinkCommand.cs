namespace DataBase.QueriesAndCommands.Commands.Links
{
    public class UpdateLinkCommand : IVoidCommand
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
