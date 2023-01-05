using System;
using System.Collections.Generic;
using Unibill.Impl;
using Uniject;

namespace Unibill
{
	public class Biller : IBillingServiceCallback, IReceiptStore
	{
		private TransactionDatabase transactionDatabase;

		private ILogger logger;

		private HelpCentre help;

		private ProductIdRemapper remapper;

		private Dictionary<PurchasableItem, List<string>> receiptMap = new Dictionary<PurchasableItem, List<string>>();

		private CurrencyManager currencyManager;

		private Dictionary<string, string> fileBundleReceipts = new Dictionary<string, string>();

		public UnibillConfiguration InventoryDatabase { get; private set; }

		public IBillingService billingSubsystem { get; private set; }

		public BillerState State { get; private set; }

		public List<UnibillError> Errors { get; private set; }

		public bool Ready
		{
			get
			{
				return State == BillerState.INITIALISED || State == BillerState.INITIALISED_WITH_ERROR;
			}
		}

		public string[] CurrencyIdentifiers
		{
			get
			{
				return currencyManager.Currencies;
			}
		}

		public event Action<bool> onBillerReady;

		public event Action<PurchaseEvent> onPurchaseComplete;

		public event Action<bool> onTransactionRestoreBegin;

		public event Action<bool> onTransactionsRestored;

		public event Action<PurchasableItem> onPurchaseCancelled;

		public event Action<PurchasableItem> onPurchaseRefunded;

		public event Action<PurchasableItem> onPurchaseFailed;

		public event Action<PurchasableItem> onPurchaseDeferred;

		public Biller(UnibillConfiguration config, TransactionDatabase tDb, IBillingService billingSubsystem, ILogger logger, HelpCentre help, ProductIdRemapper remapper, CurrencyManager currencyManager)
		{
			InventoryDatabase = config;
			transactionDatabase = tDb;
			this.billingSubsystem = billingSubsystem;
			this.logger = logger;
			logger.prefix = "UnibillBiller";
			this.help = help;
			Errors = new List<UnibillError>();
			this.remapper = remapper;
			this.currencyManager = currencyManager;
		}

		public void Initialise()
		{
			if (InventoryDatabase.AllPurchasableItems.Count == 0)
			{
				logError(UnibillError.UNIBILL_NO_PRODUCTS_DEFINED);
				onSetupComplete(false);
			}
			else
			{
				billingSubsystem.initialise(this);
			}
		}

		public int getPurchaseHistory(PurchasableItem item)
		{
			return transactionDatabase.getPurchaseHistory(item);
		}

		public int getPurchaseHistory(string purchasableId)
		{
			PurchasableItem itemById = InventoryDatabase.getItemById(purchasableId);
			if (itemById == null)
			{
				return -1;
			}
			return getPurchaseHistory(itemById);
		}

		public decimal getCurrencyBalance(string identifier)
		{
			return currencyManager.GetCurrencyBalance(identifier);
		}

		public void creditCurrencyBalance(string identifier, decimal amount)
		{
			currencyManager.CreditBalance(identifier, amount);
		}

		public bool debitCurrencyBalance(string identifier, decimal amount)
		{
			return currencyManager.DebitBalance(identifier, amount);
		}

		public void purchase(PurchasableItem item, string developerPayload = "")
		{
			if (State == BillerState.INITIALISING)
			{
				logError(UnibillError.BILLER_NOT_READY);
				return;
			}
			if (State == BillerState.INITIALISED_WITH_CRITICAL_ERROR)
			{
				logError(UnibillError.UNIBILL_INITIALISE_FAILED_WITH_CRITICAL_ERROR);
				return;
			}
			if (item == null)
			{
				logger.LogError("Trying to purchase null PurchasableItem");
				return;
			}
			if (item.PurchaseType == PurchaseType.NonConsumable && transactionDatabase.getPurchaseHistory(item) > 0)
			{
				logError(UnibillError.UNIBILL_ATTEMPTING_TO_PURCHASE_ALREADY_OWNED_NON_CONSUMABLE);
				return;
			}
			billingSubsystem.purchase(remapper.mapItemIdToPlatformSpecificId(item), developerPayload);
			logger.Log("purchase({0})", item.Id);
		}

