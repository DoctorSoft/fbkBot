using System.Collections.Generic;

namespace DataBase.Models
{
    public class MessageGroupDbModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<MessageDbModel> Messages { get; set; }

        public ICollection<AccountDbModel> Accounts { get; set; } 
    }
}
