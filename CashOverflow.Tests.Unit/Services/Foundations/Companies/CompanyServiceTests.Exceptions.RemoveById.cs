// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var databaseUpdateConcurencyException = new DbUpdateConcurrencyException();

            var lockedCompanyException = 
                new LockedCompanyException(databaseUpdateConcurencyException);

            var expectedCompanyDependencyValidationException =
                new CompanyDependencyValidationException(lockedCompanyException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCompanyByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurencyException);

            // when
            ValueTask<Company> removeByIdTask =
                this.companyService.RemoveCompanyById(someId);

            CompanyDependencyValidationException actualCompanyDependencyValidationException =
                await Assert.ThrowsAsync<CompanyDependencyValidationException>(
                    removeByIdTask.AsTask);

            // then
            actualCompanyDependencyValidationException.Should()
                .BeEquivalentTo(expectedCompanyDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCompanyByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCompanyAsync(It.IsAny<Company>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
