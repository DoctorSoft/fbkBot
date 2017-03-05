using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestsHelpers;

namespace Engines.Engines.GetMessagesEngine.GetMessages
{
    public class GetMessagesEngine : AbstractEngine<GetMessagesModel, List<GetMessagesResponseModel>>
    {
        protected override List<GetMessagesResponseModel> ExecuteEngine(GetMessagesModel model)
        {
            var messagesList = new List<GetMessagesResponseModel>();

            if (model.UrlParameters == null) return null;

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy), "fb_dtsg");

            var parametersDictionary = model.UrlParameters.ToDictionary(pair => (GetMessagesEnum)pair.Key, pair => pair.Value);

            parametersDictionary[GetMessagesEnum.User] = model.AccountId.ToString();
            parametersDictionary[GetMessagesEnum.FbDtsg] = fbDtsg;

            var parameters = CreateParametersString(parametersDictionary);

            var stringResponse = RequestsHelper.Post(Urls.NewMessages.GetDiscription(), parameters, model.Cookie, model.Proxy).Remove(0, 9);

            var data = (JObject)JsonConvert.DeserializeObject(stringResponse);
            var threads = data["payload"]["threads"];

            foreach (var thread in threads)
            {
                messagesList.Add(new GetMessagesResponseModel()
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

        private static string CreateParametersString(Dictionary<GetMessagesEnum, string> parameters)
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
