using System;
using UnityEngine;

[Serializable]
public class WeaponPickup : MonoBehaviour
{
	public GunType type;

	public PoolType poolType;

	public bool destroyParent;

	public bool justDeactivate;

	private bool enabledOnce;

	public virtual void OnEnable()
	{
		if (Global.gm.GetGunLevel(type) == 0 && enabledOnce)
		{
			DestroyPickup();
		}
		if (!enabledOnce)
		{
			enabledOnce = true;
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "PlayerPickupBox")
		{
			Global.wm.GotWeapon(type);
			DestroyPickup();
		}
	}

	public virtual void DestroyPickup()
	{
		if (justDeactivate)
		{
			if (destroyParent)
			{
				PoolsManager.ReturnObject(transform.parent.gameObject, poolType);
			}
			else
			{
				PoolsManager.ReturnObject(gameObject, poolType);
			}
			return;
		}
		if (destroyParent)
		{
			UnityEngine.Object.Destroy(transform.parent.gameObject);
		}
		UnityEngine.Object.Destroy(gameObject);
	}

	public virtual void Main()
	{
	}
}
