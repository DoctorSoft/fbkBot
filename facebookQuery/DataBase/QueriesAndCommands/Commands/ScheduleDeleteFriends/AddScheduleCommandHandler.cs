using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Dynamic;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.ScheduleDeleteFriends
{
    public class AddScheduleCommandHandler : ICommandHandler<AddScheduleCommand, long>
    {
        private readonly DataBaseContext _context;

        public AddScheduleCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public long Handle(AddScheduleCommand command)
        {
            var dublicates = _context.ScheduleRemovalOfFriends.Where(model => model.FriendId == command.FriendId && model.AccountId == command.AccountId && model.FunctionName == command.FunctionName);

            _context.ScheduleRemovalOfFriends.RemoveRange(dublicates);

            var scheduleData = new ScheduleRemovalOfFriendsDbModel
                            {
                                AccountId = command.AccountId,
                                FunctionName = command.FunctionName,
                                AddDateTime = command.AddedDateTime,
                                FriendId = command.FriendId
                            };

            _context.ScheduleRemovalOfFriends.Add(scheduleData);

            _context.SaveChanges();

            return scheduleData.Id;
        }
    }
}
