namespace Unibill.Impl
{
	public interface IBillingService
	{
		void initialise(IBillingServiceCallback biller);

		void purchase(string item, string developerPayload);

		void restoreTransactions();

		bool hasReceipt(string forItem);

		string getReceipt(string forItem);
	}
}
