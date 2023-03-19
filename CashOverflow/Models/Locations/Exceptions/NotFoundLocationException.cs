// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class NotFoundLocationException : Xeption
    {
        public NotFoundLocationException(Guid locationId)
            : base(message: $"Couldn't find location with id: {locationId}.")
        { }
    }
}