// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------


using CashOverflow.Models.Locations;
using System;

namespace CashOverflow.Models.Salaries
{
    public class Salary
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public int Experience { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid LocationId { get; set; }
        public Guid LanguageId { get; set; }
        public Guid JobId { get; set; }
    }
}
