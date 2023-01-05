using System;
using UnityEngine;

[Serializable]
public class BockExplodeDeath : MonoBehaviour
{
	public PoolType explodeObjectType;

	public PoolType poolType;

	public int numExplodeObjects;

	public int numExplodeVariance;

	public float speed;

	public float distanceOfObjectsFromCenter;

	public Color color1;

	public BlockType type1;

	public Color color2;

	public BlockType type2;

	public bool destroyParent;

	public AudioClip deathSound;

	public bool justDeactivate;

	public bool justDisableNoPool;

	public bool useStaticPosition;

	public Vector3 staticPosition;

	public Transform explodeSpot;

	private Transform thisTransform;

	public BockExplodeDeath()
	{
		numExplodeObjects = 5;
		speed = 1f;
		distanceOfObjectsFromCenter = 1f;
	}

	public virtual void Start()
	{
		thisTransform = transform;
	}

	public virtual void Die()
	{
		if ((bool)deathSound)
		{
			AudioSource.PlayClipAtPoint(deathSound, thisTransform.position, 5f);
		}
		if ((bool)GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Stop();
		}
		BoxCollider boxCollider = (BoxCollider)gameObject.GetComponent(typeof(BoxCollider));
		Vector3 vector = new Vector3(0f, boxCollider.center.y, 0f);
		float num = StaticFuncs.RandomVal(numExplodeObjects, numExplodeVariance);
		if ((bool)explodeSpot)
		{
			vector = explodeSpot.position;
		}
		else if (useStaticPosition)
		{
			vector = staticPosition;
		}
		else
		{
			vector += thisTransform.position;
		}
		for (int i = 0; (float)i < num; i++)
		{
			Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
			GameObject @object = PoolsManager.GetObject(explodeObjectType, onUnitSphere * distanceOfObjectsFromCenter + vector, thisTransform.rotation);
			@object.GetComponent<Rigidbody>().velocity = onUnitSphere * speed;
			PlayerPickUp playerPickUp = (PlayerPickUp)@object.GetComponent(typeof(PlayerPickUp));
			if (!(UnityEngine.Random.value <= 0.5f))
			{
				@object.GetComponent<Renderer>().material.color = color1;
				if ((bool)playerPickUp)
				{
					playerPickUp.type = type1;
				}
			}
			else
			{
				@object.GetComponent<Renderer>().material.color = color2;
				if ((bool)playerPickUp)
				{
					playerPickUp.type = type2;
				}
			}
		}
		if (justDeactivate)
		{
			if (destroyParent)
			{
				PoolsManager.ReturnObject(thisTransform.parent.gameObject, poolType);
			}
			else
			{
				PoolsManager.ReturnObject(gameObject, poolType);
			}
			return;
		}
		if (justDisableNoPool)
		{
			if (destroyParent)
			{
				thisTransform.parent.gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(false);
			}
			return;
		}
		UnityEngine.Object.Destroy(gameObject, 0f);
		if (destroyParent)
		{
			UnityEngine.Object.Destroy(thisTransform.parent.gameObject);
		}
	}

	public virtual void Main()
	{
	}
}
