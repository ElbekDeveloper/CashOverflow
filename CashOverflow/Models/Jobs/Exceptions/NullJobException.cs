// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
	public class NullJobException:Xeption
	{
		public NullJobException():base(message: "Job is null.")
		{
		}
	}
}

