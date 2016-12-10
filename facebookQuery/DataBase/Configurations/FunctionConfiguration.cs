using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class FunctionConfiguration: EntityTypeConfiguration<FunctionDbModel>
    {
        public FunctionConfiguration()
        {
            ToTable("Functions");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.FunctionName).IsRequired();
            Property(model => model.Name).IsRequired();

            HasMany(it => it.GroupFunctions).WithRequired(model => model.Function).HasForeignKey(model => model.FunctionId);
            HasRequired(it => it.FunctionType)
                .WithMany(model => model.Functions)
                .HasForeignKey(model => model.FunctionTypeId);
        }
    }
}
