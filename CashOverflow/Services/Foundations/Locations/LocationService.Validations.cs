// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;

namespace CashOverflow.Services.Foundations.Locations
{
    public partial class LocationService
    {
        private void ValidateLocationOnAdd(Location location)
        {
            ValidateLocationNotNull(location);
        public void ValidateLocationById(Guid locationId) =>
             Validate((Rule: IsInvalid(locationId), Parameter: nameof(Location.Id)));

            Validate(
                (Rule: IsInvalid(location.Id), Parameter: nameof(Location.Id)),
                (Rule: IsInvalid(location.Name), Parameter: nameof(Location.Name)),
                (Rule: IsInvalid(location.CreatedDate), Parameter: nameof(Location.CreatedDate)),
                (Rule: IsInvalid(location.UpdatedDate), Parameter: nameof(Location.UpdatedDate)),
                (Rule: IsNotRecent(location.CreatedDate), Parameter: nameof(Location.CreatedDate)),

                (Rule: IsInvalid(
                    firstDate: location.CreatedDate,
                    secondDate: location.UpdatedDate,
                    secondDateName: nameof(Location.UpdatedDate)),

                Parameter: nameof(Location.CreatedDate)));
        }

        private static void ValidateLocationNotNull(Location location)
        public void ValidateStorageLocation(Location maybeLocation, Guid loactionId)
        {
            if (location is null)
            if (maybeLocation is null)
            {
                throw new NullLocationException();
                throw new NotFoundLocationException(loactionId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
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

        private bool IsDateNotRecent(DateTimeOffset date)//10:51:20
        {
            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();//10:51:00
            TimeSpan timeDifference = currentDate.Subtract(date); //-20

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLocationException = new InvalidLocationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLocationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidLocationException.ThrowIfContainsErrors();
        }
    }
}
