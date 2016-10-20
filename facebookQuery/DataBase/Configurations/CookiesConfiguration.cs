using System.ComponentModel.DataAnnotations.Schema;
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
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.Locale);
            Property(model => model.Av);
            Property(model => model.Datr);
            Property(model => model.Sb);
            Property(model => model.CUser);
            Property(model => model.Xs);
            Property(model => model.Fr);
            Property(model => model.Csm);
            Property(model => model.S);
            Property(model => model.Pl);
            Property(model => model.Lu);
            Property(model => model.P);
            Property(model => model.Act);
            Property(model => model.Wd);
            Property(model => model.Presence);
        }
    }
}
