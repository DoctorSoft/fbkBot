﻿using System;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobQueue.GetQueue
{
    public class JobQueueModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long? FriendId { get; set; }

        public bool IsProcessed { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime AddedDateTime { get; set; }
        
        public bool IsForSpy { get; set; }

        public string JobId { get; set; }

        public string LaunchDateTime { get; set; }
    }
}
