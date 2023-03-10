using UnityEngine;

public class RevMobAndroidPopup : RevMobPopup
{
	private AndroidJavaObject javaObject;

	public RevMobAndroidPopup(AndroidJavaObject javaObject)
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
