using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class GroupSettingsConfiguration : EntityTypeConfiguration<GroupSettingsDbModel>
    {
        public GroupSettingsConfiguration()
        {
            ToTable("GroupSettings");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.Name);

            HasOptional(it => it.Settings).WithRequired(m => m.SettingsGroup);
            HasMany(it => it.Messages).WithOptional(model => model.GroupSettings).HasForeignKey(model => model.MessageGroupId);
            HasMany(it => it.Accounts).WithOptional(model => model.GroupSettings).HasForeignKey(model => model.GroupSettingsId);
            HasMany(it => it.GroupFunctions).WithRequired(model => model.MessageGroup).HasForeignKey(model => model.GroupId);
            HasMany(model => model.NewSettings).WithRequired(it => it.SettingsGroup).HasForeignKey(model => model.SettingsGroupId);
        }
    }
}
