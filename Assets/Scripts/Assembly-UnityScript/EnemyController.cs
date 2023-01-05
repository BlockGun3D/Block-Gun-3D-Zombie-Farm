using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class EnemyController : MonoBehaviour
{
	public GameObject[] enemyPrefabs;

	public int[] difficultyValues;

	public EnemyAssault[] waves;

	public bool allWavesTogether;

	public ObjectiveManager objectiveManager;

	public int maxSpawnedEnemies;

	private int currentWave;

	private int numWavesDone;

	private int numSpawnedEnemies;

	public EnemyController()
	{
		maxSpawnedEnemies = 30;
	}

	public virtual Enemy GetEnemy(DifficultyLevel level, Vector3 pos, Quaternion rot)
	{
		object result;
		if (numSpawnedEnemies < maxSpawnedEnemies)
		{
			Enemy enemy = new Enemy();
			enemy.@object = (GameObject)UnityEngine.Object.Instantiate(enemyPrefabs[(int)level], pos, rot);
			enemy.difficultyValue = difficultyValues[(int)level];
			numSpawnedEnemies++;
			result = enemy;
		}
		else
		{
			result = null;
		}
		return (Enemy)result;
	}

	public virtual void Activate()
	{
		if (Global.missionNum != objectiveManager.missionNum)
		{
			Deactivate();
			objectiveManager.Close();
			return;
		}
		objectiveManager.Start();
		if (allWavesTogether)
		{
			int i = 0;
			EnemyAssault[] array = waves;
			for (int length = array.Length; i < length; i++)
			{
				array[i].Go();
			}
		}
		else
		{
			currentWave = Global.gm.waveToStartOn;
			waves[currentWave].Go();
		}
	}

	public virtual void Deactivate()
	{
		Reset();
		objectiveManager.Close();
		gameObject.SetActive(false);
	}

	public virtual void WaveDone(bool isBoss)
	{
		numWavesDone++;
		if ((bool)objectiveManager && (isBoss || numWavesDone >= waves.Length))
		{
			objectiveManager.WavesComplete();
		}
	}

	public virtual void StartNewWave()
	{
		waves[++currentWave].Go();
	}

	public virtual void Reset()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		int i = 0;
		GameObject[] array2 = array;
		for (int length = array2.Length; i < length; i++)
		{
			UnityEngine.Object.Destroy(array2[i]);
		}
		currentWave = 0;
		numWavesDone = 0;
		numSpawnedEnemies = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform = (Transform)obj;
			transform.gameObject.SendMessage("Reset");
			UnityRuntimeServices.Update(enumerator, transform);
		}
	}

	public virtual void EnemyDead()
	{
		numSpawnedEnemies--;
	}

	public virtual void Main()
	{
	}
}
