using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Groups.GroupSettings
{
    public class GetGroupSettingsNameByIdQueryHandler : IQueryHandler<GetGroupSettingsNameByIdQuery, string>
    {
        private readonly DataBaseContext _context;

        public GetGroupSettingsNameByIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public string Handle(GetGroupSettingsNameByIdQuery query)
        {
            const string groupIsNotSet = "Группа не выбрана";

            if (query.GroupId == null)
            {
                return groupIsNotSet;
            }
            var result = _context.GroupSettings.FirstOrDefault(model => model.Id == query.GroupId);

            if (result == null)
            {
                return groupIsNotSet;
            }

            return result.Name;
        }
    }
}
