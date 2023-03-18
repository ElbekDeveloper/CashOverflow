// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
	public class JobDependancyException:Xeption
	{
		public JobDependancyException(Xeption innerException)
			:base("Job dependancy exception occured, contact support",innerException)
		{}
	}
}

