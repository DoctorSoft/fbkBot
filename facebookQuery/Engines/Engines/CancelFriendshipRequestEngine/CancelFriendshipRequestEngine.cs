using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using RequestsHelpers;

namespace Engines.Engines.CancelFriendshipRequestEngine
{
    public class CancelFriendshipRequestEngine : AbstractEngine<CancelFriendshipRequestModel, bool>
    {
        protected override bool ExecuteEngine(CancelFriendshipRequestModel model)
        {
            try
            {
                if (model.UrlParameters == null)
                {
                    return false;
                }
                
                var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy), "fb_dtsg");

                var parametersDictionary = model.UrlParameters.ToDictionary(pair => (CancelFriendshipRequestEnum)pair.Key, pair => pair.Value);

                parametersDictionary[CancelFriendshipRequestEnum.Confirm] = model.FriendFacebookId.ToString("G");
                parametersDictionary[CancelFriendshipRequestEnum.RequestId] = model.FriendFacebookId.ToString("G");
                parametersDictionary[CancelFriendshipRequestEnum.ListItemId] = string.Format("{0}_1_req", model.FriendFacebookId.ToString("G"));
                parametersDictionary[CancelFriendshipRequestEnum.StatusDivId] = string.Format("{0}_1_req_status", model.FriendFacebookId.ToString("G"));
                parametersDictionary[CancelFriendshipRequestEnum.User] = model.AccountFacebookId.ToString("G");
                parametersDictionary[CancelFriendshipRequestEnum.FbDtsg] = fbDtsg;

                var parameters = CreateParametersString(parametersDictionary);

                RequestsHelper.Post(Urls.CancelFriendshipRequest.GetDiscription(), parameters, model.Cookie, model.Proxy);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static string CreateParametersString(IEnumerable<KeyValuePair<CancelFriendshipRequestEnum, string>> parameters)
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
