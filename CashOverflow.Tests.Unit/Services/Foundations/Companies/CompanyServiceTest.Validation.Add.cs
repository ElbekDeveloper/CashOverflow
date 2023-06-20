using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
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
			NullCompanyException nullCompanyException = new();
			CompanyValidationException companyValidationException = new(nullCompanyException);

			// when
			ValueTask<Company> addCompanyAsync =
				this.companyService.AddCompanyAsync(nullCompany);

			// then
			await Assert.ThrowsAsync<CompanyValidationException>(() => 
				addCompanyAsync.AsTask());

			this.storageBrokerMock.Verify(broker => 
				broker.InsertCompanyAsync(It.IsAny<Company>()), Times.Never);

			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
