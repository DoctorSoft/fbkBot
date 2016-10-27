using System;
using Constants;
using Constants.EnumExtension;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using DataBase.QueriesAndCommands.Queries.UrlParameters.Models;
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
            }) as SendMessageUrlParametersModel;

            if (urlParameters == null) return null;
            var messageId = GenerateMessageId();

            urlParameters.Body = model.Message;
            urlParameters.MessageId = messageId;
            urlParameters.OfflineThreadingId = messageId;
            urlParameters.OtherUserFbid = model.FriendId.ToString("G");
            urlParameters.SpecificToListOne = model.FriendId.ToString("G");
            urlParameters.SpecificToListTwo = model.AccountId.ToString("G");
            urlParameters.UserId = model.AccountId.ToString("G");
            //urlParameters.Timestamp = Helper.DateTimeToJavaTimeStamp(DateTime.UtcNow).ToString();

            var parameters = CreateParametersString(urlParameters);


            var answer = RequestsHelper.Post(Urls.SendMessage.GetDiscription(), parameters, model.Cookie);

            return null;
        }


        private static string GenerateMessageId()
        {
            var rand = new Random();

            return "6197512797493" + rand.Next(100000, 999999);
        }

        private static string CreateParametersString(SendMessageUrlParametersModel model)
        {
            return model.Client + "&" +
                   model.ActionType + "&" +
                   "body=" +model.Body + "&" +
                   model.EphemeralTtlMode + "&" +
                   model.HasAttachment + "&" +
                   "message_id" + model.MessageId + "&" +
                   "offline_threading_id" + model.OfflineThreadingId + "&" +
                   "other_user_fbid" + model.OtherUserFbid + "&" +
                   model.Source + "&" +
                   model.SignatureId + "&" +
                   "specific_to_list[0]:fbid" + model.SpecificToListOne + "&" +
                   "specific_to_list[1]:fbid" + model.SpecificToListTwo + "&" +
                   model.Timestamp + "&" +
                   model.UiPushPhase + "&" +

                   model.UserId + "&" +
                   model.A + "&" +
                   model.Dyn + "&" +
                   model.Af + "&" +
                   model.Req + "&" +
                   model.Be + "&" +
                   model.Pc + "&" +
                   model.FbDtsg + "&" +
                   model.Ttstamp + "&" +
                   model.Rev + "&" +
                   model.SrpT;
        }
    }
}
