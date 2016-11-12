using System.Collections.Generic;
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

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie), "fb_dtsg");

            var parametersDictionary = model.UrlParameters.ToDictionary(pair => (ChangeStatusForMesagesEnum)pair.Key, pair => pair.Value);

            parametersDictionary[ChangeStatusForMesagesEnum.Ids] = model.FriendId.ToString("G") + "]=true";
            parametersDictionary[ChangeStatusForMesagesEnum.User] = model.AccountId.ToString("G");
            parametersDictionary[ChangeStatusForMesagesEnum.FbDtsg] = fbDtsg;

            var parameters = CreateParametersString(parametersDictionary);

            RequestsHelper.Post(Urls.ChangeReadStatus.GetDiscription(), parameters, model.Cookie).Remove(0, 9);

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
