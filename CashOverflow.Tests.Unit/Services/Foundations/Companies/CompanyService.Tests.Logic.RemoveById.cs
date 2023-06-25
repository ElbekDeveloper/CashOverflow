// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Companies;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public async void ShouldRemoveCompanyById()
        {
            //given
            var randomCompanyId = Guid.NewGuid();
            Guid inputCompanyId = randomCompanyId;
            Company randomCompany = CreateRandomCompany();
            Company storageCompany = randomCompany;
            Company expectedInputCompany = storageCompany;
            Company deletedCompany = expectedInputCompany;
            Company expectedCompany = deletedCompany.DeepClone();
/*
            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(inputCompanyId)).ReturnsAsync(in)*/

        }
    }
}
