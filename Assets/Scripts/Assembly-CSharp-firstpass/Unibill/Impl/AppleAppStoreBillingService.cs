using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Uniject;

namespace Unibill.Impl
{
	public class AppleAppStoreBillingService : IBillingService
	{
		private IBillingServiceCallback biller;

		private ProductIdRemapper remapper;

		private HashSet<PurchasableItem> products;

		private HashSet<string> productsNotReturnedByStorekit = new HashSet<string>();

		private string appReceipt;

		private ILogger logger;

		private bool restoreInProgress;

		public IStoreKitPlugin storekit { get; private set; }

		public AppleAppStoreBillingService(UnibillConfiguration db, ProductIdRemapper mapper, IStoreKitPlugin storekit, ILogger logger)
		{
			this.storekit = storekit;
			remapper = mapper;
			this.logger = logger;
			storekit.initialise(this);
			products = new HashSet<PurchasableItem>(db.AllPurchasableItems);
		}

		public void initialise(IBillingServiceCallback biller)
		{
			this.biller = biller;
			if (storekit.storeKitPaymentsAvailable())
			{
				string[] allPlatformSpecificProductIds = remapper.getAllPlatformSpecificProductIds();
				storekit.storeKitRequestProductData(string.Join(",", allPlatformSpecificProductIds), allPlatformSpecificProductIds);
			}
			else
			{
				biller.logError(UnibillError.STOREKIT_BILLING_UNAVAILABLE);
				biller.onSetupComplete(false);
			}
		}

		public void purchase(string item, string developerPayload)
		{
			if (productsNotReturnedByStorekit.Contains(item))
			{
				biller.logError(UnibillError.STOREKIT_ATTEMPTING_TO_PURCHASE_PRODUCT_NOT_RETURNED_BY_STOREKIT, item);
				biller.onPurchaseFailedEvent(item);
			}
			else
			{
				storekit.storeKitPurchaseProduct(item);
			}
		}

		public void restoreTransactions()
		{
			restoreInProgress = true;
			storekit.storeKitRestoreTransactions();
		}

		public void onProductListReceived(string productListString)
		{
			if (productListString.Length == 0)
			{
				biller.logError(UnibillError.STOREKIT_RETURNED_NO_PRODUCTS);
				biller.onSetupComplete(false);
				return;
			}
			Dictionary<string, object> dic = (Dictionary<string, object>)MiniJSON.jsonDecode(productListString);
			appReceipt = dic.getString("appReceipt", string.Empty);
			Dictionary<string, object> hash = dic.getHash("products");
			HashSet<PurchasableItem> hashSet = new HashSet<PurchasableItem>();
			foreach (string key in hash.Keys)
			{
				if (!remapper.canMapProductSpecificId(key.ToString()))
				{
					biller.logError(UnibillError.UNIBILL_UNKNOWN_PRODUCTID, key.ToString());
					continue;
				}
				PurchasableItem purchasableItemFromPlatformSpecificId = remapper.getPurchasableItemFromPlatformSpecificId(key.ToString());
				Dictionary<string, object> dictionary = (Dictionary<string, object>)hash[key];
				PurchasableItem.Writer.setLocalizedPrice(purchasableItemFromPlatformSpecificId, dictionary["price"].ToString());
				PurchasableItem.Writer.setLocalizedTitle(purchasableItemFromPlatformSpecificId, dictionary["localizedTitle"].ToString());
				PurchasableItem.Writer.setLocalizedDescription(purchasableItemFromPlatformSpecificId, dictionary["localizedDescription"].ToString());
				if (dictionary.ContainsKey("isoCurrencyCode"))
				{
					PurchasableItem.Writer.setISOCurrencySymbol(purchasableItemFromPlatformSpecificId, dictionary["isoCurrencyCode"].ToString());
				}
				if (dictionary.ContainsKey("priceDecimal"))
				{
					PurchasableItem.Writer.setPriceInLocalCurrency(purchasableItemFromPlatformSpecificId, decimal.Parse(dictionary["priceDecimal"].ToString()));
				}
				hashSet.Add(purchasableItemFromPlatformSpecificId);
			}
			HashSet<PurchasableItem> hashSet2 = new HashSet<PurchasableItem>(products);
			hashSet2.ExceptWith(hashSet);
			if (hashSet2.Count > 0)
			{
				foreach (PurchasableItem item in hashSet2)
				{
					biller.logError(UnibillError.STOREKIT_REQUESTPRODUCTS_MISSING_PRODUCT, item.Id, remapper.mapItemIdToPlatformSpecificId(item));
				}
			}
			productsNotReturnedByStorekit = new HashSet<string>(hashSet2.Select(_003ConProductListReceived_003Em__1));
			storekit.addTransactionObserver();
			if (appReceipt != null)
			{
				biller.setAppReceipt(appReceipt);
			}
			biller.onSetupComplete(true);
		}

		public void onPurchaseSucceeded(string data)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)MiniJSON.jsonDecode(data);
			appReceipt = (string)dictionary["receipt"];
			string text = (string)dictionary["productId"];
			if (restoreInProgress && remapper.canMapProductSpecificId(text) && remapper.getPurchasableItemFromPlatformSpecificId(text).PurchaseType == PurchaseType.Consumable)
			{
				logger.Log("Ignoring restore of consumable: " + text);
			}
			else
			{
				biller.onPurchaseSucceeded(text, appReceipt);
			}
		}

		public void onPurchaseCancelled(string productId)
		{
			biller.onPurchaseCancelledEvent(productId);
		}

		public void onPurchaseFailed(string productId)
		{
			biller.onPurchaseFailedEvent(productId);
		}

		public void onPurchaseDeferred(string productId)
		{
			biller.onPurchaseDeferredEvent(productId);
		}

		public void onTransactionsRestoredSuccess()
		{
			restoreInProgress = false;
			biller.onTransactionsRestoredSuccess();
		}

		public void onTransactionsRestoredFail(string error)
		{
			restoreInProgress = false;
			biller.onTransactionsRestoredFail(error);
		}

		public void onFailedToRetrieveProductList()
		{
			biller.logError(UnibillError.STOREKIT_FAILED_TO_RETRIEVE_PRODUCT_DATA);
			biller.onSetupComplete(true);
		}

		public bool hasReceipt(string forItem)
		{
			return !string.IsNullOrEmpty(appReceipt);
		}

		public string getReceipt(string forItem)
		{
			return appReceipt;
		}

		[CompilerGenerated]
		private string _003ConProductListReceived_003Em__1(PurchasableItem x)
		{
			return remapper.mapItemIdToPlatformSpecificId(x);
		}
	}
}
