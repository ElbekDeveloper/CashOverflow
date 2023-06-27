// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using Xeptions;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial class CompanyService
    {
        private delegate ValueTask<Company> ReturningCompanyFunction();

        private async ValueTask<Company> TryCatch(ReturningCompanyFunction returningCompanyFunction)
        {
            try
            {
                return await returningCompanyFunction();
            }
            catch (NullCompanyException nullCompanyException)
            {
                throw CreateAndLogValidationException(nullCompanyException);
            }
            catch (InvalidCompanyException invalidCompanyException)
            {
                throw CreateAndLogValidationException(invalidCompanyException);
            }
            catch (NotFoundCompanyException notFoundCompanyException)
            {
                throw CreateAndLogValidationException(notFoundCompanyException);
            }
        }

        private CompanyValidationException CreateAndLogValidationException(Xeption exception)
        {
            var companyValidationException = new CompanyValidationException(exception);
            this.loggingBroker.LogError(companyValidationException);

            return companyValidationException;
        }
    }
}