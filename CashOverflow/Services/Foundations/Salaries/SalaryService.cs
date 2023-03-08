// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            storageBroker.SelectAllSalaries();


        public async ValueTask<Salary> RetriveSalaryByIdAsync(Guid id) =>
            await storageBroker.SelectSalaryByIdAsync(id);

    }
}
