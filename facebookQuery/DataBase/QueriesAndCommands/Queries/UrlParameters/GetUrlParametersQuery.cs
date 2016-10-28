﻿using System.Collections.Generic;
using Constants.UrlUnums;
using DataBase.Constants;

namespace DataBase.QueriesAndCommands.Queries.UrlParameters
{
    public class GetUrlParametersQuery : IQuery<List<KeyValue<int, string>>>
    {
        public NamesUrlParameter NameUrlParameter { get; set; }
    }
}