		public void purchase(string purchasableId, string developerPayload = "")
		{
			PurchasableItem itemById = InventoryDatabase.getItemById(purchasableId);
			if (itemById == null)
			{
				logger.LogWarning("Unable to purchase unknown item with id: {0}", purchasableId);
			}
			purchase(itemById, developerPayload);
		}

		public void restoreTransactions()
		{
			logger.Log("restoreTransactions()");
			if (!Ready)
			{
				logError(UnibillError.BILLER_NOT_READY);
				return;
			}
			if (this.onTransactionRestoreBegin != null)
			{
				this.onTransactionRestoreBegin(true);
			}
			billingSubsystem.restoreTransactions();
		}

		public void onPurchaseSucceeded(string id, string receipt)
		{
			if (!verifyPlatformId(id))
			{
				return;
			}
			if (receipt != null)
			{
				onPurchaseReceiptRetrieved(id, receipt);
			}
			PurchasableItem purchasableItemFromPlatformSpecificId = remapper.getPurchasableItemFromPlatformSpecificId(id);
			if (receipt != null && receipt.Length > 0)
			{
				if (!receiptMap.ContainsKey(purchasableItemFromPlatformSpecificId))
				{
					receiptMap.Add(purchasableItemFromPlatformSpecificId, new List<string>());
				}
				receiptMap[purchasableItemFromPlatformSpecificId].Add(receipt);
			}
			if (purchasableItemFromPlatformSpecificId.PurchaseType == PurchaseType.NonConsumable && transactionDatabase.getPurchaseHistory(purchasableItemFromPlatformSpecificId) > 0)
			{
				logger.Log("Ignoring multi purchase of non consumable");
				return;
			}
			logger.Log("onPurchaseSucceeded({0})", purchasableItemFromPlatformSpecificId.Id);
			transactionDatabase.onPurchase(purchasableItemFromPlatformSpecificId);
			currencyManager.OnPurchased(purchasableItemFromPlatformSpecificId.Id);
			if (this.onPurchaseComplete != null)
			{
				this.onPurchaseComplete(new PurchaseEvent(purchasableItemFromPlatformSpecificId, receipt));
			}
		}

		public void onSetupComplete(bool available)
		{
			logger.Log("onSetupComplete({0})", available);
			State = ((!available) ? BillerState.INITIALISED_WITH_CRITICAL_ERROR : ((Errors.Count <= 0) ? BillerState.INITIALISED : BillerState.INITIALISED_WITH_ERROR));
			if (this.onBillerReady != null)
			{
				this.onBillerReady(Ready);
			}
		}

		public void onPurchaseCancelledEvent(string id)
		{
			if (verifyPlatformId(id))
			{
				PurchasableItem purchasableItemFromPlatformSpecificId = remapper.getPurchasableItemFromPlatformSpecificId(id);
				logger.Log("onPurchaseCancelledEvent({0})", purchasableItemFromPlatformSpecificId.Id);
				if (this.onPurchaseCancelled != null)
				{
					this.onPurchaseCancelled(purchasableItemFromPlatformSpecificId);
				}
			}
		}

		public void onPurchaseDeferredEvent(string id)
		{
			if (verifyPlatformId(id))
			{
				PurchasableItem purchasableItemFromPlatformSpecificId = remapper.getPurchasableItemFromPlatformSpecificId(id);
				logger.Log("onPurchaseDeferredEvent({0})", purchasableItemFromPlatformSpecificId.Id);
				if (this.onPurchaseDeferred != null)
				{
					this.onPurchaseDeferred(purchasableItemFromPlatformSpecificId);
				}
			}
		}

		public void onPurchaseRefundedEvent(string id)
		{
			if (verifyPlatformId(id))
			{
				PurchasableItem purchasableItemFromPlatformSpecificId = remapper.getPurchasableItemFromPlatformSpecificId(id);
				logger.Log("onPurchaseRefundedEvent({0})", purchasableItemFromPlatformSpecificId.Id);
				transactionDatabase.onRefunded(purchasableItemFromPlatformSpecificId);
				if (this.onPurchaseRefunded != null)
				{
					this.onPurchaseRefunded(purchasableItemFromPlatformSpecificId);
				}
			}
		}

