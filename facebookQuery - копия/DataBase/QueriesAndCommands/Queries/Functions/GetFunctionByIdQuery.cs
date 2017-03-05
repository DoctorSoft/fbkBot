namespace DataBase.QueriesAndCommands.Queries.Functions
{
    public class GetFunctionByIdQuery : IQuery<FunctionData>
    {
        public long Id { get; set; }
    }
}
