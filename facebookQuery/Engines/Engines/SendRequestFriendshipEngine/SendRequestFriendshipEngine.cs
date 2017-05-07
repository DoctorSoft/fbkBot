using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using RequestsHelpers;

namespace Engines.Engines.SendRequestFriendshipEngine
{
    public class SendRequestFriendshipEngine : AbstractEngine<SendRequestFriendshipModel, bool>
    {
        private static readonly Random Random = new Random();

        protected override bool ExecuteEngine(SendRequestFriendshipModel model)
        {
            try
            {
                if (model.AddFriendUrlParameters == null && model.AddFriendExtraUrlParameters == null)
                {
                    return false;
                }

                var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent), "fb_dtsg");

                var parametersAddFriendDictionary = model.AddFriendUrlParameters.ToDictionary(pair => (AddFriendEnum)pair.Key, pair => pair.Value);

                parametersAddFriendDictionary[AddFriendEnum.ToFriend] = model.FriendFacebookId.ToString("G");
                parametersAddFriendDictionary[AddFriendEnum.User] = model.AccountFacebookId.ToString();
                parametersAddFriendDictionary[AddFriendEnum.FbDtsg] = fbDtsg;

                var parametersAddFriendExtraDictionary = model.AddFriendExtraUrlParameters.ToDictionary(pair => (AddFriendExtraEnum)pair.Key, pair => pair.Value);

                parametersAddFriendExtraDictionary[AddFriendExtraEnum.User] = model.AccountFacebookId.ToString();
                parametersAddFriendExtraDictionary[AddFriendExtraEnum.FbDtsg] = fbDtsg;

                var addFriendParameters = CreateParametersString(parametersAddFriendDictionary);
                var addFriendExtraParameters = CreateParametersString(parametersAddFriendExtraDictionary);

                RequestsHelper.Post(Urls.AddFriend.GetDiscription(), addFriendParameters, model.Cookie, model.Proxy, model.UserAgent);

                RequestsHelper.Post(Urls.AddFriendExtra + "?friendid=" + model.FriendFacebookId + "&dpr=1", addFriendExtraParameters, model.Cookie, model.Proxy, model.UserAgent);
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static string CreateParametersString(Dictionary<AddFriendEnum, string> parameters)
        {
            var result = "";
            foreach (var parameter in parameters)
            {
                result += "&" + parameter.Key.GetAttributeName() + parameter.Value;
            }

            return result.Remove(0, 1);
        }

        private static string CreateParametersString(Dictionary<AddFriendExtraEnum, string> parameters)
        {
            var result = "";
            foreach (var parameter in parameters)
            {
                result += "&" + parameter.Key.GetAttributeName() + parameter.Value;
            }

            return result.Remove(0, 1);
        }

        private static string GenerateRandomValue()
        {
            //return "19629824" + _random.Next(10, 99);
            return "1" + Random.Next(1000, 9999).ToString("G") + Random.Next(10000, 99999).ToString("G");
        }
    }
}
