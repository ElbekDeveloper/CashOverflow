// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;

namespace CashOverflow.Services.Foundations.Locations
{
    public class LocationService : ILocationService
    {
        public ValueTask<Location> RemoveLocationById(Guid locationId)
        {
            throw new NotImplementedException();
        }
    }
}
