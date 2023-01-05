using System;
using System.IO;
using System.Runtime.CompilerServices;
using Unibill.Impl;
using UnityEngine;

[AddComponentMenu("Unibill/UnibillDemo")]
public class UnibillDemo : MonoBehaviour
{
	private ComboBox box;

	private GUIContent[] comboBoxList;

	private GUIStyle listStyle;

	private int selectedItemIndex;

	private PurchasableItem[] items;

	public Font font;

	[CompilerGenerated]
	private static Action<PurchasableItem, int> _003C_003Ef__am_0024cache6;

	[CompilerGenerated]
	private static Action<PurchasableItem, string> _003C_003Ef__am_0024cache7;

	[CompilerGenerated]
	private static Action<PurchasableItem, string> _003C_003Ef__am_0024cache8;

	private void Start()
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
		Unibiller.onPurchaseCompleteEvent += onPurchased;
		Unibiller.onPurchaseDeferred += onDeferred;
		if (_003C_003Ef__am_0024cache6 == null)
		{
			_003C_003Ef__am_0024cache6 = _003CStart_003Em__3;
		}
		Unibiller.onDownloadProgressedEvent += _003C_003Ef__am_0024cache6;
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = _003CStart_003Em__4;
		}
		Unibiller.onDownloadFailedEvent += _003C_003Ef__am_0024cache7;
		if (_003C_003Ef__am_0024cache8 == null)
		{
			_003C_003Ef__am_0024cache8 = _003CStart_003Em__5;
		}
		Unibiller.onDownloadCompletedEventString += _003C_003Ef__am_0024cache8;
		Unibiller.Initialise();
		initCombobox();
	}

	private void onBillerReady(UnibillState state)
	{
		Debug.Log("onBillerReady:" + state);
	}

	private void onTransactionsRestored(bool success)
	{
		Debug.Log("Transactions restored.");
	}

	private void onPurchased(PurchaseEvent e)
	{
		Debug.Log("Purchase OK: " + e.PurchasedItem.Id);
		Debug.Log("Receipt: " + e.Receipt);
		Debug.Log(string.Format("{0} has now been purchased {1} times.", e.PurchasedItem.name, Unibiller.GetPurchaseCount(e.PurchasedItem)));
	}

	private void onCancelled(PurchasableItem item)
	{
		Debug.Log("Purchase cancelled: " + item.Id);
	}

	private void onDeferred(PurchasableItem item)
	{
		Debug.Log("Purchase deferred blud: " + item.Id);
	}

	private void onFailed(PurchasableItem item)
	{
		Debug.Log("Purchase failed: " + item.Id);
	}

	private void initCombobox()
	{
		box = new ComboBox();
		items = Unibiller.AllPurchasableItems;
		comboBoxList = new GUIContent[items.Length];
		for (int i = 0; i < items.Length; i++)
		{
			comboBoxList[i] = new GUIContent(string.Format("{0} - {1}", items[i].localizedTitle, items[i].localizedPriceString));
		}
		listStyle = new GUIStyle();
		listStyle.font = font;
		listStyle.normal.textColor = Color.white;
		GUIStyleState onHover = listStyle.onHover;
		Texture2D background = new Texture2D(2, 2);
		listStyle.hover.background = background;
		onHover.background = background;
		RectOffset padding = listStyle.padding;
		int num = 4;
		listStyle.padding.bottom = num;
		num = num;
		listStyle.padding.top = num;
		num = num;
		listStyle.padding.right = num;
		padding.left = num;
	}

	public void Update()
	{
		for (int i = 0; i < items.Length; i++)
		{
			comboBoxList[i] = new GUIContent(string.Format("{0} - {1} - {2}", items[i].name, items[i].localizedTitle, items[i].localizedPriceString));
		}
	}

	private void OnGUI()
	{
		selectedItemIndex = box.GetSelectedItemIndex();
		selectedItemIndex = box.List(new Rect(0f, 0f, Screen.width, (float)Screen.width / 20f), comboBoxList[selectedItemIndex].text, comboBoxList, listStyle);
		if (GUI.Button(new Rect(0f, (float)Screen.height - (float)Screen.width / 6f, (float)Screen.width / 2f, (float)Screen.width / 6f), "Buy"))
		{
			Unibiller.initiatePurchase(items[selectedItemIndex], string.Empty);
		}
		if (GUI.Button(new Rect((float)Screen.width / 2f, (float)Screen.height - (float)Screen.width / 6f, (float)Screen.width / 2f, (float)Screen.width / 6f), "Restore transactions"))
		{
			Unibiller.restoreTransactions();
		}
		if (Unibiller.GetPurchaseCount(items[selectedItemIndex]) > 0 && items[selectedItemIndex].hasDownloadableContent && !Unibiller.IsContentDownloaded(items[selectedItemIndex]) && GUI.Button(new Rect(0f, (float)Screen.height - 2f * ((float)Screen.width / 6f), (float)Screen.width / 2f, (float)Screen.width / 6f), "Download"))
		{
			Unibiller.DownloadContentFor(items[selectedItemIndex]);
		}
		if (Unibiller.IsContentDownloaded(items[selectedItemIndex]) && GUI.Button(new Rect((float)Screen.width / 2f, (float)Screen.height - 2f * ((float)Screen.width / 6f), (float)Screen.width / 2f, (float)Screen.width / 6f), "Delete"))
		{
			Unibiller.DeleteDownloadedContent(items[selectedItemIndex]);
		}
		int num = (int)((float)Screen.height - (float)(2 * Screen.width) / 6f) - 50;
		PurchasableItem[] allNonConsumablePurchasableItems = Unibiller.AllNonConsumablePurchasableItems;
		foreach (PurchasableItem purchasableItem in allNonConsumablePurchasableItems)
		{
			GUI.Label(new Rect(0f, num, 500f, 50f), purchasableItem.Id, listStyle);
			GUI.Label(new Rect((float)Screen.width - (float)Screen.width * 0.1f, num, 500f, 50f), Unibiller.GetPurchaseCount(purchasableItem).ToString(), listStyle);
			num -= 30;
		}
		string[] allCurrencies = Unibiller.AllCurrencies;
		foreach (string text in allCurrencies)
		{
			GUI.Label(new Rect(0f, num, 500f, 50f), text, listStyle);
			GUI.Label(new Rect((float)Screen.width - (float)Screen.width * 0.1f, num, 500f, 50f), Unibiller.GetCurrencyBalance(text).ToString(), listStyle);
			num -= 30;
		}
		PurchasableItem[] allSubscriptions = Unibiller.AllSubscriptions;
		foreach (PurchasableItem purchasableItem2 in allSubscriptions)
		{
			GUI.Label(new Rect(0f, num, 500f, 50f), purchasableItem2.localizedTitle, listStyle);
			GUI.Label(new Rect((float)Screen.width - (float)Screen.width * 0.1f, num, 500f, 50f), Unibiller.GetPurchaseCount(purchasableItem2).ToString(), listStyle);
			num -= 30;
		}
		GUI.Label(new Rect(0f, num - 10, 500f, 50f), "Item", listStyle);
		GUI.Label(new Rect((float)Screen.width - (float)Screen.width * 0.2f, num - 10, 500f, 50f), "Count", listStyle);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__3(PurchasableItem item, int progress)
	{
		Debug.Log(item.name + " " + progress);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__4(PurchasableItem arg1, string arg2)
	{
		Debug.LogError(arg2);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__5(PurchasableItem obj, string dir)
	{
		Debug.Log("Completed download: " + obj.name);
		FileInfo[] files = new DirectoryInfo(dir).GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			Debug.Log(fileInfo.Name);
			if (fileInfo.Name.EndsWith("txt") && fileInfo.Length < 10000)
			{
				Debug.Log(Util.ReadAllText(fileInfo.FullName));
			}
		}
	}
}
