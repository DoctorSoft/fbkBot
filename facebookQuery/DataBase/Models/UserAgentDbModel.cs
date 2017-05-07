using System;
using System.Collections.Generic;

namespace DataBase.Models
{
    public class UserAgentDbModel
    {
        public long Id { get; set; }

        public string UserAgentString { get; set; }

        public DateTime CreateDate { get; set; }

        public ICollection<AccountDbModel> Accounts { get; set; }
    }
}