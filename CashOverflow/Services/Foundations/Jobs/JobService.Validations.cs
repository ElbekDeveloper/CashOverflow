// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;

namespace CashOverflow.Services.Foundations.Jobs
{
	public partial class JobService
	{
        private static void ValidateJobNotNull(Job job)
        {
            if (job is null)
            {
                throw new NullJobException();
            }
        }
    }
}

