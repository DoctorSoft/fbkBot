using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class NewSettingsConfiguration : EntityTypeConfiguration<NewSettingsDbModel>
    {
        public NewSettingsConfiguration()
        {
            ToTable("NewSettings");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.CommunityOptions);

            HasRequired(it => it.Account).WithMany(model => model.NewSettings).HasForeignKey(model => model.AccountId);
            HasRequired(it => it.SettingsGroup).WithMany(model => model.NewSettings).HasForeignKey(model => model.SettingsGroupId);
        }
    }
}
