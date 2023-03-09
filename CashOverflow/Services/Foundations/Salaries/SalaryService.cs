// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;

namespace CashOverflow.Services.Foundations.Salaries
{
    public class SalaryService : ISalaryService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public SalaryService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Salary> AddSalaryAsync(Salary salary) =>
            await storageBroker.InsertSalaryAsync(salary);

        public IQueryable<Salary> RetrieveSalaryAll() =>
            throw new NotImplementedException();


        public ValueTask<Salary> RetriveSalaryByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
