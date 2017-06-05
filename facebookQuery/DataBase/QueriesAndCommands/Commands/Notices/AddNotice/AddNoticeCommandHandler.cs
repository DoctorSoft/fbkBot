using System;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Notices.AddNotice
{
    public class AddNoticeCommandHandler : ICommandHandler<AddNoticeCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public AddNoticeCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(AddNoticeCommand command)
        {
            _context.Notices.Add(new NoticeDbModel
            {
                AccountId = command.AccountId,
                DateTime = DateTime.Now,
                NoticeText = command.NoticeText
            });

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
