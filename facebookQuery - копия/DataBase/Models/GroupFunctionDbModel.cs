namespace DataBase.Models
{
    public class GroupFunctionDbModel
    {
        public long Id { get; set; }

        public long FunctionId { get; set; }

        public long GroupId { get; set; }

        public FunctionDbModel Function { get; set; }

        public GroupSettingsDbModel MessageGroup { get; set; }
    }
}
