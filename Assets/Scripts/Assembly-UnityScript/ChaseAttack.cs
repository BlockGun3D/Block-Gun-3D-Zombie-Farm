using System;
using UnityEngine;

[Serializable]
public class ChaseAttack : MonoBehaviour
{
	public GameObject targetObject;

	public float movementSpeed;

	public float firstAttackSpeed;

	public float subsequentAttackSpeed;

	public float attackDamage;

	public float attackDistance;

	public bool shouldWander;

	public float maxAwarenessDistance;

	public float wanderSpeed;

	public float wanderDuration;

	public AudioClip attackSound;

	private Transform thisTransform;

	private float timeInRange;

	private float timeWandering;

	private bool doneFirstAttack;

	private GameManager gm;

	public ChaseAttack()
	{
		movementSpeed = 2f;
		firstAttackSpeed = 0.2f;
		subsequentAttackSpeed = 1f;
		attackDamage = 1f;
		attackDistance = 1f;
		shouldWander = true;
		maxAwarenessDistance = 20f;
		wanderSpeed = 1.5f;
		wanderDuration = 5f;
		timeInRange = -1f;
		timeWandering = -1f;
	}

	public virtual void Start()
	{
		gm = GameManager.GetInstance();
		thisTransform = transform;
	}

	public virtual void Update()
	{
		GameState gameState = gm.GetGameState();
		if (gameState == GameState.PLAYING)
		{
			UpdateGameplay();
		}
	}

	public virtual void UpdateGameplay()
	{
		Transform transform = targetObject.transform;
		float sqrMagnitude = (transform.position - thisTransform.position).sqrMagnitude;
		float deltaTime = Time.deltaTime;
		if (!(sqrMagnitude <= maxAwarenessDistance * maxAwarenessDistance) && shouldWander)
		{
			Wander(deltaTime);
		}
		else if (!(sqrMagnitude <= attackDistance * attackDistance))
		{
			thisTransform.LookAt(new Vector3(transform.position.x, thisTransform.position.y, transform.position.z));
			thisTransform.Translate(new Vector3(0f, 0f, movementSpeed * deltaTime));
			timeInRange = -1f;
			timeWandering = -1f;
			doneFirstAttack = false;
		}
		else if (!(timeInRange < 0f))
		{
			if ((!doneFirstAttack && timeInRange >= firstAttackSpeed) || !(timeInRange < subsequentAttackSpeed))
			{
				Attack();
				timeInRange = 0f;
				doneFirstAttack = true;
			}
			else
			{
				timeInRange += deltaTime;
			}
		}
		else
		{
			timeInRange = 0f;
		}
	}

	public virtual void Attack()
	{
		if ((bool)attackSound)
		{
			AudioSource.PlayClipAtPoint(attackSound, thisTransform.position);
		}
		targetObject.SendMessage("BeenHit", attackDamage);
	}

	public virtual void Wander(float timeDelta)
	{
		if (timeWandering < 0f || !(timeWandering <= wanderDuration))
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			thisTransform.LookAt(new Vector3(thisTransform.position.x + insideUnitCircle.x, thisTransform.position.y, thisTransform.position.z + insideUnitCircle.y));
			timeWandering = 0f;
		}
		else
		{
			timeWandering += timeDelta;
			thisTransform.Translate(new Vector3(0f, 0f, wanderSpeed * timeDelta));
		}
	}

	public virtual void SetTarget(GameObject target)
	{
		targetObject = target;
	}

	public virtual void Main()
	{
	}
}
