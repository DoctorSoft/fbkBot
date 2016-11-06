using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Message
{
    public class GetMessageModelQueryHandler : IQueryHandler<GetMessageModelQuery, List<MessageModel>>
    {
        private readonly DataBaseContext context;

        public GetMessageModelQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<MessageModel> Handle(GetMessageModelQuery query)
        {
            var results = context.Set<MessageDbModel>()
                .Where(model => model.AccountId == query.AccountId)
                .Select(model => new MessageModel
                {
                    Message = model.Message,
                    Id = model.Id,
                    IsEmergencyText = model.IsEmergencyText,
                    OrderNumber = model.OrderNumber,
                    EndTime = model.EndTime,
                    StartTime = model.StartTime,
                    ImportancyFactor = model.ImportancyFactor,
                    MessageRegime = model.MessageRegime,
                    AccountId = model.AccountId,
                    IsStopped = model.IsStopped
                }).ToList();

            return results;
        }
    }
}
