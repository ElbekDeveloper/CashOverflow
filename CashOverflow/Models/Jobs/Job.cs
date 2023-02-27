// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------


using System;

namespace CashOverflow.Models.Jobs
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
