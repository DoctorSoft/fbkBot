using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Notices.AddNotice;
using DataBase.QueriesAndCommands.Commands.Notices.RemoveNotice;
using DataBase.QueriesAndCommands.Queries.Notices.GetNotices;
using Services.Interfaces.Notices;
using Services.ViewModels.NoticeModels;

namespace Services.Services
{
    public class NoticeService : INotices
    {
        public string ConvertNoticeText(string functionName, string noticeText)
        {
            return string.Format("<b>[{0}]</b> {1}", functionName, noticeText);
        }

        public void AddNotice(long accountId, string noticeText)
        {
            new AddNoticeCommandHandler(new DataBaseContext()).Handle(new AddNoticeCommand
            {
                AccountId = accountId,
                NoticeText = noticeText
            });
        }

        public void ClearOldNotice()
        {
            new RemoveOldNoticesCommandHandler(new DataBaseContext()).Handle(new RemoveOldNoticesCommand());
        }

        public Dictionary<long, IEnumerable<NoticeModel>> GetLastNotices(int countLastNotices)
        {
            var notices = new GetTopNoticesCommandHandler().Handle(new GetTopNoticesCommand
            {
                Count = countLastNotices
            });

            var groupedNotices = new Dictionary<long, IEnumerable<NoticeModel>>();

            foreach (var message in notices)
            {
                groupedNotices.Add(message.Key, message.Value.Select(model => new NoticeModel
                {
                    AccountId = model.AccountId,
                    DateTime = model.DateTime,
                    Id = model.Id,
                    NoticeText = model.NoticeText
                }));
            }

            return groupedNotices;
        }
    }
}
