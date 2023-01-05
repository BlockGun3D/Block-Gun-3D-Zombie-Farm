using System;
using UnityEngine;

[Serializable]
public class CacheAdStuff : MonoBehaviour
{
	public GameObject chartboost;

	public virtual void CacheMoreApps(bool active)
	{
		if (active && (bool)chartboost)
		{
			chartboost.SendMessage("CacheMoreApps");
		}
	}

	public virtual void Main()
	{
	}
}
