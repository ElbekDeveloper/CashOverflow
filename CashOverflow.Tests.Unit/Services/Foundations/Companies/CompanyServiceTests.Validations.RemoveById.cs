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

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfCompanyIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomCompanyId = Guid.NewGuid();
            Guid inputCompanyId = randomCompanyId;
            Company noCompany = null;

            var notFoundComopanyException = 
                new NotFoundCompanyException(inputCompanyId);

            var expectedCompanyValidationException =
                new CompanyValidationException(notFoundComopanyException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCompanyByIdAsync(inputCompanyId)).ReturnsAsync(noCompany);

            // when 
            ValueTask<Company> removeCompanyByIdTask =
                this.companyService.RemoveCompanyById(inputCompanyId);

            CompanyValidationException actualCompanyValidationException =
                await Assert.ThrowsAsync<CompanyValidationException>(
                    removeCompanyByIdTask.AsTask);

            // then
            actualCompanyValidationException.Should().BeEquivalentTo(expectedCompanyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCompanyByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
