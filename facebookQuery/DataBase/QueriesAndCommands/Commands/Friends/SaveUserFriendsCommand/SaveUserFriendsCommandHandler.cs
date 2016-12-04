using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand
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
                    model.FacebookId, 
                    model.AccountId, 
                    model.AccountWithFriend, 
                    model.FriendMessages, 
                    model.FriendName, 
                    model.DeleteFromFriends, 
                    model.Id,
                    model.Href,
                    model.Gender
                }).AsEnumerable().Select(model => new FriendDbModel
                {
                    FacebookId = model.FacebookId,
                    AccountId = model.AccountId,
                    AccountWithFriend = model.AccountWithFriend,
                    FriendMessages = model.FriendMessages,
                    FriendName = model.FriendName,
                    DeleteFromFriends = model.DeleteFromFriends,
                    Id = model.Id,
                    Gender = model.Gender,
                    Href = model.Href
                }).ToList();

                foreach (var friendDbModel in friendsInDb)
                {
                    if (command.Friends.Any(model => model.FacebookId.Equals(friendDbModel.FacebookId))) continue;
                    {
                        var deletingFriend = context.Friends
                            .FirstOrDefault(model => model.AccountId == command.AccountId 
                                && model.FacebookId == friendDbModel.FacebookId 
                                && !model.DeleteFromFriends);

                        if (deletingFriend == null) continue;
                        deletingFriend.DeleteFromFriends = true;
                        context.SaveChanges();
                    }
                }
                
                foreach (var friend in command.Friends)
                {
                    if (!friendsInDb.Any(model => model.FacebookId.Equals(friend.FacebookId)))
                    {
                        friendsList.Add(new FriendDbModel()
                        {
                            FacebookId = friend.FacebookId,
                            AccountId = command.AccountId,
                            FriendName = friend.FriendName,
                            AddedDateTime = DateTime.Now,
                            Href = friend.Href,
                            Gender = friend.Gender
                        });
                    }
                }
            }
            else
            {
                friendsList.AddRange(command.Friends.Select(friend => new FriendDbModel
                {
                    AccountId = command.AccountId,
                    FacebookId = friend.FacebookId, 
                    FriendName = friend.FriendName,
                    DeleteFromFriends = false,
                    AddedDateTime = DateTime.Now,
                    Href = friend.Href,
                    Gender = friend.Gender
                }));
            }

            context.Set<FriendDbModel>().AddRange(friendsList);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
