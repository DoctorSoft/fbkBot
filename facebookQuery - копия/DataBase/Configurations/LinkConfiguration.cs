using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class LinkConfiguration : EntityTypeConfiguration<LinkDbModel>
    {
        public LinkConfiguration()
        {
            ToTable("Links");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.Link);
        }
    }
}
