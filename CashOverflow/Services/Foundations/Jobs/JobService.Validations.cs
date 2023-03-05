using System;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService
    {
        private static void ValidateJobId(Guid jobId) =>
            Validate((Rule: IsInvalid(jobId), Parameter: nameof(Job.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidJobException = new InvalidJobException();

            foreach((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidJobException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidJobException.ThrowIfContainsErrors();
        }
    }
}
