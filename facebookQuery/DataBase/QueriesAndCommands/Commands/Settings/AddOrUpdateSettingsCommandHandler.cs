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
            accountSettings.Countries = command.Countries;
            accountSettings.Cities = command.Cities;
            accountSettings.RetryTimeConfirmFriendships = command.RetryTimeConfirmFriendships;
            accountSettings.RetryTimeGetNewAndRecommendedFriends = command.RetryTimeGetNewAndRecommendedFriends;
            accountSettings.RetryTimeRefreshFriends = command.RetryTimeRefreshFriends;
            accountSettings.RetryTimeSendNewFriend = command.RetryTimeSendNewFriend;
            accountSettings.RetryTimeSendRequestFriendships = command.RetryTimeSendRequestFriendships;
            accountSettings.RetryTimeSendUnanswered = command.RetryTimeSendUnanswered;
            accountSettings.RetryTimeSendUnread = command.RetryTimeSendUnread;
            accountSettings.UnansweredDelay = command.UnansweredDelay;

            context.Settings.AddOrUpdate(accountSettings);

            context.SaveChanges();

            return accountSettings.Id;
        }
    }
}
