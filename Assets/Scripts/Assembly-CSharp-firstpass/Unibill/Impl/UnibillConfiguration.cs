using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Uniject;
using UnityEngine;

namespace Unibill.Impl
{
	public class UnibillConfiguration
	{
		[CompilerGenerated]
		private sealed class _003CgetItemById_003Ec__AnonStorey8
		{
			internal string id;

			internal bool _003C_003Em__11(PurchasableItem x)
			{
				return x.Id == id;
			}
		}

		[CompilerGenerated]
		private sealed class _003CgetCurrency_003Ec__AnonStorey9
		{
			internal string id;

			internal bool _003C_003Em__12(VirtualCurrency x)
			{
				return x.currencyId == id;
			}
		}

		private Uniject.ILogger logger;

		public List<PurchasableItem> inventory = new List<PurchasableItem>();

		public List<VirtualCurrency> currencies = new List<VirtualCurrency>();

		[CompilerGenerated]
		private static Predicate<PurchasableItem> _003C_003Ef__am_0024cache11;

		[CompilerGenerated]
		private static Predicate<PurchasableItem> _003C_003Ef__am_0024cache12;

		[CompilerGenerated]
		private static Predicate<PurchasableItem> _003C_003Ef__am_0024cache13;

		[CompilerGenerated]
		private static Predicate<PurchasableItem> _003C_003Ef__am_0024cache14;

		public BillingPlatform CurrentPlatform { get; set; }

		public string iOSSKU { get; set; }

		public string macAppStoreSKU { get; set; }

		public BillingPlatform AndroidBillingPlatform { get; set; }

		public string GooglePlayPublicKey { get; set; }

		public bool AmazonSandboxEnabled { get; set; }

		public bool WP8SandboxEnabled { get; set; }

		public bool UseHostedConfig { get; set; }

		public string HostedConfigUrl { get; set; }

		public string UnibillAnalyticsAppId { get; set; }

		public string UnibillAnalyticsAppSecret { get; set; }

		public bool UseWin8_1Sandbox { get; set; }

		public SamsungAppsMode SamsungAppsMode { get; set; }

		public string SamsungItemGroupId { get; set; }

		public List<PurchasableItem> AllPurchasableItems
		{
			get
			{
				return new List<PurchasableItem>(inventory);
			}
		}

		public List<PurchasableItem> AllNonConsumablePurchasableItems
		{
			get
			{
				List<PurchasableItem> list = inventory;
				if (_003C_003Ef__am_0024cache11 == null)
				{
					_003C_003Ef__am_0024cache11 = _003Cget_AllNonConsumablePurchasableItems_003Em__D;
				}
				return list.FindAll(_003C_003Ef__am_0024cache11);
			}
		}

		public List<PurchasableItem> AllConsumablePurchasableItems
		{
			get
			{
				List<PurchasableItem> list = inventory;
				if (_003C_003Ef__am_0024cache12 == null)
				{
					_003C_003Ef__am_0024cache12 = _003Cget_AllConsumablePurchasableItems_003Em__E;
				}
				return list.FindAll(_003C_003Ef__am_0024cache12);
			}
		}

		public List<PurchasableItem> AllSubscriptions
		{
			get
			{
				List<PurchasableItem> list = inventory;
				if (_003C_003Ef__am_0024cache13 == null)
				{
					_003C_003Ef__am_0024cache13 = _003Cget_AllSubscriptions_003Em__F;
				}
				return list.FindAll(_003C_003Ef__am_0024cache13);
			}
		}

		public List<PurchasableItem> AllNonSubscriptionPurchasableItems
		{
			get
			{
				List<PurchasableItem> list = inventory;
				if (_003C_003Ef__am_0024cache14 == null)
				{
					_003C_003Ef__am_0024cache14 = _003Cget_AllNonSubscriptionPurchasableItems_003Em__10;
				}
				return list.FindAll(_003C_003Ef__am_0024cache14);
			}
		}

		public UnibillConfiguration(string json, RuntimePlatform runtimePlatform, Uniject.ILogger logger, List<ProductDefinition> runtimeProducts = null)
		{
			this.logger = logger;
			Dictionary<string, object> dictionary = (Dictionary<string, object>)MiniJSON.jsonDecode(json);
			iOSSKU = dictionary.getString("iOSSKU", string.Empty);
			macAppStoreSKU = dictionary.getString("macAppStoreSKU", string.Empty);
			AndroidBillingPlatform = dictionary.getEnum<BillingPlatform>("androidBillingPlatform");
			GooglePlayPublicKey = dictionary.get<string>("GooglePlayPublicKey");
			AmazonSandboxEnabled = dictionary.getBool("useAmazonSandbox");
			WP8SandboxEnabled = dictionary.getBool("UseWP8MockingFramework");
			UseHostedConfig = dictionary.getBool("useHostedConfig");
			HostedConfigUrl = dictionary.get<string>("hostedConfigUrl");
			UseWin8_1Sandbox = dictionary.getBool("UseWin8_1Sandbox");
			SamsungAppsMode = dictionary.getEnum<SamsungAppsMode>("samsungAppsMode");
			SamsungItemGroupId = dictionary.getString("samsungAppsItemGroupId", string.Empty);
			UnibillAnalyticsAppId = dictionary.getString("unibillAnalyticsAppId", string.Empty);
			UnibillAnalyticsAppSecret = dictionary.getString("unibillAnalyticsAppSecret", string.Empty);
			switch (runtimePlatform)
			{
			case RuntimePlatform.Android:
				CurrentPlatform = AndroidBillingPlatform;
				break;
			case RuntimePlatform.IPhonePlayer:
				CurrentPlatform = BillingPlatform.AppleAppStore;
				break;
			case RuntimePlatform.OSXPlayer:
				CurrentPlatform = BillingPlatform.MacAppStore;
				break;
			case RuntimePlatform.WP8Player:
				CurrentPlatform = BillingPlatform.WindowsPhone8;
				break;
			case RuntimePlatform.MetroPlayerX86:
			case RuntimePlatform.MetroPlayerX64:
			case RuntimePlatform.MetroPlayerARM:
				CurrentPlatform = BillingPlatform.Windows8_1;
				break;
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.WindowsEditor:
				CurrentPlatform = BillingPlatform.UnityEditor;
				break;
			default:
				CurrentPlatform = BillingPlatform.UnityEditor;
				break;
			}
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["purchasableItems"];
			foreach (KeyValuePair<string, object> item3 in dictionary2)
			{
				PurchasableItem item = new PurchasableItem(item3.Key, (Dictionary<string, object>)item3.Value, CurrentPlatform);
				inventory.Add(item);
			}
			if (runtimeProducts != null)
			{
				foreach (ProductDefinition runtimeProduct in runtimeProducts)
				{
					Dictionary<string, object> hash = new Dictionary<string, object> { 
					{
						"@purchaseType",
						runtimeProduct.Type.ToString()
					} };
					PurchasableItem item2 = new PurchasableItem(runtimeProduct.PlatformSpecificId, hash, CurrentPlatform);
					inventory.Add(item2);
				}
			}
			loadCurrencies(dictionary);
		}

