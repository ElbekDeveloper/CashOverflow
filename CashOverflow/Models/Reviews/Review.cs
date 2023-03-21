// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;

namespace CashOverflow.Models.Reviews
{
    public class Review
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public int Stars { get; set; }
        public string Thoughts { get; set; }
    }
}
