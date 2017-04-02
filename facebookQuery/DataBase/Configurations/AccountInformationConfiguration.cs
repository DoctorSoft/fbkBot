using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class AccountInformationConfiguration : EntityTypeConfiguration<AccountInformationDbModel>
    {
        public AccountInformationConfiguration()
        {
            ToTable("AccountInformation");

            HasKey(model => model.Id);
            
            Property(model => model.Information);

            HasRequired(it => it.Account).WithOptional(model => model.AccountInformation);
        }
    }
}
