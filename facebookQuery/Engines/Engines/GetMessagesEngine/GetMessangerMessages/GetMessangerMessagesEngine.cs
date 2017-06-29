using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Constants;
using Constants.EnumExtension;
using Constants.GendersUnums;
using Constants.UrlEnums;
using Engines.Engines.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestsHelpers;

namespace Engines.Engines.GetMessagesEngine.GetMessangerMessages
{
    public class GetMessangerMessagesEngine : AbstractEngine<GetMessangerMessagesModel, List<FacebookMessageModel>>
    {
        protected override List<FacebookMessageModel> ExecuteEngine(GetMessangerMessagesModel model)
        {
            var messagesList = new List<FacebookMessageModel>();

            if (model.UrlParameters == null) return null;

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent), "fb_dtsg");

            var parametersDictionary = model.UrlParameters.ToDictionary(pair => (GetUnreadMessagesEnum)pair.Key, pair => pair.Value);

            parametersDictionary[GetUnreadMessagesEnum.User] = model.AccountId.ToString(CultureInfo.InvariantCulture);
            parametersDictionary[GetUnreadMessagesEnum.FbDtsg] = fbDtsg;

            var parameters = CreateParametersString(parametersDictionary);

            var stringResponse = RequestsHelper.Post(Urls.NewMessages.GetDiscription(), parameters, model.Cookie, model.Proxy, model.UserAgent).Remove(0, 9);

            var data = (JObject)JsonConvert.DeserializeObject(stringResponse);
            
            var threads = data["payload"]["threads"];
            var threads2 = data["payload"]["participants"];

            int count = 0;
            foreach (var thread in threads)
            {
                if (count >= model.NumbersOfDialogues)
                {
                    break;
                }

                var countMessages = thread["message_count"].Value<long>();
                var countUnreadMessages = thread["unread_count"].Value<long>();
                var text = thread["snippet"].Value<string>();

                if (countMessages == 1 && countUnreadMessages == 0 && text.Equals(string.Empty))
                {

                    var facebookId = thread["thread_fbid"].Value<long>();
                    JObject friendThread2 = null;

                    foreach (var thread2 in threads2)
                    {
                        if (thread2["fbid"].Value<long>().Equals(facebookId))
                        {
                            friendThread2 = (JObject) thread2;
                        }
                    }

                    messagesList.Add(new FacebookMessageModel
                    {
                        AccountId = model.AccountId,
                        FriendFacebookId = facebookId,
                        CountAllMessages = thread["message_count"].Value<int>(),
                        CountUnreadMessages = thread["unread_count"].Value<int>(),
                        Gender = friendThread2["gender"].Value<int>() == 1 ? GenderEnum.Female : GenderEnum.Male,
                        Href = friendThread2["href"].Value<string>(),
                        Name = friendThread2["name"].Value<string>(),
                        LastMessage = thread["snippet"].Value<string>(),
                        LastUnreadMessageDateTime =
                            GetDateTime(Convert.ToInt64(thread["last_message_timestamp"].Value<string>())),
                        UnreadMessage = thread["unread_count"].Value<int>() != 0
                    });
                    count++;
                }
            }

            return messagesList;
        }

        private static DateTime GetDateTime(long timeStamp)
        {
            var newDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(timeStamp / 1000d)).ToLocalTime();
            return newDateTime;
        }
        private static string ConvertToUTF8(string source)
        {
            var utfBytes = Encoding.UTF8.GetBytes(source);
            var koi8RBytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("windows-1251"), utfBytes);
            return Encoding.GetEncoding("utf-8").GetString(koi8RBytes);
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
