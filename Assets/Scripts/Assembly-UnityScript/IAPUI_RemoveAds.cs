using System;
using UnityEngine;

[Serializable]
public class IAPUI_RemoveAds : MonoBehaviour
{
	public GameObject revealObjectOnPurchase;

	public virtual void Start()
	{
		int num = 0;
		Color color = revealObjectOnPurchase.GetComponent<GUITexture>().color;
		float num2 = (color.a = num);
		Color color3 = (revealObjectOnPurchase.GetComponent<GUITexture>().color = color);
	}

	public virtual void Update()
	{
		if (!Global.gm.ShouldShowAds())
		{
			int num = 1;
			Color color = revealObjectOnPurchase.GetComponent<GUITexture>().color;
			float num2 = (color.a = num);
			Color color3 = (revealObjectOnPurchase.GetComponent<GUITexture>().color = color);
			gameObject.SetActive(false);
		}
	}

	public virtual void Main()
	{
	}
}
