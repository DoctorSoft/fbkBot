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
            Property(model => model.UserId);
            HasOptional(it => it.Cookies).WithRequired(m=>m.Account);
            HasMany(model => model.Messages).WithRequired(it => it.Account).HasForeignKey(model => model.AccountId);
        }
    }
}
