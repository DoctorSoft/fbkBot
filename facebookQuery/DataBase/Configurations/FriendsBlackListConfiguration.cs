using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class FriendsBlackListConfiguration : EntityTypeConfiguration<FriendsBlackListDbModel>
    {
        public FriendsBlackListConfiguration()
        {
            ToTable("FriendsBlackList");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.FriendFacebookId);
            Property(model => model.GroupId);
            Property(model => model.FriendName);
            Property(model => model.DateAdded);
        }
    }
}
