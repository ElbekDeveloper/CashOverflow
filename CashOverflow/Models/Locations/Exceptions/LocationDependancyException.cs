// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
	public class LocationDependancyException:Xeption
	{
		public LocationDependancyException(Xeption innerException)
			:base("Location dependancy exception occured, contact support",innerException)
		{
		}
	}
}

