// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Locations;

namespace CashOverflow.Services.Foundations.Locations
{
    public interface ILocationService
    {
        IQueryable<Location> RetrieveAllLocations();
    }
}
