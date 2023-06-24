// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
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
        public async Task ShouldThrowValidationExceptionOnAddIfCompanyIsNullAndLogItAsync()
        {
            // given
            Company nullCompany = null;
            var nullCompanyException = new NullCompanyException();

            var expectedCompanyValidationException = 
                new CompanyValidationException(nullCompanyException);

            // when
            ValueTask<Company> addCompanyTask =
                this.companyService.AddCompanyAsync(nullCompany);

            CompanyValidationException actualCompanyValidationException =
                await Assert.ThrowsAsync<CompanyValidationException>(addCompanyTask.AsTask);

            // then
            actualCompanyValidationException.Should().BeEquivalentTo(
                expectedCompanyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCompanyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCompanyAsync(It.IsAny<Company>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddIfCompanyIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidCompany = new Company
            {
                Name = invalidText
            };

            var invalidCompanyException = new InvalidCompanyException();

            invalidCompanyException.AddData(
                key: nameof(Company.Id),
                values: "Id is required");

            invalidCompanyException.AddData(
                key: nameof(Company.Name),
                values: "Text is required");

            invalidCompanyException.AddData(
                key: nameof(Company.CreatedDate),
                values: "Value is required");

            var expectedCompanyValidationException =
                new CompanyValidationException(invalidCompanyException);

            // when
            ValueTask<Company> addCompanyTask = 
                this.companyService.AddCompanyAsync(invalidCompany);

            CompanyValidationException actualCompanyValidationException =
                await Assert.ThrowsAsync<CompanyValidationException>(addCompanyTask.AsTask);

            // then
            actualCompanyValidationException.Should().BeEquivalentTo(
                expectedCompanyValidationException);

            this.loggingBrokerMock.Verify(broker => broker.LogError(
                It.Is(SameExceptionAs(expectedCompanyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertCompanyAsync(
                It.IsAny<Company>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
