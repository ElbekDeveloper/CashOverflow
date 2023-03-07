using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class JobDependencyException: Xeption
    {
        public JobDependencyException(Xeption innerException)
            : base(message: "Job dependency error occured, contact support.", innerException)
        { }
    }
}