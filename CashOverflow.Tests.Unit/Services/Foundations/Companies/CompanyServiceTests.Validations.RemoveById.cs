// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidCompanyId = Guid.Empty;

            var invalidCompanyException = new InvalidCompanyException();

            invalidCompanyException.AddData(
                key: nameof(Company.Id),
                values: "Id is required");

            var expectedValidationException =
                new CompanyValidationException(invalidCompanyException);

            //when
            ValueTask<Company> removeCompanyByIdTask = 
                this.companyService.RemoveCompanyById(invalidCompanyId);

            CompanyValidationException actualCompanyValidationExecption =
                await Assert.ThrowsAsync<CompanyValidationException>(
                    removeCompanyByIdTask.AsTask);

            //then
            actualCompanyValidationExecption.Should().BeEquivalentTo(expectedValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCompanyAsync(It.IsAny<Company>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
