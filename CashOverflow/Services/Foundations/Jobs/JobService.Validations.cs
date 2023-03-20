// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService
    {
        private void ValidateJobOnModify(Job job)
        {
            ValidateJobNotNull(job);
            Validate(
                (Rule: IsInvalid(job.Id), Parameter: nameof(Job.Id)),
                (Rule: IsInvalid(job.Title), Parameter: nameof(Job.Title)),
                (Rule: IsInvalid(job.CreatedDate), Parameter: nameof(Job.CreatedDate)),
                (Rule: IsInvalid(job.UpdatedDate), Parameter: nameof(Job.UpdatedDate)),
                (Rule: IsNotRecent(job.UpdatedDate), Parameter: nameof(Job.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: job.UpdatedDate,
                    secondDate: job.CreatedDate,
                    secondDateName: nameof(job.CreatedDate)),
                    Parameter: nameof(job.UpdatedDate)));
        }

        private void ValidateJobNotNull(Job job)
        {
            if (job is null)
            {
                throw new NullJobException();
            }
        }

        private static void ValidateAgainstStorageJobOnModify(Job inputJob, Job storageJob)
        {
            ValidateStorageJobExists(storageJob, inputJob.Id);
        }

        private static void ValidateStorageJobExists(Job maybejob, Guid jobId)
        {
            if (maybejob is null)
            {
                throw new NotFoundJobException(jobId);
            }
        }

        private static void ValidateJobId(Guid jobId) =>
           Validate((Rule: IsInvalid(jobId), Parameter: nameof(Job.Id)));

        private static dynamic IsInvalid(Guid jobId) => new
        {
            Condition = jobId == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent."
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidJobException = new InvalidJobException();

            foreach ((dynamic rule, string parameter) in validations)
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
