using System;
using UnityEngine;

[Serializable]
public class SpawnEnemyOnDeath : MonoBehaviour
{
	public GameObject enemy;

	public GameObject targetObject;

	private Transform thisTransform;

	public virtual void Start()
	{
		thisTransform = transform;
	}

	public virtual void Die()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(enemy, thisTransform.position, thisTransform.rotation);
		if ((bool)targetObject)
		{
			gameObject.SendMessage("SetTarget", targetObject);
		}
		else
		{
			gameObject.SendMessage("SetTarget", Global.pm.gameObject);
		}
		ReportDeath reportDeath = (ReportDeath)gameObject.GetComponent(typeof(ReportDeath));
		reportDeath.enabled = false;
	}

	public virtual void Main()
	{
	}
}
