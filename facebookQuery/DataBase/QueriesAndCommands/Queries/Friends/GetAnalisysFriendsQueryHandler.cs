using System;
using System.Collections.Generic;
using System.Linq;
using Constants.FriendTypesEnum;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Settings;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetAnalisysFriendsQueryHandler : IQueryHandler<GetAnalisysFriendsQuery, List<AnalysisFriendData>>
    {
        private readonly DataBaseContext _context;

        public GetAnalisysFriendsQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<AnalysisFriendData> Handle(GetAnalisysFriendsQuery query)
        {
            try
            {
                var analisisFriends = _context.AnalisysFriends
                .Where(model => model.Status == StatusesFriend.ToAnalys)
                .Where(model => !model.AccountWithFriend.IsDeleted)
                .GroupBy(model => model.AccountId)
                .Select(model => model.OrderBy(dbModel => dbModel.AddedDateTime).Take(2)).ToList();

                var result = new List<AnalysisFriendData>();

                foreach (var analysisFriendDbModelList in analisisFriends)
                {
                    foreach (var analysisFriendDbModel in analysisFriendDbModelList)
                    {
                        var dbModel = analysisFriendDbModel;
                        var account = _context.Accounts.FirstOrDefault(model => model.Id == dbModel.AccountId);

                        if (account == null || account.GroupSettingsId == null)
                        {
                            continue;
                        }

                        var settingsModel =
                            new GetSettingsByGroupSettingsIdQueryHandler(_context).Handle(
                                new GetSettingsByGroupSettingsIdQuery
                                {
                                    GroupSettingsId = (long)account.GroupSettingsId
                                });

                        if (settingsModel == null)
                        {
                            continue;
                        }

                        if (settingsModel.GeoOptions.Cities == null && settingsModel.GeoOptions.Gender == null && settingsModel.GeoOptions.Countries == null) //replace only geo fields
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
