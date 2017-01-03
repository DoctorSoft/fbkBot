using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Cookies
{
    public class UpdateCookiesForSpyHandler: ICommandHandler<UpdateCookiesForSpyCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public UpdateCookiesForSpyHandler(DataBaseContext context)
        {
            this.context = context;
        }
        public VoidCommandResponse Handle(UpdateCookiesForSpyCommand command)
        {
            var cookie = context
            .CookiesForSpy
            .FirstOrDefault(m => m.SpyAccount.Id == command.AccountId);

            if (cookie == null)
            {
                cookie = new CookiesForSpyDbModel
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
            
            context.CookiesForSpy.AddOrUpdate(cookie);
            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
