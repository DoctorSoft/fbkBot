using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.SpyAccounts
{
    public class AddOrUpdateSpyAccountCommandHandler : ICommandHandler<AddOrUpdateSpyAccountCommand, long>
    {
        private readonly DataBaseContext context;

        public AddOrUpdateSpyAccountCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public long Handle(AddOrUpdateSpyAccountCommand command)
        {
            var account = context.SpyAccounts.FirstOrDefault(model => model.Id == command.Id);

            if (account == null)
            {
                account = new SpyAccountDbModel();
            }

            account.FacebookId = command.FacebookId;
            account.Login = command.Login;
            account.Name = command.Name;
            account.PageUrl = command.PageUrl;
            account.Password = command.Password;
            account.Proxy = command.Proxy;
            account.ProxyLogin = command.ProxyLogin;
            account.ProxyPassword = command.ProxyPassword;

            context.SpyAccounts.AddOrUpdate(account);

            context.SaveChanges();

            return account.Id;
        }
    }
}
