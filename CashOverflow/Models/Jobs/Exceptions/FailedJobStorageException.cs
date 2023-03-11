using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class FailedJobStorageException: Xeption
    {
        public FailedJobStorageException(Exception innerException)
            : base(message: "Failed user storage error occurred, contact support.", innerException)
        { }
    }
}