// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Services.Foundations.Jobs
{
    public interface IJobService
    {
        IQueryable<Job> RetrieveAllJobs();
    }
}
