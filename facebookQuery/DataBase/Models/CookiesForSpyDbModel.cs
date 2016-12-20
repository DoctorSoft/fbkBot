using System;

namespace DataBase.Models
{
    public class CookiesForSpyDbModel
    {
        public long Id { get; set; }

        public string CookiesString { get; set; }

        public DateTime CreateDate { get; set; }

        public SpyAccountDbModel SpyAccount { get; set; }
    }
}