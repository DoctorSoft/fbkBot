using System;

namespace DataBase.QueriesAndCommands.Queries.Notices.GetNotices
{
    public class NoticeResponseModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string NoticeText { get; set; }

        public DateTime DateTime { get; set; }
    }
}
