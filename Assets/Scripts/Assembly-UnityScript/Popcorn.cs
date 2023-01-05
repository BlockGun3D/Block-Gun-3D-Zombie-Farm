using System;
using UnityEngine;

[Serializable]
public class Popcorn : MonoBehaviour
{
	public float popVelocity;

	public float popVelVariance;

	public float timeBetweenPops;

	public float timeVariance;

	private Transform thisTransform;

	private float lastTimePopped;

	private float thisTimeBetween;

	public Popcorn()
	{
		popVelocity = 4f;
		popVelVariance = 1f;
		timeBetweenPops = 2f;
		timeVariance = 1f;
	}

	public virtual void Start()
	{
		thisTransform = transform;
		lastTimePopped = Time.time;
		thisTimeBetween = timeBetweenPops + timeVariance * UnityEngine.Random.value - timeVariance / 2f;
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
		if (!(Time.time - lastTimePopped < thisTimeBetween))
		{
			float num = popVelVariance * UnityEngine.Random.value - popVelVariance / 2f;
			GetComponent<Rigidbody>().velocity = new Vector3(0f, popVelocity + num, 0f);
			lastTimePopped = Time.time;
			thisTimeBetween = timeBetweenPops + timeVariance * UnityEngine.Random.value - timeVariance / 2f;
		}
	}

	public virtual void Main()
	{
	}
}
