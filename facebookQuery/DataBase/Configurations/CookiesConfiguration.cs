using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class CookiesConfiguration : EntityTypeConfiguration<CookiesDbModel>
    {
        public CookiesConfiguration()
        {
            ToTable("Cookies");

            HasKey(model => model.Id);

            Property(model => model.CookiesString);

            HasRequired(it => it.Account).WithOptional(model => model.Cookies);
        }
    }
}
