using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Friends.CheckAllConditions
{
    public class CheckAllConditionsQueryHandler : IQueryHandler<CheckAllConditionsQuery, bool>
    {
        private readonly DataBaseContext _context;

        public CheckAllConditionsQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public bool Handle(CheckAllConditionsQuery query)
        {
            var friend = _context.Friends
                .FirstOrDefault(model => model.AccountId == query.AccountId && model.Id == query.FriendId);

            if (friend == null)
            {
                return false;
            }

            var result = friend.IsAddedToGroups && friend.IsAddedToPages && friend.IsWinked && friend.DialogIsCompleted;
            return result;
        }
    }
}
