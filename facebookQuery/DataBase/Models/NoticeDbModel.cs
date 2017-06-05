using System;

namespace DataBase.Models
{
    public class NoticeDbModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string NoticeText { get; set; }

        public DateTime DateTime { get; set; }
    }
}
