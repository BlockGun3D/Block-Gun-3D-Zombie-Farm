using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unibill;
using Unibill.Impl;
using Uniject.Impl;
using UnityEngine;

public class Unibiller
{
	[CompilerGenerated]
	private sealed class _003C_internal_doInitialise_003Ec__AnonStoreyA
	{
		internal BillerFactory factory;

		internal Biller biller;

		private static Action<PurchasableItem, string> _003C_003Ef__am_0024cache2;

		internal void _003C_003Em__13(bool success)
		{
			if (Unibiller.onBillerReady == null)
			{
				return;
			}
			if (success)
			{
				Unibiller.downloadManager = factory.instantiateDownloadManager(biller);
				DownloadManager downloadManager = Unibiller.downloadManager;
				if (_003C_003Ef__am_0024cache2 == null)
				{
					_003C_003Ef__am_0024cache2 = _003C_003Em__14;
				}
				downloadManager.onDownloadCompletedEvent += _003C_003Ef__am_0024cache2;
				Unibiller.downloadManager.onDownloadCompletedEvent += Unibiller.onDownloadCompletedEventString;
				Unibiller.downloadManager.onDownloadFailedEvent += Unibiller.onDownloadFailedEvent;
				Unibiller.downloadManager.onDownloadProgressedEvent += Unibiller.onDownloadProgressedEvent;
				Unibiller.onBillerReady((biller.State != BillerState.INITIALISED) ? UnibillState.SUCCESS_WITH_ERRORS : UnibillState.SUCCESS);
			}
			else
			{
				Unibiller.onBillerReady(UnibillState.CRITICAL_ERROR);
			}
		}

		private static void _003C_003Em__14(PurchasableItem item, string path)
		{
			if (Unibiller.onDownloadCompletedEvent != null)
			{
				Unibiller.onDownloadCompletedEvent(item, new DirectoryInfo(path));
			}
		}
	}

	private static Biller biller;

	private static DownloadManager downloadManager;

	private static DownloadManager DownloadManager;

	public static BillingPlatform BillingPlatform
	{
		get
		{
			if (biller != null)
			{
				return biller.InventoryDatabase.CurrentPlatform;
			}
			return BillingPlatform.UnityEditor;
		}
	}

	public static bool Initialised
	{
		get
		{
			if (biller != null)
			{
				return biller.State == BillerState.INITIALISED || biller.State == BillerState.INITIALISED_WITH_ERROR;
			}
			return false;
		}
	}

	public static UnibillError[] Errors
	{
		get
		{
			if (biller != null)
			{
				return biller.Errors.ToArray();
			}
			return new UnibillError[0];
		}
	}

	public static PurchasableItem[] AllPurchasableItems
	{
		get
		{
			return biller.InventoryDatabase.AllPurchasableItems.ToArray();
		}
	}

	public static PurchasableItem[] AllNonConsumablePurchasableItems
	{
		get
		{
			return biller.InventoryDatabase.AllNonConsumablePurchasableItems.ToArray();
		}
	}

	public static PurchasableItem[] AllConsumablePurchasableItems
	{
		get
		{
			return biller.InventoryDatabase.AllConsumablePurchasableItems.ToArray();
		}
	}

	public static PurchasableItem[] AllSubscriptions
	{
		get
		{
			return biller.InventoryDatabase.AllSubscriptions.ToArray();
		}
	}

	public static string[] AllCurrencies
	{
		get
		{
			return biller.CurrencyIdentifiers;
		}
	}

	public static event Action<UnibillState> onBillerReady;

	public static event Action<PurchasableItem> onPurchaseCancelled;

	public static event Action<PurchaseEvent> onPurchaseCompleteEvent;

	public static event Action<PurchasableItem> onPurchaseComplete;

	public static event Action<PurchasableItem> onPurchaseFailed;

	public static event Action<PurchasableItem> onPurchaseDeferred;

	public static event Action<PurchasableItem> onPurchaseRefunded;

	public static event Action<PurchasableItem, DirectoryInfo> onDownloadCompletedEvent;

	public static event Action<PurchasableItem, string> onDownloadCompletedEventString;

	public static event Action<PurchasableItem, int> onDownloadProgressedEvent;

	public static event Action<PurchasableItem, string> onDownloadFailedEvent;

	public static event Action<bool> onTransactionsRestored;

	public static void Initialise(List<ProductDefinition> runtimeProducts = null)
	{
		if (Unibiller.biller == null)
		{
			RemoteConfigManager remoteConfigManager = new RemoteConfigManager(new UnityResourceLoader(), new UnityPlayerPrefsStorage(), new UnityLogger(), Application.platform, runtimeProducts);
			UnibillConfiguration config = remoteConfigManager.Config;
			GameObject gameObject = new GameObject();
			UnityUtil util = gameObject.AddComponent<UnityUtil>();
			BillerFactory billerFactory = new BillerFactory(new UnityResourceLoader(), new UnityLogger(), new UnityPlayerPrefsStorage(), new RawBillingPlatformProvider(config), config, util);
			Biller biller = billerFactory.instantiate();
			_internal_doInitialise(biller, billerFactory);
		}
	}

	public static PurchasableItem GetPurchasableItemById(string unibillPurchasableId)
	{
		if (biller != null)
		{
			return biller.InventoryDatabase.getItemById(unibillPurchasableId);
		}
		return null;
	}

	public static void initiatePurchase(PurchasableItem purchasable, string developerPayload = "")
	{
		if (biller != null)
		{
			biller.purchase(purchasable, developerPayload);
		}
	}

	public static void initiatePurchase(string purchasableId, string developerPayload = "")
	{
		if (biller != null)
		{
			biller.purchase(purchasableId, developerPayload);
		}
	}

