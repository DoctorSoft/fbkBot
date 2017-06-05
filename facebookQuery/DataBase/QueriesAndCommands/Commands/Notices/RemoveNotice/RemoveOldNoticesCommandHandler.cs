using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Notices.RemoveNotice
{
    public class RemoveOldNoticesCommandHandler : ICommandHandler<RemoveOldNoticesCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public RemoveOldNoticesCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(RemoveOldNoticesCommand command)
        {
            var notices = _context.Notices.ToList();

            var groupByListToRemove = notices.GroupBy(x => x.AccountId)
                                          .Select(x => x.OrderByDescending(y => y.DateTime)
                                                        .Skip(10).ToList());

            var listToRemove = groupByListToRemove.SelectMany(x => x);

            _context.Notices.RemoveRange(listToRemove);
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
