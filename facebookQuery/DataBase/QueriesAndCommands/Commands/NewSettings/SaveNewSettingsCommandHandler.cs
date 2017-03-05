using System;
using System.Web.Script.Serialization;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.NewSettings
{
    public class SaveNewSettingsCommandHandler : ICommandHandler<SaveNewSettingsCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public SaveNewSettingsCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public VoidCommandResponse Handle(SaveNewSettingsCommand command)
        {
            try
            {
                var jsSerializator = new JavaScriptSerializer();
                var communityOptionsJson = jsSerializator.Serialize(command.CommunityOptions);

                var newSettingsModel = new NewSettingsDbModel
                {
                    AccountId = command.AccountId,
                    SettingsGroupId = command.GroupId,
                    CommunityOptions = communityOptionsJson
                };

                _context.NewSettings.Add(newSettingsModel);
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
