using System;
using System.Collections.Generic;
using Constants.MessageEnums;

namespace DataBase.Models
{
    public class MessageDbModel
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public long ImportancyFactor { get; set; }

        public bool IsStopped { get; set; }

        public long? AccountId { get; set; }

        public long? MessageGroupId { get; set; }

        public MessageRegime MessageRegime { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int OrderNumber { get; set; }

        public bool IsEmergencyText { get; set; }

        public AccountDbModel Account { get; set; }

        public GroupSettingsDbModel GroupSettings { get; set; }
    }
}
