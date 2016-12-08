using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class MessageGroupConfiguration : EntityTypeConfiguration<MessageGroupDbModel>
    {
        public MessageGroupConfiguration()
        {
            ToTable("MessageGroups");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.Name);

            HasMany(it => it.Messages).WithOptional(model => model.MessageGroup).HasForeignKey(model => model.MessageGroupId);
            HasMany(it => it.Accounts).WithOptional(model => model.MessageGroup).HasForeignKey(model => model.MessageGroupId);
            HasMany(it => it.GroupFunctions).WithRequired(model => model.MessageGroup).HasForeignKey(model => model.GroupId);
        }
    }
}
