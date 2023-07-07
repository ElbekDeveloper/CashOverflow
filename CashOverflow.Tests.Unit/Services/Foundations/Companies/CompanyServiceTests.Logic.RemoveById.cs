// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public async Task ShouldRemoveCompanyByIdAsync()
        {
            // given
            Guid randomCompanyId = Guid.NewGuid();
            Guid inputCompanyId = randomCompanyId;
            Company randomCompany = CreateRandomCompany();
            Company storageCompany = randomCompany;
            Company expectedInputCompany = storageCompany;
            Company deletedCompany = expectedInputCompany;
            Company expectedCompany = deletedCompany.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCompanyByIdAsync(inputCompanyId))
                    .ReturnsAsync(storageCompany);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteCompanyAsync(expectedInputCompany))
                    .ReturnsAsync(deletedCompany);

            // when

            Company actualCompany = await this.companyService
                .RemoveCompanyById(inputCompanyId);

            // then
            actualCompany.Should().BeEquivalentTo(expectedCompany);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCompanyByIdAsync(inputCompanyId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCompanyAsync(expectedInputCompany), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
