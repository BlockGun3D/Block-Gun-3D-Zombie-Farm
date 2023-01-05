using System;
using UnityEngine;

[Serializable]
public class ObjectiveManager : MonoBehaviour
{
	public ObjectiveType objectiveType;

	public int missionNum;

	public LevelManager levelManager;

	public HealthController[] destroyObjectHealths;

	public float objectHealth;

	public bool protectObjects;

	private int objectsLeft;

	private bool failed;

	public virtual void WavesComplete()
	{
		if (objectiveType == ObjectiveType.DEFEAT_WAVES || objectiveType == ObjectiveType.DEFEAT_BOSS || (objectiveType == ObjectiveType.PROTECT && !failed))
		{
			Debug.Log("ObjectiveManager: Calling level manager success");
			StartCoroutine(levelManager.Success());
		}
	}

	public virtual void Start()
	{
		int i = 0;
		HealthController[] array = destroyObjectHealths;
		for (int length = array.Length; i < length; i++)
		{
			array[i].enabled = true;
			Reset(array[i]);
		}
		objectsLeft = destroyObjectHealths.Length;
	}

	public virtual void Close()
	{
		int i = 0;
		HealthController[] array = destroyObjectHealths;
		for (int length = array.Length; i < length; i++)
		{
			Reset(array[i]);
		}
		objectsLeft = destroyObjectHealths.Length;
	}

	private void Reset(HealthController health)
	{
		health.gameObject.SetActive(true);
		health.Reset(objectHealth);
		failed = false;
	}

	public virtual void ObjectDestroyed()
	{
		objectsLeft--;
		if (objectsLeft < 1)
		{
			if (protectObjects)
			{
				failed = true;
				StartCoroutine(levelManager.Fail());
			}
			else
			{
				StartCoroutine(levelManager.Success());
			}
		}
	}

	public virtual void Main()
	{
	}
}
