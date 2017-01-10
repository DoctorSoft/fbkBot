using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Commands.SpyOptions
{
    public class SaveSpyFunctionsCommand : IVoidCommand
    {
        public long SpyId { get; set; }

        public List<long> Functions { get; set; } 
    }
}
