using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class SettingsConfiguration : EntityTypeConfiguration<SettingsDbModel>
    {
        public SettingsConfiguration()
        {
            ToTable("Settings");

            HasKey(model => model.Id);
            Property(model => model.Countries);
            Property(model => model.Cities);
            Property(model => model.Gender);
            Property(model => model.RetryTimeConfirmFriendships);
            Property(model => model.RetryTimeGetNewAndRecommendedFriends);
            Property(model => model.RetryTimeRefreshFriends);
            Property(model => model.RetryTimeSendNewFriend);
            Property(model => model.RetryTimeSendRequestFriendships);
            Property(model => model.RetryTimeSendUnanswered);
            Property(model => model.RetryTimeSendUnread);
            Property(model => model.UnansweredDelay);

            HasRequired(it => it.SettingsGroup).WithOptional(m=>m.Settings);
        }
    }
}
