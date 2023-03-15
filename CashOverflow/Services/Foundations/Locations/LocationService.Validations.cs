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
        public void ValidateLocationById(Guid locationId) =>
             Validate((Rule: IsInvalid(locationId), Parameter: nameof(Location.Id)));

        public void ValidateStorageLocation(Location maybeLocation, Guid loactionId)
        {
            if (maybeLocation is null)
            {
                throw new NotFoundLocationException(loactionId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
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
