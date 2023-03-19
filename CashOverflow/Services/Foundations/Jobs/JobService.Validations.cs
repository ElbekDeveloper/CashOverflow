// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using System;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService
    {
        private void ValidateJobOnAdd(Job job)
        {
            ValidateJobNotNull(job);

            Validate(
                (Rule: IsInvalid(job.Id), Parameter: nameof(Job.Id)),
                (Rule: IsInvalid(job.Title), Parameter: nameof(Job.Title)),
                (Rule: IsInvalid(job.Level), Parameter: nameof(Job.Level)),
                (Rule: IsInvalid(job.CreatedDate), Parameter: nameof(Job.CreatedDate)),
                (Rule: IsInvalid(job.UpdatedDate), Parameter: nameof(Job.UpdatedDate)),
                (Rule: IsNotRecent(job.CreatedDate), Parameter: nameof(Job.CreatedDate)),

                (Rule: IsInvalid(
                   firstDate: job.CreatedDate,
                   secondDate: job.UpdatedDate,
                   secondDateName: nameof(job.UpdatedDate)),

                 Parameter: nameof(Job.CreatedDate)));
        }

        private static void ValidateJobNotNull(Job job)
        {
            if (job is null)
            {
                throw new NullJobException();
            }
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

        private static dynamic IsInvalid(string title) => new
        {
            Condition = String.IsNullOrWhiteSpace(title),
            Message = "Title is required"
        };

        private static dynamic IsInvalid(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsInvalid(Level level) => new
        {
            Condition = level == default,
            Message = "Level is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDate.Subtract(date);

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

