using System;
using UnityEngine;

[Serializable]
public class Attacks : MonoBehaviour
{
	public float attackDamage;

	public string enemyTag;

	public AudioClip attackSound;

	public PoolType projectileType;

	public float projectileSpeed;

	public Transform projectileSpawnPoint;

	public float numProjectiles;

	public float aimVariance;

	private Transform thisTransform;

	public Attacks()
	{
		attackDamage = 0.1f;
		enemyTag = "Player";
		projectileSpeed = 1f;
		numProjectiles = 1f;
	}

	public virtual void Start()
	{
		thisTransform = gameObject.transform;
	}

	public virtual void Bite(GameObject target)
	{
		if ((bool)attackSound)
		{
			AudioSource.PlayClipAtPoint(attackSound, thisTransform.position);
		}
		target.SendMessage("BeenHit", attackDamage);
	}

	public virtual void Shoot(GameObject target)
	{
		float num = Vector3.Distance(target.transform.position, projectileSpawnPoint.position) / 10f;
		for (int i = 0; (float)i < numProjectiles; i++)
		{
			GameObject @object = PoolsManager.GetObject(projectileType, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
			Vector3 vector = target.transform.position + UnityEngine.Random.insideUnitSphere * aimVariance * num;
			@object.transform.LookAt(vector);
			Vector3 vector2 = vector - @object.transform.position;
			vector2.Normalize();
			@object.GetComponent<Rigidbody>().velocity = vector2 * projectileSpeed;
			@object.SendMessage("SetDamage", attackDamage);
			@object.SendMessage("SetEnemy", enemyTag);
		}
		AudioSource.PlayClipAtPoint(attackSound, projectileSpawnPoint.position);
	}

	public virtual void Main()
	{
	}
}
