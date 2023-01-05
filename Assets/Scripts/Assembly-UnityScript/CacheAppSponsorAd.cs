using System;
using UnityEngine;

[Serializable]
public class CacheAppSponsorAd : MonoBehaviour
{
	public GameObject appSponsor;

	public virtual void CacheAd(bool active)
	{
		if (active && Global.gm.ShouldShowAds() && (bool)appSponsor && ((Global.adNetworkChoose == 4 && Application.systemLanguage == SystemLanguage.English) || Global.adNetworkChoose == 5))
		{
			appSponsor.SendMessage("CacheAd");
		}
	}

	public virtual void Main()
	{
	}
}
