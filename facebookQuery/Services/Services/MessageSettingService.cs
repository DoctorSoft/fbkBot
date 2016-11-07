using System.Linq;
using Constants.MessageEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Messages;
using DataBase.QueriesAndCommands.Queries.Message;
using Services.ViewModels.OptionsModel;

namespace Services.Services
{
    public class MessageSettingService
    {
        public void SaveNewMessage(MessageViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Message))
            {
                return;
            }

            if (model.StartTime == model.EndTime || model.StartTime == null || model.EndTime == null)
            {
                model.StartTime = null;
                model.EndTime = null;
            }

            if (model.OrderNumber < 1)
            {
                model.OrderNumber = 1;
            }

            if (model.ImportancyFactor < 1)
            {
                model.ImportancyFactor = 1;
            }

            if (model.ImportancyFactor > 100)
            {
                model.ImportancyFactor = 100;
            }

            new SaveNewMessageCommandHandler(new DataBaseContext()).Handle(new SaveNewMessageCommand
            {
                AccountId = model.AccountId,
                OrderNumber = model.OrderNumber,
                EndTime = model.EndTime,
                StartTime = model.StartTime,
                Message = model.Message,
                ImportancyFactor = model.ImportancyFactor,
                IsEmergencyText = model.IsEmergencyText,
                MessageRegime = model.IsBotFirst ? MessageRegime.BotFirstMessage : MessageRegime.UserFirstMessage 
            });
        }

        public MessageListModel GetMessagesList(long? accountId)
        {
            var messages = new GetMessageModelQueryHandler(new DataBaseContext()).Handle(new GetMessageModelQuery
            {
                AccountId = accountId
            });

            var result = new MessageListModel
            {
                Messages = messages.Select(model => new MessageListItemModel
                {
                    Message = model.Message,
                    OrderNumber = model.OrderNumber,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    IsEmergencyText = model.IsEmergencyText,
                    ImportancyFactor = model.ImportancyFactor,
                    IsBotFirst = model.MessageRegime == MessageRegime.BotFirstMessage,
                    Id = model.Id
                }).ToList(),
                AccountId = accountId
            };

            return result;
        }

        public void RemoveMessage(long messageId)
        {
            new RemoveMessageCommandHandler(new DataBaseContext()).Handle(new RemoveMessageCommand
            {
                MessageId = messageId
            });
        }

        public void SetDefaulMessages(long accountId)
        {
            new SetDefaulMessagesCommandHandler(new DataBaseContext()).Handle(new SetDefaulMessagesCommand
            {
                AccountId = accountId
            });
        }
    }
}
