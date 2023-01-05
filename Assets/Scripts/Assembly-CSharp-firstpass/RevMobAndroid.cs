using UnityEngine;

public class RevMobAndroid : RevMob
{
	private AndroidJavaObject session;

	public RevMobAndroid(string appId, string gameObjectName)
	{
		base.gameObjectName = gameObjectName;
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.revmob.unity.UnityRevMob");
		session = androidJavaClass.CallStatic<AndroidJavaObject>("start", new object[5]
		{
			CurrentActivity(),
			appId,
			"unity-android",
			RevMob.REVMOB_VERSION,
			new AndroidJavaObject("com.revmob.unity.RevMobAdsUnityListener", gameObjectName, "session")
		});
	}

	public static AndroidJavaObject CurrentActivity()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		return androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public override bool IsDevice()
	{
		return Application.platform == RuntimePlatform.Android;
	}

	private AndroidJavaObject adUnitWrapperCall(string methodName, string placementId, string adUnit)
	{
		if (placementId == null)
		{
			placementId = string.Empty;
		}
		AndroidJavaObject androidJavaObject = CreateRevMobListener(gameObjectName, adUnit);
		return session.Call<AndroidJavaObject>(methodName, new object[3]
		{
			CurrentActivity(),
			placementId,
			androidJavaObject
		});
	}

	private AndroidJavaObject CreateRevMobListener(string gameObjectName, string adUnityType)
	{
		return new AndroidJavaObject("com.revmob.unity.RevMobAdsUnityListener", gameObjectName, adUnityType);
	}

	public override void PrintEnvironmentInformation()
	{
		session.Call("printEnvironmentInformation", CurrentActivity());
	}

	public override void SetTestingMode(Test test)
	{
		session.Call("setTestingMode", (int)test);
	}

	public override void SetTimeoutInSeconds(int timeout)
	{
		session.Call("setTimeoutInSeconds", timeout);
	}

	public override RevMobFullscreen ShowFullscreen(string placementId)
	{
		return new RevMobAndroidFullscreen(adUnitWrapperCall("showFullscreen", placementId, "Fullscreen"));
	}

	public override RevMobFullscreen CreateFullscreen(string placementId)
	{
		if (!IsDevice())
		{
			return null;
		}
		AndroidJavaObject javaObject = adUnitWrapperCall("createFullscreen", placementId, "Fullscreen");
		return new RevMobAndroidFullscreen(javaObject);
	}

	public override RevMobBanner CreateBanner(Position position, int x, int y, int w, int h)
	{
		return (!IsDevice()) ? null : new RevMobAndroidBanner(CurrentActivity(), CreateRevMobListener(gameObjectName, "Banner"), position, x, y, w, h, session);
	}

	public override void ShowBanner(Position position, int x, int y, int w, int h)
	{
		if (IsDevice())
		{
			session.Call("showBanner", CurrentActivity(), CreateRevMobListener(gameObjectName, "Banner"), (int)position, x, y, w, h);
		}
	}

	public override void HideBanner()
	{
		session.Call("hideBanner", CurrentActivity());
	}

	public override RevMobLink OpenAdLink(string placementId)
	{
		return new RevMobAndroidLink(adUnitWrapperCall("openAdLink", placementId, "Link"));
	}

	public override RevMobLink CreateAdLink(string placementId)
	{
		if (!IsDevice())
		{
			return null;
		}
		AndroidJavaObject javaObject = adUnitWrapperCall("createAdLink", placementId, "Link");
		return new RevMobAndroidLink(javaObject);
	}

	public override RevMobPopup ShowPopup(string placementId)
	{
		return new RevMobAndroidPopup(adUnitWrapperCall("showPopup", placementId, "Popup"));
	}

	public override RevMobPopup CreatePopup(string placementId)
	{
		return new RevMobAndroidPopup(adUnitWrapperCall("createPopup", placementId, "Popup"));
	}
}
