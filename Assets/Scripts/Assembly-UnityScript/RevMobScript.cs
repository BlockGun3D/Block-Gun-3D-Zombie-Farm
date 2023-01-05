using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RevMobScript : MonoBehaviour
{
	public string androidAppID;

	public string iosAppID;

	public string amazonAppID;

	public bool useAmazon;

	public GameObject chartboost;

	private Dictionary<string, string> APP_IDS;

	private RevMob revmob;

	private bool revMobSetup;

	private RevMobBanner banner;

	private float lastFail;

	public RevMobScript()
	{
		androidAppID = "Your Android App ID";
		iosAppID = "Your iOS App ID";
		amazonAppID = "Amazon app ID";
		APP_IDS = new Dictionary<string, string>();
	}

	public virtual void Start()
	{
		revmob = RevMob.Start(APP_IDS, gameObject.name);
		revMobSetup = true;
		Debug.Log(gameObject.name);
	}

	public virtual void ShowFullScreenAd()
	{
		if (revMobSetup)
		{
			revmob.ShowFullscreen();
		}
	}

	public virtual void ShowBanner()
	{
		if (revMobSetup)
		{
			revmob.ShowBanner();
		}
	}

	public virtual void HideBanner()
	{
		if (revMobSetup)
		{
			revmob.HideBanner();
		}
	}

	public virtual void AdDidReceive(object adUnitType)
	{
		Debug.Log("Ad did received");
	}

	public virtual void AdDidFail(object adUnitType)
	{
		Debug.Log("MAAAAAATTTTTTTTTTTT: Ad did not received");
		if ((bool)chartboost && Global.adNetworkChoose == 0 && (Time.time - lastFail > 20f || !(Time.time >= 20f)))
		{
			chartboost.SendMessage("ShowInterstitial");
			lastFail = Time.time;
		}
	}

	public virtual void AdDisplayed(object adUnitType)
	{
		Debug.Log("MATTTTTTTTTTTTT: Revmob Ad displayed");
	}

	public virtual void UserClickedInTheAd(object adUnitType)
	{
		Debug.Log("Ad clicked");
	}

	public virtual void UserClosedTheAd(object adUnitType)
	{
		Debug.Log("Ad closed");
		if (Global.adNetworkChoose == 3)
		{
			chartboost.SendMessage("ShowInterstitial");
			Debug.Log("\n\nMATTTTTT: Requesting interstitial after revmob closes\n\n\n");
		}
	}

	public virtual void Main()
	{
		APP_IDS["Android"] = ((!useAmazon) ? androidAppID : amazonAppID);
		APP_IDS["IOS"] = iosAppID;
	}
}
