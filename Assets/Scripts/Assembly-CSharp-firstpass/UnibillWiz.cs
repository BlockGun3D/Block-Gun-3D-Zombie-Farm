using UnityEngine;

[AddComponentMenu("Unibill/UnibillWiz")]
public class UnibillWiz : MonoBehaviour
{
	private PurchasableItem[] items;

	public GameObject receivingObject;

	private void Start()
	{
	}

	private void onBillerReady(UnibillState state)
	{
		Debug.Log("onBillerReady:" + state);
		switch (state)
		{
		case UnibillState.SUCCESS:
		case UnibillState.SUCCESS_WITH_ERRORS:
			receivingObject.SendMessage("StoreConnected");
			break;
		case UnibillState.CRITICAL_ERROR:
			receivingObject.SendMessage("StoreCouldNotConnect");
			break;
		}
	}

	private void onTransactionsRestored(bool success)
	{
		Debug.Log("Transactions restored.");
		int num = 0;
		if (Unibiller.GetPurchaseCount(items[6]) > 0)
		{
			num |= 1;
		}
		if (Unibiller.GetPurchaseCount(items[7]) > 0)
		{
			num |= 2;
		}
		receivingObject.SendMessage("TransactionsRestored", num);
	}

	private void onPurchased(PurchasableItem item)
	{
		Debug.Log("Purchase OK: " + item.Id);
		Debug.Log(string.Format("{0} has now been purchased {1} times.", item.name, Unibiller.GetPurchaseCount(item)));
		int indexInList = GetIndexInList(item);
		receivingObject.SendMessage("PurchaseComplete", indexInList);
	}

	private void onCancelled(PurchasableItem item)
	{
		Debug.Log("Purchase cancelled: " + item.Id);
		int indexInList = GetIndexInList(item);
		receivingObject.SendMessage("PurchaseCanceled", indexInList);
	}

	private void onFailed(PurchasableItem item)
	{
		Debug.Log("Purchase failed: " + item.Id);
		int indexInList = GetIndexInList(item);
		receivingObject.SendMessage("PurchaseFailed", indexInList);
	}

	private int GetIndexInList(PurchasableItem item)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].Id == item.Id)
			{
				return i;
			}
		}
		Debug.Log("GetIndexInList: Not in list!");
		return -1;
	}

	public void Update()
	{
	}

	public void InitializeUnibill()
	{
		if (Resources.Load("unibillInventory.json") == null)
		{
			Debug.LogError("You must define your purchasable inventory within the inventory editor!");
			base.gameObject.SetActive(false);
			return;
		}
		Unibiller.onBillerReady += onBillerReady;
		Unibiller.onTransactionsRestored += onTransactionsRestored;
		Unibiller.onPurchaseCancelled += onCancelled;
		Unibiller.onPurchaseFailed += onFailed;
		Unibiller.onPurchaseComplete += onPurchased;
		Unibiller.Initialise();
		items = Unibiller.AllPurchasableItems;
	}

	public void BuyItemWithInventoryIndex(int index)
	{
		if (index > items.Length)
		{
			Debug.Log("BuyItemWithInventoryIndex: index out of bounds");
		}
		else
		{
			Unibiller.initiatePurchase(items[index], string.Empty);
		}
	}

	public void RestoreTransactions()
	{
		Unibiller.restoreTransactions();
	}

	public string GetLocalizedPrice(int index)
	{
		if (index > items.Length)
		{
			Debug.Log("GetLocalizedPrice: index out of bounds");
			return string.Empty;
		}
		return items[index].localizedPriceString;
	}

	public string GetLocalizedTitle(int index)
	{
		if (index > items.Length)
		{
			Debug.Log("GetLocalizedTitle: index out of bounds");
			return string.Empty;
		}
		return items[index].localizedTitle;
	}

	public int GetPurchaseCount(int index)
	{
		if (index > items.Length)
		{
			Debug.Log("GetLocalizedTitle: index out of bounds");
			return -1;
		}
		return Unibiller.GetPurchaseCount(items[index]);
	}
}
