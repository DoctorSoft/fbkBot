
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.Models;
using EntityFramework.Extensions;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriends
{
    public class SaveUserFriendsCommandHandler : ICommandHandler<SaveUserFriendsCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public SaveUserFriendsCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }
        public VoidCommandResponse Handle(SaveUserFriendsCommand command)
        {
            var friendsList = new List<FriendDbModel>();

            var friendsInDb = context.Friends.Where(model=>model.AccountId == command.AccountId).Select(model => new FriendDbModel
            {
                AccountId = model.AccountId,
                FriendId = model.FriendId,
                AccountWithFriend = model.AccountWithFriend,
                FriendMessages = model.FriendMessages,
                FriendName = model.FriendName
            });

            if (friendsInDb.Count() != 0)
            {
                friendsList.AddRange(from friend in friendsInDb
                    where command.Friends.All(model => model.FriendId != friend.FriendId)
                    select new FriendDbModel
                    {
                        AccountId = command.AccountId, FriendId = friend.FriendId, FriendName = friend.FriendName, DeleteFromFriends = true
                    });
            }
            else
            {
                friendsList.AddRange(command.Friends.Select(friend => new FriendDbModel
                {
                    AccountId = command.AccountId, 
                    FriendId = friend.FriendId,
                    FriendName = friend.FriendName,
                    DeleteFromFriends = false
                }));
            }

            context.Set<FriendDbModel>().AddRange(friendsList);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
