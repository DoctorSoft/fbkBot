using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Messages
{
    public class SaveNewMessageCommandHandler : ICommandHandler<SaveNewMessageCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public SaveNewMessageCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(SaveNewMessageCommand command)
        {
            var model = new MessageDbModel
            {
                AccountId = command.AccountId,
                IsStopped = false,
                EndTime = command.EndTime,
                ImportancyFactor = command.ImportancyFactor,
                IsEmergencyText = command.IsEmergencyText,
                Message = command.Message,
                MessageRegime = command.MessageRegime,
                OrderNumber = command.OrderNumber,
                StartTime = command.StartTime
            };

            context.Set<MessageDbModel>().Add(model);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
