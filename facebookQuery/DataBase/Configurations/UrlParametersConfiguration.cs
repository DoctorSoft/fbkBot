using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class UrlParametersConfiguration: EntityTypeConfiguration<UrlParametersDbModel>
    {
        public UrlParametersConfiguration ()
        {
            ToTable("UrlParameters");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.CodeParameters);
            Property(model => model.ParametersSet);
        }
    }
}
