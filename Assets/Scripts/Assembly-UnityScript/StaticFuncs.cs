using System;
using UnityEngine;

[Serializable]
public class StaticFuncs : MonoBehaviour
{
	[NonSerialized]
	public static GameObject thePlayer = GameObject.Find("Player");

	[NonSerialized]
	private static Touch lastTouch;

	public static bool TestButtonTouchBegan(GUITexture button)
	{
		Touch touch;
		//if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WP8Player)
		//{
			int touchCount = Input2.touchCount;
			int num = 0;
			while (num < touchCount)
			{
				touch = Input2.GetTouch(num);
				if (!button.HitTest(touch.position) || touch.phase != 0)
				{
					num++;
					continue;
				}
				goto IL_005b;
			}
		//}
		int result = 0;
		goto IL_0073;
		IL_0073:
		return (byte)result != 0;
		IL_005b:
		lastTouch = touch;
		result = 1;
		goto IL_0073;
	}

	public static bool TestButtonTouch(GUITexture button)
	{
		Touch touch;
		//if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WP8Player)
		//{
			int touchCount = Input2.touchCount;
			int num = 0;
			while (num < touchCount)
			{
				touch = Input2.GetTouch(num);
				if (!button.HitTest(touch.position))
				{
					num++;
					continue;
				}
				goto IL_004e;
			}
		//}
		int result = 0;
		goto IL_0066;
		IL_004e:
		lastTouch = touch;
		result = 1;
		goto IL_0066;
		IL_0066:
		return (byte)result != 0;
	}

	public static bool TestButtonTouchEnded(GUITexture button)
	{
		Touch touch;
		//if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WP8Player)
		//{
			int touchCount = Input2.touchCount;
			int num = 0;
			while (num < touchCount)
			{
				touch = Input2.GetTouch(num);
				if (!button.HitTest(touch.position) || (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled))
				{
					num++;
					continue;
				}
				goto IL_0068;
			}
		//}
		int result = 0;
		goto IL_0080;
		IL_0080:
		return (byte)result != 0;
		IL_0068:
		lastTouch = touch;
		result = 1;
		goto IL_0080;
	}

	public static Touch GetLastTouch()
	{
		return lastTouch;
	}

	public static float RandomVal(float val, float variance)
	{
		return val + UnityEngine.Random.value * variance - variance / 2f;
	}

	public virtual void Main()
	{
	}
}
