// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
	public class LocationDependencyValidationException:Xeption
	{
		public LocationDependencyValidationException(Xeption innerException)
			: base(message: "Location dependency validation error occured,fix the errors and try again.", innerException)
		{ }
	}
}

