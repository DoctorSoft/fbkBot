namespace DataBase.QueriesAndCommands.Commands.Accounts
{
    public class AddOrUpdateAccountCommand : ICommand<long>
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PageUrl { get; set; }

        public long FacebookId { get; set; } 
    }
}
