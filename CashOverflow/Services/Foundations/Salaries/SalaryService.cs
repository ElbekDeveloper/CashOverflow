// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;

namespace CashOverflow.Services.Foundations.Salaries
{
    public class SalaryService : ISalaryService
    {
        private IStorageBroker storageBroker;
        private ILoggingBroker loggingBroker;

        public SalaryService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Salary> AddSalaryAsync(Salary salary)
        {
            try
            {
                if (salary is null)
                {
                    throw new NullSalaryException();
                }

                return await this.storageBroker.InsertSalaryAsync(salary);
            }
            catch(NullSalaryException nullSalaryException)
            {
                var salaryValidationException = new SalaryValidationException(nullSalaryException);
                this.loggingBroker.LogError(salaryValidationException);

                throw salaryValidationException;
            }
        }
    }
}