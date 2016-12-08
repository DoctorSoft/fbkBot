using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class GroupFunctionConfiguration : EntityTypeConfiguration<GroupFunctionDbModel>
    {
        public GroupFunctionConfiguration()
        {
            ToTable("GroupFunctions");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(it => it.MessageGroup).WithMany(model => model.GroupFunctions).HasForeignKey(model => model.GroupId);
            HasRequired(it => it.Function).WithMany(model => model.GroupFunctions).HasForeignKey(model => model.FunctionId);
        }
    }
}
