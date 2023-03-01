﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using Microsoft.EntityFrameworkCore;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Job> Jobs { get; set; }

        public async ValueTask<Job> InsertJobAsync(Job job) =>
            await InsertAsync(job);
            
        public async ValueTask<Job> SelectJobByIdAsync(Guid jobId) =>
            await SelectAsync<Job>(jobId);
    }
}
