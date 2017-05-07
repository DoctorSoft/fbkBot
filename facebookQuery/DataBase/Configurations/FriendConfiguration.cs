using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class FriendConfiguration : EntityTypeConfiguration<FriendDbModel>
    {
        public FriendConfiguration()
        {
            ToTable("Friends");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.FriendName);
            Property(model => model.FacebookId);
            Property(model => model.MessageRegime);
            Property(model => model.Href);
            Property(model => model.Gender);
            Property(model => model.FriendType);
            Property(model => model.DialogIsCompleted);
            Property(model => model.IsAddedToGroups);
            Property(model => model.IsAddedToPages);
            Property(model => model.IsWinked);
            Property(model => model.IsWinkedFriendsFriend);
            Property(model => model.AddedToRemoveDateTime);
            Property(model => model.CountWinksToFriends);

            HasRequired(it => it.AccountWithFriend).WithMany(m => m.Friends).HasForeignKey(model => model.AccountId);
            HasMany(it => it.FriendMessages).WithRequired(model => model.Friend).HasForeignKey(model => model.FriendId);
        }
    }
}
