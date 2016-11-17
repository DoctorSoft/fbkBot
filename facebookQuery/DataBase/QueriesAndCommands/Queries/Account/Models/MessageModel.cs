﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constants.MessageEnums;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Queries.Account.Models
{
    public class MessageModel
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public long ImportancyFactor { get; set; }

        public bool IsStopped { get; set; }

        public long? AccountId { get; set; }

        public MessageRegime MessageRegime { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int OrderNumber { get; set; }

        public bool IsEmergencyText { get; set; }

    }
}