	public static int GetPurchaseCount(PurchasableItem item)
	{
		if (biller != null)
		{
			return biller.getPurchaseHistory(item);
		}
		return 0;
	}

	public static int GetPurchaseCount(string purchasableId)
	{
		if (biller != null)
		{
			return biller.getPurchaseHistory(purchasableId);
		}
		return 0;
	}

	public static decimal GetCurrencyBalance(string currencyIdentifier)
	{
		if (biller != null)
		{
			return biller.getCurrencyBalance(currencyIdentifier);
		}
		return 0m;
	}

	public static void CreditBalance(string currencyIdentifier, decimal amount)
	{
		if (biller != null)
		{
			biller.creditCurrencyBalance(currencyIdentifier, amount);
		}
	}

	public static bool DebitBalance(string currencyIdentifier, decimal amount)
	{
		if (biller != null)
		{
			return biller.debitCurrencyBalance(currencyIdentifier, amount);
		}
		return false;
	}

	public static void restoreTransactions()
	{
		if (biller != null)
		{
			biller.restoreTransactions();
		}
	}

	public static void clearTransactions()
	{
		if (biller != null)
		{
			biller.ClearPurchases();
		}
	}

	public static void DownloadContentFor(PurchasableItem item)
	{
		if (downloadManager != null)
		{
			downloadManager.downloadContentFor(item);
		}
	}

	public static DirectoryInfo GetDownloadableContentFor(PurchasableItem item)
	{
		if (downloadManager != null && item.hasDownloadableContent)
		{
			return new DirectoryInfo(downloadManager.getContentPath(item.downloadableContentId));
		}
		return null;
	}

	public static string GetDownloadableContentPathFor(PurchasableItem item)
	{
		if (downloadManager != null && item.hasDownloadableContent)
		{
			return downloadManager.getContentPath(item.downloadableContentId);
		}
		return null;
	}

	public static bool IsContentDownloaded(PurchasableItem item)
	{
		if (downloadManager != null && item.hasDownloadableContent)
		{
			return downloadManager.isDownloaded(item.downloadableContentId);
		}
		return false;
	}

	public static bool IsDownloadScheduled(PurchasableItem item)
	{
		if (downloadManager != null && item.hasDownloadableContent)
		{
			return downloadManager.isDownloadScheduled(item.downloadableContentId);
		}
		return false;
	}

	public static void DeleteDownloadedContent(PurchasableItem item)
	{
		if (downloadManager != null)
		{
			downloadManager.deleteContent(item.downloadableContentId);
		}
	}

	public static void _internal_doInitialise(Biller biller, BillerFactory factory)
	{
		_003C_internal_doInitialise_003Ec__AnonStoreyA _003C_internal_doInitialise_003Ec__AnonStoreyA = new _003C_internal_doInitialise_003Ec__AnonStoreyA();
		_003C_internal_doInitialise_003Ec__AnonStoreyA.factory = factory;
		_003C_internal_doInitialise_003Ec__AnonStoreyA.biller = biller;
		Unibiller.biller = _003C_internal_doInitialise_003Ec__AnonStoreyA.biller;
		_003C_internal_doInitialise_003Ec__AnonStoreyA.biller.onBillerReady += _003C_internal_doInitialise_003Ec__AnonStoreyA._003C_003Em__13;
		_003C_internal_doInitialise_003Ec__AnonStoreyA.biller.onPurchaseCancelled += _onPurchaseCancelled;
		_003C_internal_doInitialise_003Ec__AnonStoreyA.biller.onPurchaseComplete += _onPurchaseComplete;
		_003C_internal_doInitialise_003Ec__AnonStoreyA.biller.onPurchaseFailed += _onPurchaseFailed;
		_003C_internal_doInitialise_003Ec__AnonStoreyA.biller.onPurchaseDeferred += _onPurchaseDeferred;
		_003C_internal_doInitialise_003Ec__AnonStoreyA.biller.onPurchaseRefunded += _onPurchaseRefunded;
		_003C_internal_doInitialise_003Ec__AnonStoreyA.biller.onTransactionsRestored += _onTransactionsRestored;
		_003C_internal_doInitialise_003Ec__AnonStoreyA.biller.Initialise();
	}

	private static void _onPurchaseCancelled(PurchasableItem item)
	{
		if (Unibiller.onPurchaseCancelled != null)
		{
			Unibiller.onPurchaseCancelled(item);
		}
	}

	private static void _onPurchaseComplete(PurchaseEvent e)
	{
		if (Unibiller.onPurchaseComplete != null)
		{
			Unibiller.onPurchaseComplete(e.PurchasedItem);
		}
		if (Unibiller.onPurchaseCompleteEvent != null)
		{
			Unibiller.onPurchaseCompleteEvent(e);
		}
	}

	private static void _onPurchaseFailed(PurchasableItem item)
	{
		if (Unibiller.onPurchaseFailed != null)
		{
			Unibiller.onPurchaseFailed(item);
		}
	}

	private static void _onPurchaseDeferred(PurchasableItem item)
	{
		if (Unibiller.onPurchaseDeferred != null)
		{
			Unibiller.onPurchaseDeferred(item);
		}
	}

	private static void _onPurchaseRefunded(PurchasableItem item)
	{
		if (Unibiller.onPurchaseRefunded != null)
		{
			Unibiller.onPurchaseRefunded(item);
		}
	}

	private static void _onTransactionsRestored(bool success)
	{
		if (Unibiller.onTransactionsRestored != null)
		{
			Unibiller.onTransactionsRestored(success);
		}
	}
}
