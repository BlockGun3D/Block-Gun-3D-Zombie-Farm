using Uniject;
using Uniject.Impl;
using UnityEngine;

namespace Unibill.Impl
{
	internal class RawBillingPlatformProvider : IRawBillingPlatformProvider
	{
		private UnibillConfiguration config;

		private GameObject gameObject;

		private ILevelLoadListener listener;

		private IHTTPClient client;

		public RawBillingPlatformProvider(UnibillConfiguration config)
		{
			this.config = config;
			gameObject = new GameObject();
		}

		public IRawGooglePlayInterface getGooglePlay()
		{
			return new RawGooglePlayInterface();
		}

		public IRawAmazonAppStoreBillingInterface getAmazon()
		{
			return new RawAmazonAppStoreBillingInterface(config);
		}

		public IStoreKitPlugin getStorekit()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return new StoreKitPluginImpl();
			}
			return new OSXStoreKitPluginImpl();
		}

		public IRawSamsungAppsBillingService getSamsung()
		{
			return new RawSamsungAppsBillingInterface();
		}

		public ILevelLoadListener getLevelLoadListener()
		{
			if (listener == null)
			{
				listener = gameObject.AddComponent<UnityLevelLoadListener>();
			}
			return listener;
		}

		public IHTTPClient getHTTPClient(IUtil util)
		{
			if (client == null)
			{
				client = new HTTPClient(util);
			}
			return client;
		}
	}
}
