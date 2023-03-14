// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Salary> InsertLocationAsync(Salary location);
        IQueryable<Salary> SelectAllLocations();
        ValueTask<Salary> SelectLocationByIdAsync(Guid Id);
        ValueTask<Salary> UpdateLocationAsync(Salary location);
        ValueTask<Salary> DeleteLocationAsync(Salary location);
    }
}
