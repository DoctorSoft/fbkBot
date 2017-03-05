using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Commands.Groups
{
    public class SaveGroupFunctionsCommand : IVoidCommand
    {
        public long GroupId { get; set; }

        public List<long> Functions { get; set; } 
    }
}
