using System.Linq;
using DataBase.Context;
using DataBase.Models;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;

namespace DataBase.QueriesAndCommands.Commands.Messages
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
                .Where(model => model.AccountId == null)
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

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
