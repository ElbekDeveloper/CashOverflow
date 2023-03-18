// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
	public class JobDependencyValidationException:Xeption
	{
		public JobDependencyValidationException(Xeption innerException)
			:base("Job dependency validation error occured,fix the errors and try again.", innerException)
		{ }
	}
}

