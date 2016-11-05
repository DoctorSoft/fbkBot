using Constants.MessageEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Messages;
using Services.ViewModels.OptionsModel;

namespace Services.Services
{
    public class MessageSettingService
    {
        public void SaveNewMessage(MessageViewModel model)
        {
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
    }
}
