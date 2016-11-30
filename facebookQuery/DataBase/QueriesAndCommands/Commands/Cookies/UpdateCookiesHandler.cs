using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Cookies
{
    public class UpdateCookiesHandler: ICommandHandler<UpdateCookiesCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public UpdateCookiesHandler(DataBaseContext context)
        {
            this.context = context;
        }
        public VoidCommandResponse Handle(UpdateCookiesCommand command)
        {
            var cookie = context
            .Cookies
            .FirstOrDefault(m=>m.Account.FacebookId==command.AccountId);

            if (cookie == null)
            {
                cookie = new CookiesDbModel()
                {
                    CookiesString = command.NewCookieString,
                    CreateDate = DateTime.Now,
                    Id = command.AccountId
                };
            }
            else
            {
                cookie.CookiesString = command.NewCookieString;
                cookie.CreateDate = DateTime.Now;
            }
            
            context.Cookies.AddOrUpdate(cookie);
            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
