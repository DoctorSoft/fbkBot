using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using RequestsHelpers;

namespace Engines.Engines.RemoveFriendEngine
{
    public class RemoveFriendEngine : AbstractEngine<RemoveFriendModel, bool>
    {
        protected override bool ExecuteEngine(RemoveFriendModel model)
        {
            try
            {
                if (model.UrlParameters == null)
                {
                    return false;
                }
                
                var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy), "fb_dtsg");

                var parametersDictionary = model.UrlParameters.ToDictionary(pair => (RemoveFriendEnum)pair.Key, pair => pair.Value);

                parametersDictionary[RemoveFriendEnum.Uid] = model.FriendFacebookId.ToString();
                parametersDictionary[RemoveFriendEnum.User] = model.AccountFacebookId.ToString();
                parametersDictionary[RemoveFriendEnum.FbDtsg] = fbDtsg;

                var parameters = CreateParametersString(parametersDictionary);

                RequestsHelper.Post(Urls.RemoveFriend.GetDiscription(), parameters, model.Cookie, model.Proxy);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static string CreateParametersString(Dictionary<RemoveFriendEnum, string> parameters)
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
