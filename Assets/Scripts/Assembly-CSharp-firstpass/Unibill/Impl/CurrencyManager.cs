using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Uniject;

namespace Unibill.Impl
{
	public class CurrencyManager
	{
		private IStorage storage;

		private UnibillConfiguration config;

		[CompilerGenerated]
		private static Func<VirtualCurrency, string> _003C_003Ef__am_0024cache3;

		public string[] Currencies { get; private set; }

		public CurrencyManager(UnibillConfiguration config, IStorage storage)
		{
			this.storage = storage;
			this.config = config;
			List<VirtualCurrency> currencies = config.currencies;
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003CCurrencyManager_003Em__A;
			}
			Currencies = currencies.Select(_003C_003Ef__am_0024cache3).ToArray();
		}

		public void OnPurchased(string id)
		{
			foreach (VirtualCurrency currency in config.currencies)
			{
				if (currency.mappings.ContainsKey(id))
				{
					CreditBalance(currency.currencyId, currency.mappings[id]);
				}
			}
		}

		public decimal GetCurrencyBalance(string id)
		{
			return storage.GetInt(getKey(id), 0);
		}

		public void CreditBalance(string id, decimal amount)
		{
			storage.SetInt(getKey(id), (int)(GetCurrencyBalance(id) + amount));
		}

		public void SetBalance(string id, decimal amount)
		{
			storage.SetInt(getKey(id), (int)amount);
		}

		public bool DebitBalance(string id, decimal amount)
		{
			decimal currencyBalance = GetCurrencyBalance(id);
			if (currencyBalance - amount >= 0m)
			{
				storage.SetInt(getKey(id), (int)(currencyBalance - amount));
				return true;
			}
			return false;
		}

		private string getKey(string id)
		{
			return string.Format("com.outlinegames.unibill.currencies.{0}.balance", id);
		}

		[CompilerGenerated]
		private static string _003CCurrencyManager_003Em__A(VirtualCurrency x)
		{
			return x.currencyId;
		}
	}
}
