using UnityEngine;

public class RevMobAndroidFullscreen : RevMobFullscreen
{
	private AndroidJavaObject javaObject;

	public RevMobAndroidFullscreen(AndroidJavaObject javaObject)
	{
		this.javaObject = javaObject;
	}

	public override void Show()
	{
		javaObject.Call("show");
	}

	public override void Hide()
	{
		javaObject.Call("hide");
	}
}
