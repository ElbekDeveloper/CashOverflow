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
        public async Task ShouldModifyCompanyAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Company randomCompany = CreateRandomModifyCompany(randomDate);
            Company inputCompany = randomCompany;
            Company storageCompany = inputCompany.DeepClone();
            Company updatedCompany = inputCompany;
            Company expectedCompany = updatedCompany.DeepClone();
            Guid companyId = inputCompany.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCompanyByIdAsync(companyId))
                    .ReturnsAsync(storageCompany);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateCompanyAsync(inputCompany))
                    .ReturnsAsync(updatedCompany);

            // when
            Company actualCompany =
               await this.companyService.ModifyCompanyAsync(inputCompany);

            // then
            actualCompany.Should().BeEquivalentTo(expectedCompany);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCompanyByIdAsync(companyId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCompanyAsync(inputCompany), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
