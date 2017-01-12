using System;
using System.Collections.Generic;
using System.Linq;
using Constants.FriendTypesEnum;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetAnalisysFriendsQueryHandler : IQueryHandler<GetAnalisysFriendsQuery, List<AnalysisFriendData>>
    {
        private readonly DataBaseContext context;

        public GetAnalisysFriendsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<AnalysisFriendData> Handle(GetAnalisysFriendsQuery query)
        {
            try
            {

                var analisisFriends = context.AnalisysFriends
                .Where(model => model.Status == StatusesFriend.ToAnalys)
                .Where(model => !model.AccountWithFriend.IsDeleted)
                .GroupBy(model => model.AccountId)
                .Select(model => model.OrderBy(dbModel => dbModel.AddedDateTime).FirstOrDefault()).ToList();

                var result = new List<AnalysisFriendData>();

                foreach (var analysisFriendDbModel in analisisFriends)
                {
                    var dbModel = analysisFriendDbModel;
                    var settingsModel = context.AccountSettings.FirstOrDefault(model => model.Id == dbModel.AccountId);

                    if (settingsModel == null)
                    {
                        continue;
                    }

                    if (settingsModel.LivesPlace == null && settingsModel.Gender == null &&
                        settingsModel.SchoolPlace == null && settingsModel.WorkPlace == null) //replace
                    {
                        continue;
                    }

                    result.Add(new AnalysisFriendData
                    {
                        FacebookId = analysisFriendDbModel.FacebookId,
                        AccountId = analysisFriendDbModel.AccountId,
                        FriendName = analysisFriendDbModel.FriendName,
                        Id = analysisFriendDbModel.Id,
                        AddedToAnalysDateTime = analysisFriendDbModel.AddedDateTime,
                        Type = analysisFriendDbModel.Type,
                        Status = analysisFriendDbModel.Status
                    });
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
