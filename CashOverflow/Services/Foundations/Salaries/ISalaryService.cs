// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;

namespace CashOverflow.Services.Foundations.Salaries
{
    public interface ISalaryService
    {
        /// <exception cref="Models.Locations.Exceptions.LocationValidationException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationDependencyValidationException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationDependencyException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationServiceException"></exception>
        ValueTask<Salary> AddSalaryAsync(Salary salary);

        /// <exception cref="Models.Locations.Exceptions.LocationDependencyException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationServiceException"></exception>
        IQueryable<Salary> RetrieveAllSalaries();
    }
}
