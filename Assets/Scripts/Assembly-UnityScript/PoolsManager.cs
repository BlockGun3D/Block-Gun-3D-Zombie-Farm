using System;
using UnityEngine;

[Serializable]
public class PoolsManager : MonoBehaviour
{
	public ObjectPool[] pools;

	[NonSerialized]
	public static ObjectPool[] gPools;

	public virtual void Start()
	{
		gPools = new ObjectPool[pools.Length];
		for (int i = 0; i < pools.Length; i++)
		{
			gPools[i] = pools[i];
		}
	}

	public static GameObject GetObject(PoolType type, Vector3 pos, Quaternion rot)
	{
		return gPools[(int)type].GetObject(pos, rot);
	}

	public static void ReturnObject(GameObject theObject, PoolType type)
	{
		gPools[(int)type].ReturnObject(theObject);
	}

	public virtual void Main()
	{
	}
}
