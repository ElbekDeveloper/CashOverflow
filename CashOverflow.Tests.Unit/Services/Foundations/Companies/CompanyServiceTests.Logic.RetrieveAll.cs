// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Companies;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Linq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllCompanies()
        {
            //given
            IQueryable<Company> randomCompanies = CreateRandomCompanies();
            IQueryable<Company> storageCompanies = randomCompanies;
            IQueryable<Company> expectedCompanies = storageCompanies.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCompanies()).Returns(storageCompanies);

            //when
            IQueryable<Company> actualCompanies =
                this.companyService.RetrieveAllCompanies();

            //then
            actualCompanies.Should().BeEquivalentTo(expectedCompanies);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCompanies(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
