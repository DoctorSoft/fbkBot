using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Script.Serialization;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Settings
{
    public class AddOrUpdateSettingsCommandHandler : ICommandHandler<AddOrUpdateSettingsCommand, long>
    {
        private readonly DataBaseContext _context;

        public AddOrUpdateSettingsCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public long Handle(AddOrUpdateSettingsCommand command)
        {
            var groupSettings = _context.Settings.FirstOrDefault(model => model.Id == command.GroupId);
            var toUpdate = true;

            if (groupSettings == null)
            {
                groupSettings = new SettingsDbModel();
                toUpdate = false;
            }
                
            var jsSerializator = new JavaScriptSerializer();
            var geoOptionsJson = jsSerializator.Serialize(command.GeoOptions);
            var friendOptionsJson = jsSerializator.Serialize(command.FriendsOptions);
            var messageOptionsJson = jsSerializator.Serialize(command.MessageOptions);
            var limitsOptionsJson = jsSerializator.Serialize(command.LimitsOptions);


            groupSettings.Id = command.GroupId;

            groupSettings.GeoOptions = geoOptionsJson;
            groupSettings.FriendsOptions = friendOptionsJson;
            groupSettings.MessageOptions = messageOptionsJson;
            groupSettings.LimitsOptions = limitsOptionsJson;

            if (toUpdate)
            {
                _context.Settings.AddOrUpdate(groupSettings);
            }
            else
            {
                _context.Settings.Add(groupSettings);
            }

            _context.SaveChanges();

            return groupSettings.Id;
        }
    }
}
