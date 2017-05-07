using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.CounterCheckFriends.MarkCounterCheckFriendsCommand
{
    public class MarkCounterCheckFriendsCommandHandler : ICommandHandler<MarkCounterCheckFriendsCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkCounterCheckFriendsCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(MarkCounterCheckFriendsCommand command)
        {
            var counterModel = _context.CounterCheckFriends.FirstOrDefault(
                model => model.Id == command.AccountId);

            if (command.ResetCounter && counterModel != null)
            {
                counterModel.RetryNumber = 0;

                _context.SaveChanges();

                return new VoidCommandResponse();
            }

            if (counterModel == null)
            {
                var newCounterModel = new CounterCheckFriendsDbModel
                {
                    Id = command.AccountId,
                    RetryNumber = 1
                };
                _context.CounterCheckFriends.Add(newCounterModel);
                _context.SaveChanges();

                return new VoidCommandResponse();
            }

            counterModel.RetryNumber = command.NewRetryCount;
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
