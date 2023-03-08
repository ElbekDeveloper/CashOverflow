// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;

namespace CashOverflow.Services.Foundations.Locations
{
	public partial class LocationService
	{
        private static void ValidateLocationNotNull(Location location)
        {
            if (location is null)
            {
                throw new NullLocationException();
            }
        }

    }
}

