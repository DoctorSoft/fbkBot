using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using RequestsHelpers;

namespace Engines.Engines.ConfirmFriendshipEngine
{
    public class ConfirmFriendshipEngine : AbstractEngine<ConfirmFriendshipModel, bool>
    {
        protected override bool ExecuteEngine(ConfirmFriendshipModel model)
        {
            try
            {
                if (model.UrlParameters == null)
                {
                    return false;
                }
                
                var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent), "fb_dtsg");

                var parametersDictionary = model.UrlParameters.ToDictionary(pair => (ConfirmFriendshipEnum)pair.Key, pair => pair.Value);

                parametersDictionary[ConfirmFriendshipEnum.ViewerId] = model.AccountFacebookId.ToString();
                parametersDictionary[ConfirmFriendshipEnum.User] = model.AccountFacebookId.ToString();
                parametersDictionary[ConfirmFriendshipEnum.Id] = model.FriendFacebookId.ToString("G");
                parametersDictionary[ConfirmFriendshipEnum.FbDtsg] = fbDtsg;

                var parameters = CreateParametersString(parametersDictionary);

                RequestsHelper.Post(Urls.ConfirmFriendship.GetDiscription(), parameters, model.Cookie, model.Proxy, model.UserAgent);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static string CreateParametersString(Dictionary<ConfirmFriendshipEnum, string> parameters)
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
