using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.ExtraMessages;
using DataBase.QueriesAndCommands.Queries.ExtraMessages;
using Services.ViewModels.ExtraMessagesModel;

namespace Services.Services
{
    public class ExtraMessagesService
    {
        public ExtraMessageList GetExtraMessages()
        {
            var stopWords = new GetExtraMessagesQueryHandler(new DataBaseContext()).Handle(new GetExtraMessagesQuery());

            return new ExtraMessageList
            {
                ExtraMessages = stopWords.Select(data => new ExtraMessage
                {
                    Id = data.Id,
                    Message = data.Message
                }).ToList()
            };
        }
        public void AddNewExtraMessage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            new AddNewExtraMessagesCommandHandler(new DataBaseContext()).Handle(new AddNewExtraMessagesCommand
            {
                Name = name
            });
        }

        public void RemoveExtraMessage(long stopWordId)
        {
            new RemoveExtraMessageCommandHandler(new DataBaseContext()).Handle(new RemoveExtraMessageCommand
            {
                Id = stopWordId
            });
        }

        public void UpdateExtraMessage(long stopWordId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            new UpdateExtraMessageCommandHandler(new DataBaseContext()).Handle(new UpdateExtraMessageCommand
            {
                Name = name,
                Id = stopWordId
            });
        }
    }
}
