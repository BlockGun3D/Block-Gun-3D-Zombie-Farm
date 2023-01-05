using System.Collections.Generic;

namespace Unibill.Impl
{
	public interface IBillingServiceCallback
	{
		void onSetupComplete(bool successful);

		void onPurchaseSucceeded(string platformSpecificId, string receipt);

		void onPurchaseCancelledEvent(string item);

		void onPurchaseRefundedEvent(string item);

		void onPurchaseFailedEvent(string item);

		void onPurchaseDeferredEvent(string item);

		void onTransactionsRestoredSuccess();

		void onTransactionsRestoredFail(string error);

		void onActiveSubscriptionsRetrieved(IEnumerable<string> subscriptions);

		void onPurchaseReceiptRetrieved(string productId, string receipt);

		void setAppReceipt(string receipt);

		void logError(UnibillError error, params object[] args);

		void logError(UnibillError error);
	}
}
