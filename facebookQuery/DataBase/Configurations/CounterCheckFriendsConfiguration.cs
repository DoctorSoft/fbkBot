using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class CounterCheckFriendsConfiguration : EntityTypeConfiguration<CounterCheckFriendsDbModel>
    {
        public CounterCheckFriendsConfiguration()
        {
            ToTable("CounterCheckFriends");

            HasKey(model => model.Id);

            Property(model => model.RetryNumber);

            HasRequired(it => it.Account).WithOptional(model => model.CounterCheckFriends);
        }
    }
}
