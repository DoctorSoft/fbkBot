using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.Groups
{
    public class GroupFunctionData
    {
        public long FunctionId { get; set; }

        public FunctionName FunctionName { get; set; }

        public string Name { get; set; }

        public long GroupId { get; set; }

        public string GroupName { get; set; }
    }
}
