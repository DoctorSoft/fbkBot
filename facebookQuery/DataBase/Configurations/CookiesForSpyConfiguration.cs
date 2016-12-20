using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class CookiesForSpyConfiguration : EntityTypeConfiguration<CookiesForSpyDbModel>
    {
        public CookiesForSpyConfiguration()
        {
            ToTable("CookiesForSpy");

            HasKey(model => model.Id);

            Property(model => model.CookiesString);

            HasRequired(it => it.SpyAccount).WithOptional(model => model.Cookies);
        }
    }
}
