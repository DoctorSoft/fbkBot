﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using RequestsHelpers;

namespace Engines.Engines.GetMessagesEngine.ChangeMessageStatus
{
    public class ChangeMessageStatusEngine : AbstractEngine<ChangeMessageStatusModel, VoidModel>
    {
        protected override VoidModel ExecuteEngine(ChangeMessageStatusModel model)
        {
            if (model.UrlParameters == null) return null;

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent), "fb_dtsg");

            var parametersDictionary = model.UrlParameters.ToDictionary(pair => (ChangeStatusForMesagesEnum)pair.Key, pair => pair.Value);

            parametersDictionary[ChangeStatusForMesagesEnum.Ids] = model.FriendFacebookId.ToString("G") + "]=true";
            parametersDictionary[ChangeStatusForMesagesEnum.User] = model.AccountId.ToString(CultureInfo.InvariantCulture);
            parametersDictionary[ChangeStatusForMesagesEnum.FbDtsg] = fbDtsg;
            parametersDictionary[ChangeStatusForMesagesEnum.Ttstamp] = "2658169757012152707310256495865817278110491018710365111116";

            var parameters = CreateParametersString(parametersDictionary);

            RequestsHelper.Post(Urls.ChangeReadStatus.GetDiscription(), parameters, model.Cookie, model.Proxy, model.UserAgent).Remove(0, 9);

            return new VoidModel();
        }

        private static string CreateParametersString(Dictionary<ChangeStatusForMesagesEnum, string> parameters)
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
