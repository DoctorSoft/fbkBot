using System;
using System.Collections.Generic;
using System.Linq;
using Constants.FriendTypesEnum;
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
                    model.Gender,
                    model.FriendType
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
                    Href = model.Href,
                    FriendType = model.FriendType
                }).ToList();

                foreach (var friendDbModel in friendsInDb)
                {
                    if (friendDbModel.FriendType == FriendTypes.NotFriends) //Не помечаем как удаленный друзей, которые написали будучи не в друзьях
                    {
                        continue;
                    }

                    if (command.Friends.Any(model => model.FacebookId.Equals(friendDbModel.FacebookId)))
                    {
                        continue;
                    }

                    var deletingFriend = context.Friends
                        .FirstOrDefault(model => model.AccountId == command.AccountId 
                            && model.FacebookId == friendDbModel.FacebookId 
                            && !model.DeleteFromFriends);

                    var isBlocked = context.FriendsBlackList
                    .Any(model => model.FriendFacebookId == friendDbModel.FacebookId);

                    if (deletingFriend != null)
                    {
                        deletingFriend.DeleteFromFriends = true;
                        context.SaveChanges();
                    }

                    if (isBlocked && deletingFriend != null)
                    {
                        context.Friends.Remove(deletingFriend);
                        context.SaveChanges();
                    }

                }
                
                foreach (var friend in command.Friends)
                {
                    if (!friendsInDb.Any(model => model.FacebookId.Equals(friend.FacebookId)))
                    {
                        friendsList.Add(new FriendDbModel
                        {
                            FacebookId = friend.FacebookId,
                            AccountId = command.AccountId,
                            FriendName = friend.FriendName,
                            AddedDateTime = DateTime.Now,
                            Href = friend.Href,
                            Gender = friend.Gender,
                            FriendType = FriendTypes.InFriends
                        });
                    }
                }

                var deletedFriends =
                    context.Friends.Where(model => model.AccountId == command.AccountId && model.DeleteFromFriends).ToList();

                foreach (var deletedFriend in deletedFriends)
                {
                    try
                    {
                        var isBan = context.FriendsBlackList.Any(model => model.FriendFacebookId == deletedFriend.FacebookId);
                        var inCurrentFriends = command.Friends.Any(model => model.FacebookId == deletedFriend.FacebookId);
                        //Если среди удаленных находится друг, которого мы получили в текущем списке
                        if (deletedFriend.FacebookId == 100006675100751)
                        {
                            string s = ";";
                        }
                        if (!isBan && inCurrentFriends)
                        {
                            var friend = deletedFriend;
                            var notDeleteFreind = deletedFriends.Where(model => model.Id == friend.Id).FirstOrDefault();

                            notDeleteFreind.DeleteFromFriends = false;
                            context.SaveChanges();
                        }
                    }
                    catch (Exception)
                    {
                        
                        throw;
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
                    Gender = friend.Gender,
                    FriendType = FriendTypes.InFriends
                }));
            }

            context.Set<FriendDbModel>().AddRange(friendsList);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
