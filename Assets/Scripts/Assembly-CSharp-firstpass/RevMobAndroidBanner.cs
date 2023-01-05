using UnityEngine;

public class RevMobAndroidBanner : RevMobBanner
{
	private AndroidJavaObject javaObject;

	public RevMobAndroidBanner(AndroidJavaObject activity, AndroidJavaObject listener, RevMob.Position position, int x, int y, int w, int h, AndroidJavaObject session)
	{
		javaObject = session;
		javaObject.Call("createBanner", activity, listener, (int)position, x, y, w, h);
	}

	public override void Show()
	{
		Debug.Log("BCRS showBanner");
		javaObject.Call("showBanner");
	}

	public override void Hide()
	{
		Debug.Log("BCRS hideBanner");
		javaObject.Call("hideBanner");
	}

	public override void Release()
	{
		Debug.Log("BCRS releaseBanner");
		javaObject.Call("releaseBanner");
	}
}
