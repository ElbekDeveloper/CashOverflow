// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;

namespace CashOverflow.Models.Languages {
    public class Language {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
