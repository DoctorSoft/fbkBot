using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Notices.GetNotices
{
    public class GetTopNoticesCommandHandler : ICommandHandler<GetTopNoticesCommand, Dictionary<long, IEnumerable<NoticeResponseModel>>>
    {
        private readonly DataBaseContext _context;

        public GetTopNoticesCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public Dictionary<long, IEnumerable<NoticeResponseModel>> Handle(GetTopNoticesCommand command)
        {
            var result = new Dictionary<long, IEnumerable<NoticeResponseModel>>();

            var noticesInDb = _context.Notices
                .GroupBy(model => model.AccountId)
                .ToList();

            foreach (var notice in noticesInDb)
            {
                result.Add(notice.Key, notice
                .OrderByDescending(model => model.DateTime)
                .Take(command.Count)
                .Select(model => new NoticeResponseModel
                {
                    AccountId = model.AccountId,
                    DateTime = model.DateTime,
                    Id = model.Id,
                    NoticeText = model.NoticeText
                }));
            }

            return result;
        }
    }
}
