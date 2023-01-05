using System;
using UnityEngine;

[Serializable]
public class IAPUI_BuyItem : MonoBehaviour
{
	public UnibillWiz biller;

	public int inventoryIndex;

	public virtual void IAPBuyItem()
	{
		biller.BuyItemWithInventoryIndex(inventoryIndex);
	}

	public virtual void RestorePurchases()
	{
		biller.RestoreTransactions();
	}

	public virtual void Main()
	{
	}
}
