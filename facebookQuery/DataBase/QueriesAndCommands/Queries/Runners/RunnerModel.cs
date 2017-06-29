using System;

namespace DataBase.QueriesAndCommands.Queries.Runners
{
    public class RunnerModel
    {
        public long Id { get; set; }

        public string DeviceName { get; set; }

        public bool IsAllowed { get; set; }

        public DateTime LastAction { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
