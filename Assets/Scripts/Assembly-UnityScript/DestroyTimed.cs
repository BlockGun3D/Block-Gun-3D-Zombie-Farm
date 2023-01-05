using System;
using UnityEngine;

[Serializable]
public class DestroyTimed : MonoBehaviour
{
	public float lifeTime;

	public float variance;

	public bool destroyParent;

	public bool justDeactivate;

	public PoolType poolType;

	private float startTime;

	private float durration;

	public DestroyTimed()
	{
		lifeTime = 1f;
	}

	public virtual void Start()
	{
		durration = UnityEngine.Random.value * variance - variance / 2f + lifeTime;
	}

	public virtual void OnEnable()
	{
		startTime = Time.time;
	}

	public virtual void Update()
	{
		if (Time.time - startTime <= durration)
		{
			return;
		}
		if (!justDeactivate)
		{
			if (destroyParent)
			{
				UnityEngine.Object.Destroy(transform.parent.gameObject);
			}
			UnityEngine.Object.Destroy(gameObject);
		}
		else if (destroyParent)
		{
			PoolsManager.ReturnObject(transform.parent.gameObject, poolType);
		}
		else
		{
			PoolsManager.ReturnObject(gameObject, poolType);
		}
	}

	public virtual void Main()
	{
	}
}
