using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class AccountConfiguration : EntityTypeConfiguration<AccountDbModel>
    {
        public AccountConfiguration()
        {
            ToTable("Accounts");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.Login);
            Property(model => model.Password);
            Property(model => model.PageUrl);
            Property(model => model.UserId);

            HasOptional(it => it.Cookies).WithRequired(m=>m.Account);
            HasMany(model => model.Messages).WithOptional(it => it.Account).HasForeignKey(model => model.AccountId);
            HasMany(model => model.Friends).WithRequired(it => it.AccountWithFriend).HasForeignKey(model => model.AccountId);
        }
    }
}
