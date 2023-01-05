using System;
using UnityEngine;

[Serializable]
public class ProjectileHandler : MonoBehaviour
{
	public string enemyTag;

	public string destructibleTag;

	public string sendMessageOnImpact;

	public PoolType poolType;

	public float defaultDamage;

	public bool ignoreLevelGeometry;

	public bool hurtEnemyOrPlayer;

	private float damage;

	public ProjectileHandler()
	{
		enemyTag = "Enemy";
		destructibleTag = string.Empty;
		sendMessageOnImpact = string.Empty;
	}

	public virtual void Start()
	{
		if (damage == 0f)
		{
			damage = defaultDamage;
		}
	}

	public virtual void SetDamage(float inDamage)
	{
		damage = inDamage;
	}

	public virtual void SetEnemy(string enemy)
	{
		enemyTag = enemy;
	}

	public virtual void OnCollisionEnter(Collision collision)
	{
		HandleCollision(collision.gameObject);
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		HandleCollision(other.gameObject);
	}

	private void HandleCollision(GameObject @object)
	{
		if (Global.gm.GetGameState() == GameState.PLAYING && ((@object.tag == "LevelGeometry" && !ignoreLevelGeometry) || @object.tag == enemyTag || @object.tag == destructibleTag))
		{
			if (sendMessageOnImpact == string.Empty)
			{
				PoolsManager.ReturnObject(gameObject, poolType);
			}
			else
			{
				gameObject.SendMessage(sendMessageOnImpact);
			}
			if (@object.tag == enemyTag || @object.tag == destructibleTag)
			{
				@object.SendMessage("BeenHit", damage);
			}
		}
	}

	public virtual void Main()
	{
	}
}
