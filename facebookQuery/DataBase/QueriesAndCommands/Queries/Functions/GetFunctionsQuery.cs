﻿using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.Functions
{
    public class GetFunctionsQuery : IQuery<List<FunctionData>>
    {
        public bool ForSpy { get; set; }
    }
}
