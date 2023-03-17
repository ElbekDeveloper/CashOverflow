// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class InvalidLocationException : Xeption
    {
        public InvalidLocationException()
            : base(message: "Location is invalid.")
        { }
    }
}
