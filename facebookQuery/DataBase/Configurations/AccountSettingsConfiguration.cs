using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class AccountSettingsConfiguration : EntityTypeConfiguration<AccountSettingsDbModel>
    {
        public AccountSettingsConfiguration()
        {
            ToTable("AccountSettings");

            HasKey(model => model.Id);
            Property(model => model.LivesPlace);
            Property(model => model.SchoolPlace);
            Property(model => model.WorkPlace);
            Property(model => model.Gender);

            HasRequired(it => it.Account).WithOptional(m=>m.Settings);
        }
    }
}