		public void onPurchaseFailedEvent(string id)
		{
			if (verifyPlatformId(id))
			{
				PurchasableItem purchasableItemFromPlatformSpecificId = remapper.getPurchasableItemFromPlatformSpecificId(id);
				logger.Log("onPurchaseFailedEvent({0})", purchasableItemFromPlatformSpecificId.Id);
				if (this.onPurchaseFailed != null)
				{
					this.onPurchaseFailed(purchasableItemFromPlatformSpecificId);
				}
			}
		}

		public void onTransactionsRestoredSuccess()
		{
			logger.Log("onTransactionsRestoredSuccess()");
			if (this.onTransactionsRestored != null)
			{
				this.onTransactionsRestored(true);
			}
		}

		public void ClearPurchases()
		{
			foreach (PurchasableItem allPurchasableItem in InventoryDatabase.AllPurchasableItems)
			{
				transactionDatabase.clearPurchases(allPurchasableItem);
			}
		}

		public void onTransactionsRestoredFail(string error)
		{
			logger.Log("onTransactionsRestoredFail({0})", error);
			this.onTransactionsRestored(false);
		}

		public bool isOwned(PurchasableItem item)
		{
			return getPurchaseHistory(item) > 0;
		}

		public void setAppReceipt(string receipt)
		{
			foreach (PurchasableItem allPurchasableItem in InventoryDatabase.AllPurchasableItems)
			{
				if (getPurchaseHistory(allPurchasableItem) > 0)
				{
					allPurchasableItem.receipt = receipt;
				}
			}
		}

		public void onActiveSubscriptionsRetrieved(IEnumerable<string> subscriptions)
		{
			foreach (PurchasableItem allSubscription in InventoryDatabase.AllSubscriptions)
			{
				transactionDatabase.clearPurchases(allSubscription);
			}
			foreach (string subscription in subscriptions)
			{
				if (!remapper.canMapProductSpecificId(subscription))
				{
					logger.LogError("Entitled to unknown subscription: {0}. Ignoring", subscription);
				}
				else
				{
					transactionDatabase.onPurchase(remapper.getPurchasableItemFromPlatformSpecificId(subscription));
				}
			}
		}

		public void logError(UnibillError error)
		{
			logError(error, new object[0]);
		}

		public void logError(UnibillError error, params object[] args)
		{
			Errors.Add(error);
			logger.LogError(help.getMessage(error), args);
		}

		public void onPurchaseReceiptRetrieved(string platformSpecificItemId, string receipt)
		{
			if (remapper.canMapProductSpecificId(platformSpecificItemId))
			{
				PurchasableItem purchasableItemFromPlatformSpecificId = remapper.getPurchasableItemFromPlatformSpecificId(platformSpecificItemId);
				purchasableItemFromPlatformSpecificId.receipt = receipt;
				if (!string.IsNullOrEmpty(purchasableItemFromPlatformSpecificId.downloadableContentId))
				{
					fileBundleReceipts[purchasableItemFromPlatformSpecificId.downloadableContentId] = receipt;
				}
			}
		}

		public bool hasItemReceiptForFilebundle(string bundleId)
		{
			return fileBundleReceipts.ContainsKey(bundleId) || billingSubsystem.hasReceipt(getItemFromFileBundle(bundleId).LocalId);
		}

		public string getItemReceiptForFilebundle(string bundleId)
		{
			if (fileBundleReceipts.ContainsKey(bundleId))
			{
				return fileBundleReceipts[bundleId];
			}
			PurchasableItem itemFromFileBundle = getItemFromFileBundle(bundleId);
			if (billingSubsystem.hasReceipt(itemFromFileBundle.LocalId))
			{
				return billingSubsystem.getReceipt(itemFromFileBundle.LocalId);
			}
			return "fake";
		}

		public PurchasableItem getItemFromFileBundle(string id)
		{
			foreach (PurchasableItem allPurchasableItem in InventoryDatabase.AllPurchasableItems)
			{
				if (allPurchasableItem.downloadableContentId == id)
				{
					return allPurchasableItem;
				}
			}
			throw new ArgumentException(string.Format("Unable to find item with content id {0}", id));
		}

		private bool verifyPlatformId(string platformId)
		{
			if (!remapper.canMapProductSpecificId(platformId))
			{
				logError(UnibillError.UNIBILL_UNKNOWN_PRODUCTID, platformId);
				return false;
			}
			return true;
		}
	}
}
