using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand
{
    public class SaveFriendsForAnalysisCommandHandler : ICommandHandler<SaveFriendsForAnalysisCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public SaveFriendsForAnalysisCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(SaveFriendsForAnalysisCommand command)
        {
            var friendsList = new List<AnalysisFriendDbModel>();

            var friendsInDb = context.AnalisysFriends.Where(model => model.AccountId == command.AccountId).Select(model => new
            {
                model.FacebookId,
                model.AccountId,
                model.AccountWithFriend,
                model.FriendName,
                model.Status,
                model.Id,
                model.Type
            }).AsEnumerable().Select(model => new AnalysisFriendDbModel
            {
                FacebookId = model.FacebookId,
                AccountId = model.AccountId,
                AccountWithFriend = model.AccountWithFriend,
                FriendName = model.FriendName,
                Id = model.Id,
                Type = model.Type,
                Status = model.Status
            }).ToList();

            foreach (var friendDbModel in command.Friends)
            {
                if (friendsInDb.Any(model => model.FacebookId.Equals(friendDbModel.FacebookId)))
                {
                    continue;
                }

                if (friendsList.Any(model=>model.FacebookId.Equals(friendDbModel.FacebookId)))
                {
                    continue;
                }
                friendsList.Add(new AnalysisFriendDbModel
                {
                    FacebookId = friendDbModel.FacebookId,
                    AccountId = command.AccountId,
                    FriendName = friendDbModel.FriendName,
                    AddedDateTime = DateTime.Now,
                    Type = friendDbModel.Type,
                    Status = friendDbModel.Status,
                });
            }

            context.Set<AnalysisFriendDbModel>().AddRange(friendsList);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
