using System;
using UnityEngine;

[Serializable]
public class DropItemOnDeath : MonoBehaviour
{
	public PoolType[] dropItemsType;

	public float[] chanceOfDropping;

	public virtual void Die()
	{
		for (int i = 0; i < dropItemsType.Length; i++)
		{
			float num = (float)Global.gm.GetLuckLevel() * (chanceOfDropping[i] / 2f);
			if (!(chanceOfDropping[i] + num <= UnityEngine.Random.value))
			{
				Transform transform = this.transform;
				GameObject @object = PoolsManager.GetObject(dropItemsType[i], transform.position, transform.rotation);
				break;
			}
		}
	}

	public virtual void Main()
	{
	}
}
