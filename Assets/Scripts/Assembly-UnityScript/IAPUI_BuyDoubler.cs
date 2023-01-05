using System;
using UnityEngine;

[Serializable]
public class IAPUI_BuyDoubler : MonoBehaviour
{
	public UnibillWiz biller;

	public int inventoryIndex;

	public virtual void IAPBuyItem()
	{
		if (!Global.gm.DoubleBlocks())
		{
			biller.BuyItemWithInventoryIndex(inventoryIndex);
		}
	}

	public virtual void Main()
	{
	}
}
