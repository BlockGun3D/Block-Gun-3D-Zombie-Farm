using System;
using UnityEngine;

[Serializable]
public class IAPUI_BuyNoAds : MonoBehaviour
{
	public UnibillWiz biller;

	public int inventoryIndex;

	public virtual void IAPBuyItem()
	{
		if (Global.gm.ShouldShowAds())
		{
			biller.BuyItemWithInventoryIndex(inventoryIndex);
		}
	}

	public virtual void Main()
	{
	}
}
