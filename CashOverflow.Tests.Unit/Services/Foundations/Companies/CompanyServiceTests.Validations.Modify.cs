// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCompanyIsNullAndLogItAsync()
        {
            // given
            Company nullCompany = null;
            var nullCompanyException = new NullCompanyException();

            var expectedCompanyValidationException =
                new CompanyValidationException(nullCompanyException);

            // when
            ValueTask<Company> modifyCompanyTask = this.companyService.ModifyCompanyAsync(nullCompany);

            CompanyValidationException actualCompanyValidationException =
                await Assert.ThrowsAsync<CompanyValidationException>(modifyCompanyTask.AsTask);

            // then
            actualCompanyValidationException.Should().BeEquivalentTo(expectedCompanyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfCompanyIsInvalidAndLogItAsync(string invalidString)
        {
            // given
            var invalidCompany = new Company
            {
                Name = invalidString
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
                values: "Date is required");

            var expectedCompanyValidationException =
                new CompanyValidationException(invalidCompanyException);

            // when
            ValueTask<Company> modifyCompanyTask =
                this.companyService.ModifyCompanyAsync(invalidCompany);

            CompanyValidationException actualCompanyValidationException =
                await Assert.ThrowsAsync<CompanyValidationException>(
                    modifyCompanyTask.AsTask);

            // then
            actualCompanyValidationException.Should().BeEquivalentTo(expectedCompanyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCompanyDoesNotExistAndLogItAsync()
        {
            // given
            int negativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Company randomCompany = CreateRandomCompany(dateTime);
            Company nonExistCompany = randomCompany;
            nonExistCompany.CreatedDate = dateTime.AddMinutes(negativeMinutes);
            Company nullCompany = null;

            var notFoundCompanyException = new NotFoundCompanyException(nonExistCompany.Id);

            var expectedCompanyValidationException =
                new CompanyValidationException(notFoundCompanyException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCompanyByIdAsync(nonExistCompany.Id))
                    .ReturnsAsync(nullCompany);

            // when
            ValueTask<Company> modifyCompanyTask =
                this.companyService.ModifyCompanyAsync(nonExistCompany);

            CompanyValidationException actualCompanyValidationException =
                await Assert.ThrowsAsync<CompanyValidationException>(
                    modifyCompanyTask.AsTask);

            // then
            actualCompanyValidationException.Should().BeEquivalentTo(expectedCompanyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCompanyByIdAsync(nonExistCompany.Id), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Company randomCompany = CreateRandomModifyCompany(randomDateTime);
            Company invalidCompany = randomCompany.DeepClone();
            Company storageCompany = invalidCompany.DeepClone();
            storageCompany.CreatedDate = storageCompany.CreatedDate.AddMinutes(randomMinutes);
            var invalidCompanyException = new InvalidCompanyException();
            Guid CompanyId = invalidCompany.Id;

            invalidCompanyException.AddData(
                key: nameof(Company.CreatedDate),
                values: $"Date is not same as {nameof(Company.CreatedDate)}");

            var expectedCompanyValidationException =
                new CompanyValidationException(invalidCompanyException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCompanyByIdAsync(CompanyId)).ReturnsAsync(storageCompany);

            // when
            ValueTask<Company> modifyCompanyTask =
                this.companyService.ModifyCompanyAsync(invalidCompany);

            CompanyValidationException actualCompanyValidationException =
                await Assert.ThrowsAsync<CompanyValidationException>(
                    modifyCompanyTask.AsTask);

            // then
            actualCompanyValidationException.Should().BeEquivalentTo(expectedCompanyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCompanyByIdAsync(invalidCompany.Id), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
