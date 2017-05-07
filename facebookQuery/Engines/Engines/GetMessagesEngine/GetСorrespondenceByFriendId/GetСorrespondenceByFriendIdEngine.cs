using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
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

            if (model.UrlParameters == null) return null;

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent), "fb_dtsg");

            var parametersDictionary = model.UrlParameters.ToDictionary(pair => (GetCorrespondenceEnum)pair.Key, pair => pair.Value);

            parametersDictionary[GetCorrespondenceEnum.User] = model.AccountFacebookId.ToString();
            parametersDictionary[GetCorrespondenceEnum.FbDtsg] = fbDtsg;

            var parameters = CreateParametersString(parametersDictionary);

            var stringResponse = RequestsHelper.Post(Urls.GetСorrespondenceByFriendId.GetDiscription() + "?user_id=" + model.FriendId + "&dpr=1", parameters, model.Cookie, model.Proxy, model.UserAgent).Remove(0, 9);

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
