using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.ScheduleDeleteFriends
{
    public class DeleteScheduleByIdCommandHandler : ICommandHandler<DeleteScheduleByIdCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public DeleteScheduleByIdCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(DeleteScheduleByIdCommand command)
        {
            try
            {
                var dbModels = command.Schedules.Select(model => new ScheduleRemovalOfFriendsDbModel
                {
                    FunctionName = model.FunctionName,
                    AccountId = model.AccountId,
                    FriendId = model.FriendId,
                    AddDateTime = model.AddDateTime,
                    Id = model.Id
                });

                foreach (var record in dbModels)
                {
                    _context.ScheduleRemovalOfFriends.Attach(record);
                    _context.ScheduleRemovalOfFriends.Remove(record);
                }

                _context.SaveChanges();
            }
            catch
            {
            }

            return new VoidCommandResponse();
        }
    }
}
