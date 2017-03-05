using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.Functions
{
    public class FunctionData
    {
        public long FunctionId { get; set; }

        public FunctionName FunctionName { get; set; }

        public string Name { get; set; }

        public FunctionTypeName FunctionTypeName { get; set; }

        public string TypeName { get; set; }

        public bool ForSpy { get; set; }
    }
}
