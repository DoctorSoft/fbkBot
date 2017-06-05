using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Script.Serialization;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Models.JsonModels.AccountInformationModels;

namespace DataBase.QueriesAndCommands.Commands.AccountInformation
{
    public class AddAccountInformationCommandHandler : ICommandHandler<AddAccountInformationCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public AddAccountInformationCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(AddAccountInformationCommand command)
        {
            var accountInforamationModel = _context.AccountInformation.FirstOrDefault(model => model.Id == command.AccountId);
            var jsSerializator = new JavaScriptSerializer();

            if (accountInforamationModel == null)
            {
                var newAccountInformationJson = jsSerializator.Serialize(command.AccountInformationData);
                var dbModel = new AccountInformationDbModel
                {
                    Id = command.AccountId,
                    Information = newAccountInformationJson
                };

                _context.AccountInformation.AddOrUpdate(dbModel);
                _context.SaveChanges();
                return new VoidCommandResponse(); 
            }

            var friendOptionsModel = jsSerializator.Deserialize<AccountInformationDataDbModel>(accountInforamationModel.Information);

            friendOptionsModel.CountCurrentFriends = command.AccountInformationData.CountCurrentFriends != 0 ? command.AccountInformationData.CountCurrentFriends : friendOptionsModel.CountCurrentFriends;
            friendOptionsModel.CountIncommingFriendsRequest = command.AccountInformationData.CountIncommingFriendsRequest != 0 ? command.AccountInformationData.CountIncommingFriendsRequest : friendOptionsModel.CountIncommingFriendsRequest;
            friendOptionsModel.CountNewMessages = command.AccountInformationData.CountNewMessages != 0 ? command.AccountInformationData.CountNewMessages : friendOptionsModel.CountNewMessages;

            var accountInformationJson = jsSerializator.Serialize(friendOptionsModel);

            accountInforamationModel.Information = accountInformationJson;

            _context.AccountInformation.AddOrUpdate(accountInforamationModel);
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
