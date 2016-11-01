using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Script.Serialization;
using DataBase.Constants;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using DataBase.QueriesAndCommands.Queries.UrlParameters.Models;

namespace DataBase.Migrations
{
    internal sealed class DataBaseConfiguration : DbMigrationsConfiguration<DataBaseContext>
    {
        public DataBaseConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(DataBaseContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Launch();
            }

            var accountsList = new List<AccountDbModel>()
            {
                /*new AccountDbModel()
                {
                    Id = 1,
                    Login = "ms.nastasia.1983@mail.ru",
                    Password = "Ntvyjnf123",
                    PageUrl = "https://www.facebook.com/profile.php?id=100013726390504",
                    UserId = 100013726390504,
                    Cookies = new CookiesDbModel()
                    {
                        CookiesString = "",
                        CreateDate = DateTime.Now
                    }
                },*/
                new AccountDbModel()
                {
                    Id = 1,
                    Login = "petya-pervyy-1999@mail.ru",
                    Password = "nWE#w(Qb",
                    PageUrl = "https://www.facebook.com/profile.php?id=100013532889680",
                    UserId = 100013532889680,
                    Cookies = new CookiesDbModel()
                    {
                        CookiesString = "",
                        CreateDate = DateTime.Now
                    }
                },
            };

            
            context.Accounts.AddRange(accountsList);

            /*
            var parameters = new Dictionary<SendMessageEnum, string>
            {
                {SendMessageEnum.Client, "mercury"},
                {SendMessageEnum.ActionType, "ma-type%3Auser-generated-message"},
                {SendMessageEnum.Body, ""},
                {SendMessageEnum.EphemeralTtlMode, "0"},
                {SendMessageEnum.HasAttachment, "false"},
                {SendMessageEnum.MessageId, ""},
                {SendMessageEnum.OfflineThreadingId, ""},
                {SendMessageEnum.OtherUserFbid, ""},
                {SendMessageEnum.Source, "source%3Achat%3Aweb"},
                {SendMessageEnum.SignatureId, "6ec32383"},
                {SendMessageEnum.SpecificToListOne, ""},
                {SendMessageEnum.SpecificToListTwo, ""},
                {SendMessageEnum.Timestamp, "1477233712253"},
                {SendMessageEnum.UiPushPhase, "V3"},
                {SendMessageEnum.UserId, ""},
                {SendMessageEnum.A, "1"},
                {
                    SendMessageEnum.Dyn,
                    "aihoFeyfyGmagngDxyG9giolzkqbxqbAKGiBAy8Z9LFwxBxvyui9wWhE98nwgUy22Ea-dhUKbkwy8xa5ZKex3BKuEjKeCxPG4GDg4ium4UpKq4GCzk58nVV8-cxnxm3i2y9ADBy8K48hxGbwBxqu49LZ1uJ12VqxOEqCV8F3qzE"
                },
                {SendMessageEnum.Af, "o"},
                {SendMessageEnum.Req, "co"},
                {SendMessageEnum.Be, "-1"},
                {SendMessageEnum.Pc, "PHASED%3ADEFAULT"},
                {SendMessageEnum.FbDtsg, "AQEX6WS-FFC3%3AAQFwo5hs9ISS"},
                {SendMessageEnum.Ttstamp, "2658169885487834570706751586581701191115310411557738383"},
                {SendMessageEnum.Rev, "2638327"},
                {SendMessageEnum.SrpT, "1477219353"}
            };
           
            var parametersUnread = new Dictionary<GetUnreadMessagesEnum, string>
            {
                {GetUnreadMessagesEnum.Client, "web_messenger"},
                {GetUnreadMessagesEnum.InboxOffset, "0"},
                {GetUnreadMessagesEnum.InboxFilter, "unread"},
                {GetUnreadMessagesEnum.InboxLimit, "1000"},
                {GetUnreadMessagesEnum.User, ""},
                {GetUnreadMessagesEnum.A, "1"},
                {GetUnreadMessagesEnum.Be, "-1"},
                {GetUnreadMessagesEnum.Pc, "PHASED:DEFAULT"},
                {GetUnreadMessagesEnum.FbDtsg, ""}
            };

            var js = new JavaScriptSerializer();
            var jsonUnread = js.Serialize(parametersUnread.Select(pair => pair).ToList());

            var urlParametersList = new List<UrlParametersDbModel>()
            {
                new UrlParametersDbModel
                {
                    CodeParameters = (int)NamesUrlParameter.GetMessages,
                    ParametersSet = jsonUnread
                }
            };

            context.UrlParameters.AddRange(urlParametersList);
             */
            context.SaveChanges();
        }
    }
}
