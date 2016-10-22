using System.Collections.Generic;

namespace DataBase.Models
{
    public class MessageDbModel
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public long ImportancyFactor { get; set; }

        public bool IsStopped { get; set; }

        public long AccountId { get; set; }

        public AccountDbModel Account { get; set; } 
    }
}
