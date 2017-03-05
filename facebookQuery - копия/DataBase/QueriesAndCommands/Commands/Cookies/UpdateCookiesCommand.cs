namespace DataBase.QueriesAndCommands.Commands.Cookies
{
    public class UpdateCookiesCommand: IVoidCommand
    {
        public long AccountId { get; set; }

        public string NewCookieString { get; set; }
    }
}
