using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Accounts
{
    public class AddOrUpdateAccountCommandHandler : ICommandHandler<AddOrUpdateAccountCommand, long>
    {
        private readonly DataBaseContext context;

        public AddOrUpdateAccountCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public long Handle(AddOrUpdateAccountCommand command)
        {
            var account = context.Accounts.FirstOrDefault(model => model.Id == command.Id);

            if (account == null)
            {
                account = new AccountDbModel();
            }

            if (command.Id != null)
            {
                account.Id = (long)command.Id;
            }
            account.FacebookId = command.FacebookId;
            account.Login = command.Login;
            account.Name = command.Name;
            account.PageUrl = command.PageUrl;
            account.Password = command.Password;
            account.Proxy = command.Proxy;
            account.ProxyLogin = command.ProxyLogin;
            account.ProxyPassword = command.ProxyPassword;

            context.Accounts.AddOrUpdate(account);

            context.SaveChanges();

            return account.Id;
        }
    }
}
