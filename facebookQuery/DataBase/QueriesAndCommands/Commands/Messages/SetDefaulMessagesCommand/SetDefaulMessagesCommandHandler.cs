using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;

namespace DataBase.QueriesAndCommands.Commands.Messages.SetDefaulMessagesCommand
{
    public class SetDefaulMessagesCommandHandler : ICommandHandler<SetDefaulMessagesCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public SetDefaulMessagesCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(SetDefaulMessagesCommand command)
        {
            context.Set<MessageDbModel>().Where(model => model.AccountId == command.AccountId).Delete();

            var messagesToCopy = context.Set<MessageDbModel>()
                .Where(model => model.AccountId == null && command.GroupId == model.MessageGroupId)
                .ToList()
                .Select(model => new MessageDbModel
                {
                    ImportancyFactor = model.ImportancyFactor,
                    AccountId = command.AccountId,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    IsEmergencyText = model.IsEmergencyText,
                    IsStopped = model.IsStopped,
                    Message = model.Message,
                    MessageRegime = model.MessageRegime,
                    OrderNumber = model.OrderNumber
                }).ToList();

            context.BulkInsert(messagesToCopy);

            var userToConnect = context.Set<AccountDbModel>().FirstOrDefault(model => model.Id == command.AccountId && !model.IsDeleted);

            userToConnect.MessageGroupId = command.GroupId;

            context.Set<AccountDbModel>().AddOrUpdate(userToConnect);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
