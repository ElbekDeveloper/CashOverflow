using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class JobServiceException: Xeption
    {
        public JobServiceException(Exception innerException)
            : base(message: "Job service error occured, contact support.", innerException)    
        { }
    }
}