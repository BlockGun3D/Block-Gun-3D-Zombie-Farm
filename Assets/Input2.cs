using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input2 : MonoBehaviour
{
	public static Touch GetTouch(int i)
	{
		return (Application.isMobilePlatform) ? Input.GetTouch(i) : InputHelper.GetTouches()[0];
	}

	public static int touchCount
	{
		get
		{
			return (Application.isMobilePlatform) ? Input.touchCount : InputHelper.GetTouches().Count;
		}
	}

	public static Touch[] touches
	{
		get
		{
			return (Application.isMobilePlatform) ? Input.touches : InputHelper.GetTouches().ToArray();
		}
	}
}
