using System;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.NewSettings
{
    public class DeleteNewSettingsCommandHandler : ICommandHandler<DeleteNewSettingsCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public DeleteNewSettingsCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public VoidCommandResponse Handle(DeleteNewSettingsCommand command)
        {
            try
            {
                var cummunities = _context.NewSettings.Where(model => model.AccountId == command.AccountId && model.SettingsGroupId == command.GroupId);

                _context.NewSettings.RemoveRange(cummunities);
                _context.SaveChanges();

                return new VoidCommandResponse();
            }
            catch (Exception ex)
            {
                return new VoidCommandResponse();
            }
        }
    }
}
