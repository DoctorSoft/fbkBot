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

            Property(model => model.FriendsOptions);
            Property(model => model.GeoOptions);
            Property(model => model.MessageOptions);
            Property(model => model.LimitsOptions);

            HasRequired(it => it.SettingsGroup).WithOptional(m=>m.Settings);
        }
    }
}
