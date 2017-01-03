namespace DataBase.QueriesAndCommands.Commands.Cookies
{
    public class UpdateCookiesForSpyCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public string NewCookieString { get; set; }
    }
}
