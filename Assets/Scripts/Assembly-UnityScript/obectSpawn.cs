using System;
using UnityEngine;

[Serializable]
public class obectSpawn : MonoBehaviour
{
	public int maxNum;

	public float spawnFrequency;

	public float spawnRadius;

	public GameObject spawnObject;

	public GameObject targetObject;

	internal float lastSpawnTime;

	internal int numSpawned;

	private Transform thisTransform;

	public virtual void Start()
	{
		lastSpawnTime = Time.time;
		thisTransform = transform;
	}

	public virtual void Update()
	{
		GameManager instance = GameManager.GetInstance();
		GameState gameState = instance.GetGameState();
		if (gameState == GameState.PLAYING)
		{
			UpdateGameplay();
		}
	}

	public virtual void UpdateGameplay()
	{
		if (!(Time.time - lastSpawnTime <= spawnFrequency) && numSpawned < maxNum)
		{
			Vector2 vector = UnityEngine.Random.insideUnitCircle * spawnRadius;
			Vector3 position = new Vector3(thisTransform.position.x + vector.x, thisTransform.position.y, thisTransform.position.z + vector.y);
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(spawnObject, position, thisTransform.rotation);
			if ((bool)targetObject)
			{
				gameObject.SendMessage("SetTarget", targetObject);
			}
			lastSpawnTime = Time.time;
			numSpawned++;
		}
	}

	public virtual void Main()
	{
	}
}
