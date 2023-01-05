namespace Unibill.Impl
{
	public interface IRawGooglePlayInterface
	{
		void initialise(GooglePlayBillingService callback, string publicKey, string[] productIds);

		void pollForConsumables();

		void purchase(string json);

		void restoreTransactions();
	}
}
