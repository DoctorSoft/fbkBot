using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class SpyFunctionConfiguration : EntityTypeConfiguration<SpyFunctionDbModel>
    {
        public SpyFunctionConfiguration()
        {
            ToTable("SpyFunctions");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(it => it.Spy).WithMany(model => model.Functions).HasForeignKey(model => model.SpyId);
            HasRequired(it => it.Function).WithMany(model => model.SpyFunctions).HasForeignKey(model => model.FunctionId);
        }
    }
}
