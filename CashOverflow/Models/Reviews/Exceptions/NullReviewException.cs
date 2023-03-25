// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
	public class NullReviewException:Xeption
	{
		public NullReviewException()
			: base(message: "Review is null.")
		{ }
	}
}

