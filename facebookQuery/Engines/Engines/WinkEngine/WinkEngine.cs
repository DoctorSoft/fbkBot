using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using RequestsHelpers;

namespace Engines.Engines.WinkEngine
{
    public class WinkEngine : AbstractEngine<WinkModel, bool>
    {
        protected override bool ExecuteEngine(WinkModel model)
        {
            try
            {
                if (model.UrlParameters == null)
                {
                    return false;
                }
                
                var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent), "fb_dtsg");

                var parametersDictionary = model.UrlParameters.ToDictionary(pair => (WinkEnum) pair.Key,pair => pair.Value);

                parametersDictionary[WinkEnum.PokeTarget] = model.FriendFacebookId.ToString("G");
                parametersDictionary[WinkEnum.User] = model.AccountFacebookId.ToString("G");
                parametersDictionary[WinkEnum.FbDtsg] = fbDtsg;

                var parameters = CreateParametersString(parametersDictionary);

                RequestsHelper.Post(Urls.Wink.GetDiscription() + "?poke_target=" + model.FriendFacebookId + "&dpr=1", parameters, model.Cookie, model.Proxy, model.UserAgent);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static string CreateParametersString(Dictionary<WinkEnum, string> parameters)
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
