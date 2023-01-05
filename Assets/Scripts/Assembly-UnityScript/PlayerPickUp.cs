using System;
using UnityEngine;

[Serializable]
public class PlayerPickUp : MonoBehaviour
{
	public BlockType type;

	public PoolType poolType;

	public int scoreValue;

	public bool destroyParent;

	public AudioClip pickupSound;

	public bool justDeactivate;

	public PlayerPickUp()
	{
		type = BlockType.GREEN;
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (!(other.gameObject.tag == "PlayerPickupBox"))
		{
			return;
		}
		AudioSource.PlayClipAtPoint(pickupSound, transform.position);
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
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject);
			if (destroyParent)
			{
				UnityEngine.Object.Destroy(transform.parent.gameObject);
			}
		}
		Global.gm.AddBlock(type);
		Global.gm.AddToScore(scoreValue);
	}

	public virtual void Main()
	{
	}
}
