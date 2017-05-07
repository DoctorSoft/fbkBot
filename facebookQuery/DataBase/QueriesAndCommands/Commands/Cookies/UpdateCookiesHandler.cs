using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Cookies
{
    public class UpdateCookiesHandler: ICommandHandler<UpdateCookiesCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public UpdateCookiesHandler(DataBaseContext context)
        {
            this._context = context;
        }
        public VoidCommandResponse Handle(UpdateCookiesCommand command)
        {
            var cookie = _context
            .Cookies
            .FirstOrDefault(m => m.Account.Id == command.AccountId);

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
            
            _context.Cookies.AddOrUpdate(cookie);
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
