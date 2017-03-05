using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class StopWordConfiguration : EntityTypeConfiguration<StopWordDbModel>
    {
        public StopWordConfiguration()
        {
            ToTable("StopWords");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.Word);
        }
    }
}
