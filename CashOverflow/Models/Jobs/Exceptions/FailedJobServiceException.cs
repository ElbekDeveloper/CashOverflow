// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
	public class FailedJobServiceException:Xeption
	{
		public FailedJobServiceException(Exception innerException)
            : base(message: "Failed job service error occured,contact support.", innerException)
        {}
	}
}

