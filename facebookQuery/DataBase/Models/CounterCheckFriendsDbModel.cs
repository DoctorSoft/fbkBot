namespace DataBase.Models
{
    public class CounterCheckFriendsDbModel
    {
        public long Id { get; set; }

        public int RetryNumber { get; set; }

        public AccountDbModel Account { get; set; }
    }
}