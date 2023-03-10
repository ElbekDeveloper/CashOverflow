// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
	public class LocationValidationException :Xeption
	{
		public LocationValidationException(Xeption innerException)
			:base(message: "Location validation error occured,fix the error and try again.",innerException)
		{}
	}
}

