using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class ScheduleRemovalOfFriendsConfiguration : EntityTypeConfiguration<ScheduleRemovalOfFriendsDbModel>
    {
        public ScheduleRemovalOfFriendsConfiguration()
        {
            ToTable("ScheduleRemovalOfFriends");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.AccountId);
            Property(model => model.FriendId);
            Property(model => model.FunctionName);
            Property(model => model.AddDateTime);
        }
    }
}
