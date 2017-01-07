using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.AccountSettings
{
    public class AddOrUpdateAccountSettingsCommandHandler : ICommandHandler<AddOrUpdateAccountSettingsCommand, long>
    {
        private readonly DataBaseContext context;

        public AddOrUpdateAccountSettingsCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public long Handle(AddOrUpdateAccountSettingsCommand command)
        {
            var accountSettings = context.AccountSettings.FirstOrDefault(model => model.Id == command.AccountId);

            if (accountSettings == null)
            {
                accountSettings = new AccountSettingsDbModel();
            }

            accountSettings.Id = command.AccountId;
            accountSettings.Gender = command.Gender;
            accountSettings.LivesPlace = command.LivesPlace;
            accountSettings.SchoolPlace = command.SchoolPlace;
            accountSettings.WorkPlace = command.WorkPlace;

            context.AccountSettings.AddOrUpdate(accountSettings);

            context.SaveChanges();

            return accountSettings.Id;
        }
    }
}
