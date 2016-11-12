﻿using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestsHelpers;

namespace Engines.Engines.GetMessagesEngine.GetСorrespondenceByFriendId
{
    public class GetСorrespondenceByFriendIdEngine : AbstractEngine<GetСorrespondenceByFriendIdModel, List<GetСorrespondenceByFriendIdResponseModel>>
    {
        protected override List<GetСorrespondenceByFriendIdResponseModel> ExecuteEngine(GetСorrespondenceByFriendIdModel model)
        {
            var messagesList = new List<GetСorrespondenceByFriendIdResponseModel>();

            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.GetCorrespondence
            });

            if (urlParameters == null) return null;

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie), "fb_dtsg");

            var parametersDictionary = urlParameters.ToDictionary(pair => (GetCorrespondenceEnum)pair.Key, pair => pair.Value);

            parametersDictionary[GetCorrespondenceEnum.User] = model.AccountId.ToString();
            parametersDictionary[GetCorrespondenceEnum.FbDtsg] = fbDtsg;

            var parameters = CreateParametersString(parametersDictionary);

            var stringResponse = RequestsHelper.Post(Urls.GetСorrespondenceByFriendId.GetDiscription() + "&user_id" + model.FriendId, parameters, model.Cookie).Remove(0, 9);

            var data = (JObject)JsonConvert.DeserializeObject(stringResponse);
            var threads = data["payload"]["threads"];

            foreach (var thread in threads)
            {
                messagesList.Add(new GetСorrespondenceByFriendIdResponseModel()
                {
                });
            }

            return messagesList;
        }

        private static string CreateParametersString(Dictionary<GetCorrespondenceEnum, string> parameters)
        {
            var result = "";
            foreach (var parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Value))
                {
                    result += "&" + parameter.Key.GetAttributeName() + parameter.Value;
                }
            }

            return result.Remove(0, 1);
        }
    }
}
