using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using RequestsHelpers;

namespace Engines.Engines.SendMessageEngine
{
    public class SendMessageEngine : AbstractEngine<SendMessageModel, SendMessageResponseModel>
    {
        protected override SendMessageResponseModel ExecuteEngine(SendMessageModel model)
        {
            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.SendMessage
            });

            if (urlParameters == null) return null;
            var messageId = GenerateMessageId();

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie), "fb_dtsg");

            var parametersDictionary = urlParameters.ToDictionary(pair => (SendMessageEnum)pair.Key, pair => pair.Value);

            parametersDictionary[SendMessageEnum.Body] = model.Message;
            parametersDictionary[SendMessageEnum.MessageId] = messageId;
            parametersDictionary[SendMessageEnum.OfflineThreadingId] = messageId;
            parametersDictionary[SendMessageEnum.OtherUserFbid] = model.FriendId.ToString("G");
            parametersDictionary[SendMessageEnum.SpecificToListOne] = model.FriendId.ToString("G");
            parametersDictionary[SendMessageEnum.SpecificToListTwo] = model.AccountId.ToString("G");
            parametersDictionary[SendMessageEnum.UserId] = model.AccountId.ToString("G");
            parametersDictionary[SendMessageEnum.Timestamp] = DateTime.Now.Ticks.ToString();
            parametersDictionary[SendMessageEnum.FbDtsg] = fbDtsg;

            var parameters = CreateParametersString(parametersDictionary);

            var answer = RequestsHelper.Post(Urls.SendMessage.GetDiscription(), parameters, model.Cookie);

            return null;
        }


        private static string GenerateMessageId()
        {
            var rand = new Random();

            return "6197512797493" + rand.Next(100000, 999999);
        }

        private static string CreateParametersString(Dictionary<SendMessageEnum, string> parameters)
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
