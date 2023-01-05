namespace unibill.Dummy
{
	public interface IWindowsIAPCallback
	{
		void OnProductListReceived(Product[] products);

		void OnProductListError(string message);

		void OnPurchaseSucceeded(string productId);

		void OnPurchaseSucceeded(string productId, string receipt);

		void OnPurchaseCancelled(string productId);

		void OnPurchaseFailed(string productId, string error);

		void logError(string error);

		void log(string message);
	}
}
