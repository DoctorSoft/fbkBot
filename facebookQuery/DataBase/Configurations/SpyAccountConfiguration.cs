using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class SpyAccountConfiguration  : EntityTypeConfiguration<SpyAccountDbModel>
    {
        public SpyAccountConfiguration()
        {
            ToTable("SpyAccounts");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.Name);
            Property(model => model.Login);
            Property(model => model.Password);
            Property(model => model.PageUrl);
            Property(model => model.FacebookId);
            Property(model => model.Proxy);
            Property(model => model.ProxyLogin);
            Property(model => model.ProxyPassword);
            Property(model => model.AuthorizationDataIsFailed);
            Property(model => model.ProxyDataIsFailed);
            Property(model => model.ConformationIsFailed);

            HasOptional(it => it.Cookies).WithRequired(m=>m.SpyAccount);
        }
    }
}
