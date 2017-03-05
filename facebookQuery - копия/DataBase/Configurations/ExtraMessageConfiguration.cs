using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class ExtraMessageConfiguration : EntityTypeConfiguration<ExtraMessageDbModel>
    {
        public ExtraMessageConfiguration()
        {
            ToTable("ExtraMessages");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.Message);
        }
    }
}
