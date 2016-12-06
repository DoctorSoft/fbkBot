using System;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand
{
    public class SaveNewFriendCommandHandler : ICommandHandler<SaveNewFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public SaveNewFriendCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }
        public VoidCommandResponse Handle(SaveNewFriendCommand command)
        {
            context.Set<FriendDbModel>().Add(command.FriendData);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
