// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
	public class NullLocationException:Xeption
	{
		public NullLocationException():base(message:"Location is null.")
		{}

	}
}

