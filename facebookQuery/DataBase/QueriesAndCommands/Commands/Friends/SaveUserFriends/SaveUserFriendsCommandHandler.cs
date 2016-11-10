
using System.Collections.Generic;
using System.Data.Entity;
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

            if (context.Friends.Any())
            {
                var friendsInDb = context.Friends.Where(model => model.AccountId == command.AccountId).Select(model => new 
                {
                    model.AccountId, 
                    model.FriendId, 
                    model.AccountWithFriend, 
                    model.FriendMessages, 
                    model.FriendName, 
                    model.DeleteFromFriends, 
                    model.Id
                }).AsEnumerable().Select(model => new FriendDbModel
                {
                    AccountId = model.AccountId,
                    FriendId = model.FriendId,
                    AccountWithFriend = model.AccountWithFriend,
                    FriendMessages = model.FriendMessages,
                    FriendName = model.FriendName,
                    DeleteFromFriends = model.DeleteFromFriends,
                    Id = model.Id
                }).ToList();

                foreach (var friendDbModel in friendsInDb)
                {
                    if (!command.Friends.Any(model=> model.FriendId.Equals(friendDbModel.FriendId)))
                    {
                        var deletingFriend = context.Friends
                            .FirstOrDefault(model => model.AccountId == command.AccountId && model.FriendId == friendDbModel.FriendId && !model.DeleteFromFriends);

                        if (deletingFriend != null)
                        {
                            deletingFriend.DeleteFromFriends = true;
                            context.SaveChanges();
                        }
                    }
                }
                
                foreach (var friend in command.Friends)
                {
                    if (!friendsInDb.Any(model => model.FriendId.Equals(friend.FriendId)))
                    {
                        friendsList.Add(new FriendDbModel()
                        {
                            AccountId = command.AccountId,
                            FriendId = friend.FriendId,
                            FriendName = friend.FriendName, 
                        });
                    }
                }
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