		private void loadCurrencies(Dictionary<string, object> root)
		{
			currencies = new List<VirtualCurrency>();
			Dictionary<string, object> hash = root.getHash("currencies");
			if (hash == null)
			{
				return;
			}
			foreach (KeyValuePair<string, object> item in hash)
			{
				Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
				foreach (KeyValuePair<string, object> item2 in (Dictionary<string, object>)item.Value)
				{
					dictionary.Add(item2.Key, decimal.Parse(item2.Value.ToString()));
				}
				currencies.Add(new VirtualCurrency(item.Key, dictionary));
			}
		}

		public PurchasableItem AddItem()
		{
			PurchasableItem purchasableItem = new PurchasableItem();
			inventory.Add(purchasableItem);
			return purchasableItem;
		}

		public Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("iOSSKU", iOSSKU);
			dictionary.Add("macAppStoreSKU", macAppStoreSKU);
			dictionary.Add("androidBillingPlatform", AndroidBillingPlatform.ToString());
			dictionary.Add("GooglePlayPublicKey", GooglePlayPublicKey);
			dictionary.Add("useAmazonSandbox", AmazonSandboxEnabled);
			dictionary.Add("UseWP8MockingFramework", WP8SandboxEnabled);
			dictionary.Add("useHostedConfig", UseHostedConfig);
			dictionary.Add("hostedConfigUrl", HostedConfigUrl);
			dictionary.Add("UseWin8_1Sandbox", UseWin8_1Sandbox);
			dictionary.Add("samsungAppsMode", SamsungAppsMode.ToString());
			dictionary.Add("samsungAppsItemGroupId", SamsungItemGroupId);
			dictionary.Add("unibillAnalyticsAppId", UnibillAnalyticsAppId);
			dictionary.Add("unibillAnalyticsAppSecret", UnibillAnalyticsAppSecret);
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			foreach (PurchasableItem item in inventory)
			{
				dictionary2.Add(item.Id, item.Serialize());
			}
			dictionary.Add("purchasableItems", dictionary2);
			Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
			foreach (VirtualCurrency currency in currencies)
			{
				dictionary3.Add(currency.currencyId, currency.mappings);
			}
			dictionary.Add("currencies", dictionary3);
			return dictionary;
		}

		public PurchasableItem getItemById(string id)
		{
			_003CgetItemById_003Ec__AnonStorey8 _003CgetItemById_003Ec__AnonStorey = new _003CgetItemById_003Ec__AnonStorey8();
			_003CgetItemById_003Ec__AnonStorey.id = id;
			PurchasableItem purchasableItem = inventory.Find(_003CgetItemById_003Ec__AnonStorey._003C_003Em__11);
			if (purchasableItem == null)
			{
				logger.LogWarning("Unknown purchasable item:{0}. Check your Unibill inventory configuration.", _003CgetItemById_003Ec__AnonStorey.id);
			}
			return purchasableItem;
		}

		public VirtualCurrency getCurrency(string id)
		{
			_003CgetCurrency_003Ec__AnonStorey9 _003CgetCurrency_003Ec__AnonStorey = new _003CgetCurrency_003Ec__AnonStorey9();
			_003CgetCurrency_003Ec__AnonStorey.id = id;
			return currencies.Find(_003CgetCurrency_003Ec__AnonStorey._003C_003Em__12);
		}

		private bool tryGetBoolean(string name, Dictionary<string, object> root)
		{
			if (root.ContainsKey(name))
			{
				return bool.Parse(root[name].ToString());
			}
			return false;
		}

		[CompilerGenerated]
		private static bool _003Cget_AllNonConsumablePurchasableItems_003Em__D(PurchasableItem x)
		{
			return x.PurchaseType == PurchaseType.NonConsumable;
		}

		[CompilerGenerated]
		private static bool _003Cget_AllConsumablePurchasableItems_003Em__E(PurchasableItem x)
		{
			return x.PurchaseType == PurchaseType.Consumable;
		}

		[CompilerGenerated]
		private static bool _003Cget_AllSubscriptions_003Em__F(PurchasableItem x)
		{
			return x.PurchaseType == PurchaseType.Subscription;
		}

		[CompilerGenerated]
		private static bool _003Cget_AllNonSubscriptionPurchasableItems_003Em__10(PurchasableItem x)
		{
			return x.PurchaseType != PurchaseType.Subscription;
		}
	}
}
