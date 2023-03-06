using System;
namespace CashOverflow.Brokers.Loggings
{
	public interface ILoggingBroker
	{
		void LogError(Exception exception);
		void LogCritical(Exception exception);
	}
}

