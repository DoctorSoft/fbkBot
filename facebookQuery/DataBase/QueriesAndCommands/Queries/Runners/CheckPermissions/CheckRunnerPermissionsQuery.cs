namespace DataBase.QueriesAndCommands.Queries.Runners.CheckPermissions
{
    public class CheckRunnerPermissionsQuery : IQuery<bool>
    {
        public long RunnerId{ get; set; }
    }
}
