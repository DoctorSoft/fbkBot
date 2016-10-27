using System;

namespace DataBase.Models
{
    public class CookiesDbModel
    {
        public long Id { get; set; }

        public string CookiesString { get; set; }

        public DateTime CreateDate { get; set; }

        public AccountDbModel Account { get; set; }
    }
}