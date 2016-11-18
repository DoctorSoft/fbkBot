using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Commands.StopWords;
using DataBase.QueriesAndCommands.Queries.Groups;
using DataBase.QueriesAndCommands.Queries.StopWords;
using Services.ViewModels.GroupModels;
using Services.ViewModels.StopWordsModels;

namespace Services.Services
{
    public class StopWordsService
    {
        public StopWordList GetStopWords()
        {
            var stopWords = new GetStopWordsQueryHandler(new DataBaseContext()).Handle(new GetStopWordsQuery());

            return new StopWordList
            {
                StopWords = stopWords.Select(data => new StopWord
                {
                    Id = data.Id,
                    Name = data.Name
                }).ToList()
            };
        }

        public void AddNewStopWord(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            new AddNewStopWordCommandHandler(new DataBaseContext()).Handle(new AddNewStopWordCommand
            {
                Name = name
            });
        }

        public void RemoveStopWord(long stopWordId)
        {
            new RemoveStopWordCommandHandler(new DataBaseContext()).Handle(new RemoveStopWordCommand
            {
                Id = stopWordId
            });
        }

        public void UpdateStopWord(long stopWordId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            new UpdateStopWordCommandHandler(new DataBaseContext()).Handle(new UpdateStopWordCommand
            {
                Name = name,
                Id = stopWordId
            });
        }
    }
}
