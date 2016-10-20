using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.Migrations
{
    internal sealed class DataBaseConfiguration : DbMigrationsConfiguration<DataBaseContext>
    {
        public DataBaseConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(DataBaseContext context)
        {
            var cookiesList = new List<CookiesDbModel>()
            {
                new CookiesDbModel()
                {
                    Id = 1,
                    AccountId = 1,
                    Locale = "ru_RU",
                    Av="1",
                    Datr="rREdV7eqIbgh25Gal6Q9J9ZC",
                    Sb="ZA1EV8pIHILc8m8MpfzyOHHA",
                    CUser="100013726390504",
                    Xs="40%3AGL9vgtMygUyU1A%3A2%3A1476897375%3A-1",
                    Fr="0almW4fUXZ8jobYqn.AWXxr3ej9Ks5rFkbBXaeflZEmpg.BXRA1l.uM.AAA.0.0.BYB6pe.AWWl-J9f",
                    Csm="2",
                    S="Aa4JxX8cIBNkkbrq.BYB6pf",
                    Pl="n",
                    Lu="ggAF8SUUdyIdnGp-mB0GMomA",
                    P="-2",
                    Act="1476897381177%2F1",
                    Wd="1920x974",
                    Presence="EDvF3EtimeF1476897483EuserFA21B13726390504A2EstateFDt2F_5bDiFA2user_3a223500233A2ErF1C_5dElm2FA2user_3a223500233A2Euct2F1476896775007EtrFnullEtwF2996746860EatF1476897483907G476897483930CEchFDp_5f1B13726390504F2CC"
                }
            };
            var accountsList = new List<AccountDbModel>()
            {
                new AccountDbModel()
                {
                    Id = 1,
                    CookieId = 1,
                    PageUrl = "https://www.facebook.com/profile.php?id=100013726390504"
                }
            };

            context.Cookies.RemoveRange(context.Cookies);
            context.Cookies.AddRange(cookiesList);

            context.Accounts.RemoveRange(context.Accounts);
            context.Accounts.AddRange(accountsList);
        }
    }
}
