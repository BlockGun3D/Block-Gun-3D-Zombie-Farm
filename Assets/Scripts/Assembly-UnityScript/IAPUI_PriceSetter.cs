using System;
using UnityEngine;

[Serializable]
public class IAPUI_PriceSetter : MonoBehaviour
{
	public UnibillWiz biller;

	public int inventoryIndex;

	public virtual void Start()
	{
		GetComponent<GUIText>().text = biller.GetLocalizedPrice(inventoryIndex);
	}

	public virtual void Main()
	{
	}
}
