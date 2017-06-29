namespace DataBase.QueriesAndCommands.Queries.Account.CheckExistLogin
{
    public class CheckExistLoginQuery : IQuery<bool>
    {
        public string Login { get; set; }
    }
}
