// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Locations;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Locations
{
    public interface ILocationService
    {
        ValueTask<Location> AddLocationAsync(Location location);
    }
}
