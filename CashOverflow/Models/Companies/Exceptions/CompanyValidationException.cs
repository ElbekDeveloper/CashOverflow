using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
	public class CompanyValidationException : Xeption
	{
		public CompanyValidationException(Xeption innerException)
			: base(message: "Company validation error occured. Fix it and try again", 
				  innerException)
		{ }
	}
}
