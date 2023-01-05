using System;
using UnityEngine;

[Serializable]
public class WanderMesh : MonoBehaviour
{
	public float wanderSpeed;

	public float wanderDuration;

	public float wanderDurationVariance;

	public string walkAnimation;

	public float stoppedDuration;

	public float stoppedDurationVariance;

	public string stoppedAnimation;

	public Transform wanderAreaTopLeft;

	public Transform wanderAreaBottomRight;

	private Transform thisTransform;

	private float timer;

	private UnityEngine.AI.NavMeshAgent navAgent;

	private float currentDuration;

	private GameManager gm;

	private bool wandering;

	public WanderMesh()
	{
		wanderSpeed = 1.5f;
		wanderDuration = 2f;
		wanderDurationVariance = 1f;
		walkAnimation = string.Empty;
		stoppedDuration = 5f;
		stoppedDurationVariance = 2f;
		stoppedAnimation = string.Empty;
		timer = -1f;
	}

	public virtual void Start()
	{
		gm = GameManager.GetInstance();
		thisTransform = transform;
		navAgent = (UnityEngine.AI.NavMeshAgent)GetComponent(typeof(UnityEngine.AI.NavMeshAgent));
		wandering = false;
	}

	public virtual void Update()
	{
		GameState gameState = gm.GetGameState();
		if (gameState == GameState.PLAYING)
		{
			if ((bool)GetComponent<AudioSource>() && !GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}
			UpdateGameplay();
			return;
		}
		navAgent.Stop();
		if ((bool)GetComponent<Animation>())
		{
			GetComponent<Animation>().Stop();
		}
		if ((bool)GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	public virtual void UpdateGameplay()
	{
		if (wandering)
		{
			if (!(timer >= 0f))
			{
				Vector3 destination = new Vector3(UnityEngine.Random.Range(wanderAreaTopLeft.position.x, wanderAreaBottomRight.position.x), thisTransform.position.y, UnityEngine.Random.Range(wanderAreaBottomRight.position.z, wanderAreaTopLeft.position.z));
				navAgent.destination = destination;
				navAgent.speed = wanderSpeed;
				timer = 0f;
				currentDuration = StaticFuncs.RandomVal(wanderDuration, wanderDurationVariance);
				if ((bool)GetComponent<Animation>() && walkAnimation != string.Empty)
				{
					GetComponent<Animation>().Play(walkAnimation);
				}
			}
			else if (!(timer <= currentDuration))
			{
				timer = -1f;
				wandering = false;
			}
			else
			{
				timer += Time.deltaTime;
			}
		}
		else if (!(timer >= 0f))
		{
			navAgent.Stop();
			timer = 0f;
			currentDuration = StaticFuncs.RandomVal(stoppedDuration, stoppedDurationVariance);
			if ((bool)GetComponent<Animation>() && stoppedAnimation != string.Empty)
			{
				GetComponent<Animation>().Play(stoppedAnimation);
			}
		}
		else if (!(timer <= currentDuration))
		{
			timer = -1f;
			wandering = true;
		}
		else
		{
			timer += Time.deltaTime;
		}
	}

	public virtual void Main()
	{
	}
}
