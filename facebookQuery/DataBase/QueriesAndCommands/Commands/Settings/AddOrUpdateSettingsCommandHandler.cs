using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Settings
{
    public class AddOrUpdateSettingsCommandHandler : ICommandHandler<AddOrUpdateSettingsCommand, long>
    {
        private readonly DataBaseContext context;

        public AddOrUpdateSettingsCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public long Handle(AddOrUpdateSettingsCommand command)
        {
            var accountSettings = context.Settings.FirstOrDefault(model => model.Id == command.GroupId);

            if (accountSettings == null)
            {
                accountSettings = new SettingsDbModel();
            }
            if (command.GroupId != null)
            {
                accountSettings.Id = command.GroupId;
            }
            accountSettings.Gender = command.Gender;
            accountSettings.LivesPlace = command.LivesPlace;
            accountSettings.SchoolPlace = command.SchoolPlace;
            accountSettings.WorkPlace = command.WorkPlace;
            accountSettings.DelayTimeSendNewFriend = command.DelayTimeSendNewFriend;
            accountSettings.DelayTimeSendUnanswered = command.DelayTimeSendUnanswered;
            accountSettings.DelayTimeSendUnread = command.DelayTimeSendUnread;
            accountSettings.UnansweredDelay = command.UnansweredDelay;

            context.Settings.AddOrUpdate(accountSettings);

            context.SaveChanges();

            return accountSettings.Id;
        }
    }
}
