<<<<<<< HEAD
using System;
using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class CompanyDependencyException : Xeption
    {
        public CompanyDependencyException(Exception innerException)
            : base(message: "Company dependency error occurred, contact support.", innerException)
        { }
    }
}
=======
﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions;

public class CompanyDependencyException : Xeption
{
    public CompanyDependencyException(Exception innerException)
        : base(message: "Company dependency error occurred, contact support.", innerException)
    { }
}
>>>>>>> master
