using System;
using UnityEngine;

[Serializable]
public class BannerAdControl : MonoBehaviour
{
	public GameObject tapjoyPrefab;

	public virtual void StateShowBanner(bool active)
	{
		if (active && !Global.bannerShowing && Global.gm.ShouldShowAds())
		{
			tapjoyPrefab.SendMessage("ShowDisplayAd");
			Global.bannerShowing = true;
		}
	}

	public virtual void StateHideBanner(bool active)
	{
		if (active && Global.bannerShowing)
		{
			tapjoyPrefab.SendMessage("HideDisplayAd");
			Global.bannerShowing = false;
		}
	}

	public virtual void Main()
	{
	}
}
