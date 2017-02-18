﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class JobQueueConfiguration : EntityTypeConfiguration<JobQueueDbModel>
    {
        public JobQueueConfiguration()
        {
            ToTable("JobsQueue");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.AccountId);
            Property(model => model.FunctionName);
            Property(model => model.AddedDateTime);
        }
    }
}
