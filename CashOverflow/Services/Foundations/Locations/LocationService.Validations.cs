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
        private void ValidateLocationId(Guid locationId) =>
            Validate((Rule: IsInvalid(locationId), Parameter: nameof(Location.Id)));

        private void ValidateStorageLocation(Location maybeLocation, Guid locationId)
        {
            if (maybeLocation is null)
            {
                throw new NotFoundLocationException(locationId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static void Validate((dynamic Rule, string Parameter) value)
        {
            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();//10:51:00
            TimeSpan timeDifference = currentDate.Subtract(date); //-20

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLocationException = new InvalidLocationException();

            if (value.Rule.Condition)
                {
                invalidLocationException.AddData(
                    key: value.Parameter,
                    values: value.Rule.Message);
            }

            invalidLocationException.ThrowIfContainsErrors();
        }
    }
}
