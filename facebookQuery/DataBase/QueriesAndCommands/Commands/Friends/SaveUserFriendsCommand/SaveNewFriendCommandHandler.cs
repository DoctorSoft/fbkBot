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
            var account = context.Accounts.FirstOrDefault(model => model.FacebookId == command.AccountId);
            var friend = new FriendDbModel()
            {
                AccountWithFriend = account,
                AccountId = account.Id,
                IsBlocked = false,
                MessageRegime = MessageRegime.UserFirstMessage,
                Gender = command.FriendData.Gender,
                Href = command.FriendData.Href,
                FacebookId = command.FriendData.FacebookId,
                FriendName = command.FriendData.FriendName,
                DeleteFromFriends = false,
                AddedDateTime = DateTime.Now,
            };

            context.Set<FriendDbModel>().Add(friend);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
