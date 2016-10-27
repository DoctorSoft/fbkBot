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
            /*if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Launch();
            }*/

            var accountsList = new List<AccountDbModel>()
            {
                new AccountDbModel()
                {
                    Id = 1,
                    PageUrl = "https://www.facebook.com/profile.php?id=100013726390504",
                    UserId = 100013726390504,
                    Cookies = new CookiesDbModel()
                    {
                        CookiesString = "",
                        CreateDate = DateTime.Now
                    }
                }
            };

            context.Accounts.AddRange(accountsList);

            var parameters = new SendMessageUrlParametersModel()
            {       
                Client = "mercury",
                ActionType = "ma-type%3Auser-generated-message",
                Body="",
                EphemeralTtlMode = "0",
                HasAttachment = "false",
                MessageId = "",
                OfflineThreadingId = "",
                OtherUserFbid = "",
                Source = "source%3Achat%3Aweb",
                SignatureId = "6ec32383",
                SpecificToListOne = "",
                SpecificToListTwo = "",
                Timestamp = "1477233712253",
                UiPushPhase="V3",
                
                UserId = "",
                A = "1",
                Dyn = "aihoFeyfyGmagngDxyG9giolzkqbxqbAKGiBAy8Z9LFwxBxvyui9wWhE98nwgUy22Ea-dhUKbkwy8xa5ZKex3BKuEjKeCxPG4GDg4ium4UpKq4GCzk58nVV8-cxnxm3i2y9ADBy8K48hxGbwBxqu49LZ1uJ12VqxOEqCV8F3qzE",
                Af = "o",
                Req = "co",
                Be = "-1",
                Pc = "PHASED%3ADEFAULT",
                FbDtsg = "AQEX6WS-FFC3%3AAQFwo5hs9ISS",
                Ttstamp = "2658169885487834570706751586581701191115310411557738383",
                Rev = "2638327",
                SrpT = "1477219353"
            };

            var js = new JavaScriptSerializer();
            var json = js.Serialize(parameters);

            var urlParametersList = new List<UrlParametersDbModel>()
            {
                new UrlParametersDbModel
                {
                    CodeParameters = (int)NamesUrlParameter.SendMessage,
                    ParametersSet = json
                }
            };

            context.UrlParameters.AddRange(urlParametersList);

            context.SaveChanges();
        }
    }
}
