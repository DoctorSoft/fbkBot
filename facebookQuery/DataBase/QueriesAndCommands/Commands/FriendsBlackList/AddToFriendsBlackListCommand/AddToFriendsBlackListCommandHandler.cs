using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.FriendsBlackList.AddToFriendsBlackListCommand
{
    public class AddToFriendsBlackListCommandHandler : ICommandHandler<AddToFriendsBlackListCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public AddToFriendsBlackListCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(AddToFriendsBlackListCommand command)
        {
            var friendDbModel = context.FriendsBlackList.FirstOrDefault(model => model.FriendFacebookId == command.FriendFacebookId);

            if (friendDbModel!=null)
            {
                friendDbModel.DateAdded = DateTime.Now;

                context.FriendsBlackList.AddOrUpdate(friendDbModel);
                context.SaveChanges();
                
                return new VoidCommandResponse();
            }

            friendDbModel = new FriendsBlackListDbModel
            {
                GroupId = command.GroupSettingsId,
                FriendName = command.FriendName,
                FriendFacebookId = command.FriendFacebookId,
                DateAdded = DateTime.Now
            };

            context.FriendsBlackList.AddOrUpdate(friendDbModel);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
