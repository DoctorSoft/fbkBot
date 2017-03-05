using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class FunctionTypeConfiguration : EntityTypeConfiguration<FunctionTypeDbModel>
    {
        public FunctionTypeConfiguration()
        {
            ToTable("FunctionTypes");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.FunctionTypeName).IsRequired();
            Property(model => model.TypeName).IsRequired();

            HasMany(it => it.Functions).WithRequired(model => model.FunctionType).HasForeignKey(model => model.FunctionTypeId);
        }
    }
}
