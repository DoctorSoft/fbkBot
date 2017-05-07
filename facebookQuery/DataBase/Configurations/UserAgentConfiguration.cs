using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class UserAgentConfiguration : EntityTypeConfiguration<UserAgentDbModel>
    {
        public UserAgentConfiguration()
        {
            ToTable("UserAgents");

            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.CreateDate);
            Property(model => model.UserAgentString).IsRequired();

            HasMany(it => it.Accounts).WithOptional(model => model.UserAgent).HasForeignKey(model => model.UserAgentId);
        }
    }
}
