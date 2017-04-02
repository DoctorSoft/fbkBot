namespace DataBase.Models
{
    public class AccountInformationDbModel
    {
        public long Id { get; set; }

        public string Information { get; set; }

        public AccountDbModel Account { get; set; }
    }
}
