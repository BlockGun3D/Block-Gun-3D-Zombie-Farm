using System;
using UnityEngine;

[Serializable]
public class RateUsButton : MonoBehaviour
{
	public virtual void Update()
	{
		if (Global.gm.pressedRate == 1 || Global.gm.openedCount < 3)
		{
			gameObject.SetActive(false);
		}
	}

	public virtual void Main()
	{
	}
}
