// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

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
        public async Task ShouldAddCompanyAsync()
        {
            // given
            Company randomCompany = CreateRandomCompany();
            Company inputCompany = randomCompany;
            Company persistedCompany = inputCompany;
            Company expectedCompany = persistedCompany.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCompanyAsync(inputCompany)).
                    ReturnsAsync(persistedCompany);

            // when
            Company actualCompany = await this.companyService
                .AddCompanyAsync(inputCompany);

            // then
            actualCompany.Should().BeEquivalentTo(expectedCompany);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCompanyAsync(inputCompany), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
