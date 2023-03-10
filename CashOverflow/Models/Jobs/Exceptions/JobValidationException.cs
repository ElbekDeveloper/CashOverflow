// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class JobValidationException:Xeption  
    {
        public JobValidationException(Exception innerException) 
            :base(message:"", innerException)
        { }
    }
}
