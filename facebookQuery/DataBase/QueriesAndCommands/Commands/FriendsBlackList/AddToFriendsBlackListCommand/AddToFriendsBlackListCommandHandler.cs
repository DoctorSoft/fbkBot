using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.FriendsBlackList.AddToFriendsBlackListCommand
{
    public class AddToFriendsBlackListCommandHandler : ICommandHandler<AddToFriendsBlackListCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public AddToFriendsBlackListCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(AddToFriendsBlackListCommand command)
        {
            var friendDbModel = _context.FriendsBlackList
                .FirstOrDefault(model => model.FriendFacebookId == command.FriendFacebookId 
                && model.GroupId == command.GroupSettingsId);

            if (friendDbModel!=null)
            {
                friendDbModel.DateAdded = DateTime.Now;

                _context.FriendsBlackList.AddOrUpdate(friendDbModel);
                _context.SaveChanges();
                
                return new VoidCommandResponse();
            }

            friendDbModel = new FriendsBlackListDbModel
            {
                GroupId = command.GroupSettingsId,
                FriendName = command.FriendName,
                FriendFacebookId = command.FriendFacebookId,
                DateAdded = DateTime.Now
            };

            _context.FriendsBlackList.AddOrUpdate(friendDbModel);

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
