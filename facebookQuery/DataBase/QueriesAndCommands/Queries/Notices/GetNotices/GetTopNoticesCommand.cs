using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.Notices.GetNotices
{
    public class GetTopNoticesCommand : ICommand<Dictionary<long, IEnumerable<NoticeResponseModel>>>
    {
        public int Count { get; set; }
    }
}
