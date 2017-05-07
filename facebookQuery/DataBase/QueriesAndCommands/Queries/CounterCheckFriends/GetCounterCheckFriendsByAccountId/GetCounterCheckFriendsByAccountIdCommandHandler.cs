using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.CounterCheckFriends.GetCounterCheckFriendsByAccountId
{
    public class GetCounterCheckFriendsByAccountIdCommandHandler : ICommandHandler<GetCounterCheckFriendsByAccountIdCommand, GetCounterCheckFriendsByAccountIdModel>
    {
        private readonly DataBaseContext _context;

        public GetCounterCheckFriendsByAccountIdCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public GetCounterCheckFriendsByAccountIdModel Handle(GetCounterCheckFriendsByAccountIdCommand command)
        {
            var counterModel = _context.CounterCheckFriends.FirstOrDefault(model => model.Id == command.AccountId);

            if (counterModel != null)
            {
                return new GetCounterCheckFriendsByAccountIdModel
                {
                    AccountId = counterModel.Id,
                    RetryNumber = counterModel.RetryNumber
                };
            }
            
            return new GetCounterCheckFriendsByAccountIdModel
            {
                AccountId = command.AccountId,
                RetryNumber = 0
            };
        }
    }
}
