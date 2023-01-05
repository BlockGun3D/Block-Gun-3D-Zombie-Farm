using UnityEngine;

public class RevMobAndroidLink : RevMobLink
{
	private AndroidJavaObject javaObject;

	public RevMobAndroidLink(AndroidJavaObject javaObject)
	{
		this.javaObject = javaObject;
	}

	public override void Open()
	{
		javaObject.Call("open");
	}

	public override void Cancel()
	{
		javaObject.Call("cancel");
	}
}
