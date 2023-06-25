// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
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
    }
}
