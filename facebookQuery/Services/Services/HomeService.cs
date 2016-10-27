using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using DataBase.QueriesAndCommands.Queries.UrlParameters.Models;
using Engines.Engines.GetNewCookiesEngine;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class HomeService
    {
        public List<AccountViewModel> GetAccounts()
        {
            var accounts = new GetAccountsQueryHandler(new DataBaseContext()).Handle(new GetAccountsQuery
            {
                Count = 10,
                Page = 0
            });

            return accounts.Select(model => new AccountViewModel
            {
                Id = model.Id,
                PageUrl = model.PageUrl
            }).ToList();
        }


        public bool RefreshCookies(string login, string password)
        {
            new GetNewCookiesEngine().Execute(new GetNewCookiesModel()
            {
                Login = login,
                Password = password
            });


            return true;
        }

        public void SendMessage(long senderId, long recipientId, string messageText)
        {
            var rnd = new Random();
            var masssageId = "61946105663696201" + rnd.Next(10, 99);

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = 100013726390504
            });

            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.SendMessage
            }) as SendMessageUrlParametersModel;

            if (urlParameters == null) return;
            urlParameters.Body = messageText;
            urlParameters.MessageId = masssageId;
            urlParameters.OfflineThreadingId = masssageId;
            urlParameters.OtherUserFbid = recipientId.ToString("G");
            urlParameters.SpecificToListOne = recipientId.ToString("G");
            urlParameters.SpecificToListTwo = senderId.ToString("G");
            urlParameters.UserId = senderId.ToString("G");
            //urlParameters.Timestamp = Helper.DateTimeToJavaTimeStamp(DateTime.UtcNow).ToString();

            //var coockies = CreateCookieString(account.Cookie);
            var parameters = CreateParametersString(urlParameters);

            var url = "https://www.facebook.com/messaging/send/?dpr=1";
            var wb = new WebClient();
           // wb.Headers.Add(HttpRequestHeader.Cookie, coockies);
            wb.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wb.Headers[HttpRequestHeader.Accept] = "*/*";
            //wb.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate, br";
            wb.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            wb.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";

            wb.UploadString(url, parameters);
        }

        private static string CreateParametersString(SendMessageUrlParametersModel model)
        {
            return "&" +
                   model.Client + "&" +
                   model.ActionType + "&" +
                   model.Body + "&" +
                   model.EphemeralTtlMode + "&" +
                   model.HasAttachment + "&" +
                   model.MessageId + "&" +
                   model.OfflineThreadingId + "&" +
                   model.OtherUserFbid + "&" +
                   model.Source + "&" +
                   model.SignatureId + "&" +
                   model.SpecificToListOne + "&" +
                   model.SpecificToListTwo + "&" +
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


