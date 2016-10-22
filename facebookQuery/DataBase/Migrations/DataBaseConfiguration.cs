using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Script.Serialization;
using DataBase.Constants;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Queries.UrlParameters;

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
                new AccountDbModel()
                {
                    Id = 1,
                    PageUrl = "https://www.facebook.com/profile.php?id=100013726390504",
                }
            };

            context.Accounts.AddRange(accountsList);

            var cookieList = new List<CookiesDbModel>()
            {
                new CookiesDbModel()
                {
                    Id = 1,
                    Locale = "ru_RU",
                    Av = "1",
                    Datr = "rREdV7eqIbgh25Gal6Q9J9ZC",
                    Sb = "ZA1EV8pIHILc8m8MpfzyOHHA",
                    CUser = "100013726390504",
                    Xs = "40%3AGL9vgtMygUyU1A%3A2%3A1476897375%3A-1",
                    Fr = "0almW4fUXZ8jobYqn.AWXxr3ej9Ks5rFkbBXaeflZEmpg.BXRA1l.uM.AAA.0.0.BYB6pe.AWWl-J9f",
                    Csm = "2",
                    S = "Aa4JxX8cIBNkkbrq.BYB6pf",
                    Pl = "n",
                    Lu = "ggAF8SUUdyIdnGp-mB0GMomA",
                    P = "-2",
                    Act = "1476897381177%2F1",
                    Wd = "1920x974",
                    Presence =
                        "EDvF3EtimeF1476897483EuserFA21B13726390504A2EstateFDt2F_5bDiFA2user_3a223500233A2ErF1C_5dElm2FA2user_3a223500233A2Euct2F1476896775007EtrFnullEtwF2996746860EatF1476897483907G476897483930CEchFDp_5f1B13726390504F2CC"
                }
            };

            context.Cookies.AddRange(cookieList);

            var parameters = new SendMessageUrlParametersModel()
            {
                Client = "mercury",
                ActionType = "ma-type:user-generated-message",
                Body="",
                EphemeralTtlMode = "0",
                HasAttachment = "false",
                MessageId = "",
                OtherUserFbid = "",
                Source = "source:titan:web",
                SignatureId = "6f66bfb5", 
                SpecificToListOne = "",
                SpecificToListTwo = "",
                Timestamp = "1476818665788",
                UiPushPhase="V3",
                
                UserId = "",
                A = "1",
                Dyn = "aihoFeyfyGmagngDxyG8EigmzFEbFbGA8Ay8Z9LFwxBxCbzEeAq2i5U4e2CEaUgxebkwy8wGFeex2uVWxeUWq264EK14DBwJKq4GCzEkxvDAzUO5u5o5S9ADBy8K48hxGbwYDx2r_xLggKm7U9eiax6ew",
                Af = "-1",
                Req = "x",
                Be = "-1",
                Pc = "PHASED:DEFAULT",
                FbDtsg = "AQHXI-PMhJk_:AQEgGrp3bo1j",
                Ttstamp = "2658172887345807710474107955865816910371114112519811149106",
                Rev = "2631735",
                SrpT = "1476897375"
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
