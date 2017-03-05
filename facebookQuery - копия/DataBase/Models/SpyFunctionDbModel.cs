namespace DataBase.Models
{
    public class SpyFunctionDbModel
    {
        public long Id { get; set; }

        public long FunctionId { get; set; }

        public long SpyId { get; set; }

        public FunctionDbModel Function { get; set; }

        public SpyAccountDbModel Spy { get; set; }
    }
}
