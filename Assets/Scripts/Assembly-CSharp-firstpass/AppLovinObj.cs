using UnityEngine;

public class AppLovinObj : MonoBehaviour
{
	private AppLovin appLovin;

	public GameObject revmob;

	private bool interLoaded;

	private void Start()
	{
		AppLovin.InitializeSdk();
		AppLovin.SetUnityAdListener("AppLovin");
		AppLovin.ShowInterstitial();
		AppLovin.PreloadInterstitial();
	}

	private void Update()
	{
	}

	private void ShowInterstitial()
	{
		if (interLoaded)
		{
			AppLovin.ShowInterstitial();
			AppLovin.PreloadInterstitial();
		}
		else
		{
			AppLovin.PreloadInterstitial();
		}
	}

	private void onAppLovinEventReceived(string ev)
	{
		if (!ev.Equals("HIDDENINTER"))
		{
			if (ev.Equals("LOADEDINTER"))
			{
				interLoaded = true;
			}
			else if (ev.Equals("REWARDOVERQUOTA") || ev.Equals("REWARDREJECTED") || ev.Equals("REWARDTIMEOUT") || ev.Equals("LOADFAILED"))
			{
				ShowRevmob();
			}
		}
	}

	public void ShowRevmob()
	{
		revmob.SendMessage("ShowFullScreenAd");
	}
}
