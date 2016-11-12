﻿using System.Collections.Generic;
using System.Linq;
using CommonModels;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestsHelpers;

namespace Engines.Engines.GetMessagesEngine.GetUnreadMessages
{
    public class GetUnreadMessagesEngine : AbstractEngine<GetUnreadMessagesModel, List<GetUnreadMessagesResponseModel>>
    {
        protected override List<GetUnreadMessagesResponseModel> ExecuteEngine(GetUnreadMessagesModel model)
        {
            var messagesList = new List<GetUnreadMessagesResponseModel>();

            if (model.UrlParameters == null) return null;

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie), "fb_dtsg");

            var parametersDictionary = model.UrlParameters.ToDictionary(pair => (GetUnreadMessagesEnum)pair.Key, pair => pair.Value);

            parametersDictionary[GetUnreadMessagesEnum.User] = model.AccountId.ToString("G");
            parametersDictionary[GetUnreadMessagesEnum.FbDtsg] = fbDtsg;

            var parameters = CreateParametersString(parametersDictionary);

            var stringResponse = RequestsHelper.Post(Urls.NewMessages.GetDiscription(), parameters, model.Cookie).Remove(0, 9);

            var data = (JObject)JsonConvert.DeserializeObject(stringResponse);
            var threads = data["payload"]["threads"];

            foreach (var thread in threads)
            {
                messagesList.Add(new GetUnreadMessagesResponseModel()
                {
                    FriendId = thread["thread_fbid"].Value<long>(),
                    CountAllMessages = thread["message_count"].Value<int>(),
                    CountUnreadMessages = thread["unread_count"].Value<int>(),
                    LastMessage = thread["snippet"].Value<string>(),
                    UnreadMessage = thread["unread_count"].Value<int>() != 0
                });
            }

            return messagesList;
        }

        private static string CreateParametersString(Dictionary<GetUnreadMessagesEnum, string> parameters)
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
