using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class AccountConfiguration:EntityTypeConfiguration<AccountDbModel>
    {
        public AccountConfiguration()
        {
            ToTable("Accounts");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.PageUrl);
            HasOptional(it => it.Cookies).WithOptionalPrincipal(m=>m.Account);
        }
    }
}
