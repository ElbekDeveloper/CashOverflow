using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class FailedJobServiceException: Xeption
    {
        public FailedJobServiceException(Exception innerException)
            : base(message: "Failed job service error occured, please contact support.", innerException)
        { }
    }
}