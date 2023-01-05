using System;
using UnityEngine;

[Serializable]
public class FullScreenAd : MonoBehaviour
{
	public RevMobScript revmob;

	public GameObject chartboost;

	public GameObject appLovin;

	[NonSerialized]
	private static float lastTimeAd;

	[NonSerialized]
	private static int choice;

	public virtual void Start()
	{
	}

	public virtual void FullScreenAdVoid(bool activated)
	{
		choice = Global.adNetworkChoose;
		float time = Time.time;
		if (activated && Global.gm.ShouldShowAds() && (time - lastTimeAd > 60f || !(lastTimeAd >= 1f)))
		{
			Debug.Log("Showing AppLovin ad");
			appLovin.SendMessage("ShowInterstitial");
			lastTimeAd = time;
		}
	}

	public virtual void Main()
	{
	}
}
