using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class FriendMessageConfiguration : EntityTypeConfiguration<FriendMessageDbModel>
    {
        public FriendMessageConfiguration()
        {
            ToTable("FriendMessages");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.Message);
            Property(model => model.LastReadMessageDateTime);
            Property(model => model.LastUnreadMessageDateTime);
            Property(model => model.MessageDirection);

            HasRequired(it => it.Friend).WithMany(model => model.FriendMessages).HasForeignKey(model => model.FriendId);
        }
    }
}
