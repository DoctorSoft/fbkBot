namespace DataBase.QueriesAndCommands.Commands.Groups
{
    public class UpdateGroupCommand : IVoidCommand
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
