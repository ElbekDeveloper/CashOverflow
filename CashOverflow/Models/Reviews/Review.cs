// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Companies;

namespace CashOverflow.Models.Reviews
{
    public class Review
    {
        public Guid Id { get; set; }
        public int Stars { get; set; }
        public string Thoughts { get; set; }

        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
