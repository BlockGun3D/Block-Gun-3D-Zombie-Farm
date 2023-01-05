using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ChaseAttackMesh : MonoBehaviour
{
	public GameObject targetObject;

	public GameObject secondaryTarget;

	public bool chaseSecondaryTarget;

	public bool noSecondariesAllowed;

	public float movementSpeed;

	public float firstAttackSpeed;

	public float subsequentAttackSpeed;

	public float attackDistance;

	public float stopDistAlternate;

	public bool shouldWander;

	public float maxAwarenessDistance;

	public float wanderSpeed;

	public float wanderDuration;

	public float wanderDistance;

	public string walkAnimation;

	public string attackAnimation;

	public string attackFunc;

	public Transform attackOrigin;

	private Transform thisTransform;

	private float timeInRange;

	private float timeWandering;

	private bool doneFirstAttack;

	private UnityEngine.AI.NavMeshAgent navAgent;

	private RaycastHit hit;

	private float stopDist;

	private GameManager gm;

	private ChaseState chaseState;

	public ChaseAttackMesh()
	{
		movementSpeed = 2f;
		firstAttackSpeed = 0.2f;
		subsequentAttackSpeed = 1f;
		attackDistance = 1f;
		stopDistAlternate = -1f;
		shouldWander = true;
		maxAwarenessDistance = 20f;
		wanderSpeed = 1.5f;
		wanderDuration = 5f;
		wanderDistance = 20f;
		walkAnimation = string.Empty;
		attackAnimation = string.Empty;
		attackFunc = string.Empty;
		timeInRange = -1f;
		timeWandering = -1f;
	}

	public virtual void Start()
	{
		if (!targetObject)
		{
			targetObject = Global.pm.gameObject;
		}
		gm = GameManager.GetInstance();
		thisTransform = transform;
		navAgent = (UnityEngine.AI.NavMeshAgent)GetComponent(typeof(UnityEngine.AI.NavMeshAgent));
		navAgent.stoppingDistance = (stopDist = ((stopDistAlternate <= 0f) ? (attackDistance - 0.1f) : stopDistAlternate));
		chaseState = ChaseState.NONE;
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
		chaseState = ChaseState.NONE;
		if ((bool)GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	public virtual void UpdateGameplay()
	{
		if (!secondaryTarget || !secondaryTarget.active)
		{
			secondaryTarget = targetObject;
		}
		Transform transform = ((!chaseSecondaryTarget) ? targetObject.transform : secondaryTarget.transform);
		float deltaTime = Time.deltaTime;
		Vector3 vector = transform.position - attackOrigin.position;
		float num = vector.sqrMagnitude - navAgent.radius * navAgent.radius;
		if (!navAgent.hasPath)
		{
			if (!navAgent.pathPending)
			{
				navAgent.destination = transform.position;
			}
			return;
		}
		if (!(num <= maxAwarenessDistance * maxAwarenessDistance) && shouldWander)
		{
			navAgent.stoppingDistance = 1f;
			Wander(deltaTime);
			if (!(walkAnimation != string.Empty) || chaseState == ChaseState.WALK_SLOW)
			{
				return;
			}
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(GetComponent<Animation>());
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is AnimationState))
				{
					obj = RuntimeServices.Coerce(obj, typeof(AnimationState));
				}
				AnimationState animationState = (AnimationState)obj;
				animationState.speed = 0.5f;
				UnityRuntimeServices.Update(enumerator, animationState);
			}
			if ((bool)GetComponent<Animation>())
			{
				GetComponent<Animation>().Play(walkAnimation);
			}
			chaseState = ChaseState.WALK_SLOW;
			return;
		}
		if (num > attackDistance * attackDistance || (Physics.Raycast(attackOrigin.position, vector.normalized, out hit) && hit.collider.tag == "LevelGeometry"))
		{
			navAgent.stoppingDistance = 3f;
			navAgent.destination = transform.position;
			navAgent.speed = movementSpeed;
			timeInRange = -1f;
			timeWandering = -1f;
			doneFirstAttack = false;
			if (!(walkAnimation != string.Empty) || chaseState == ChaseState.WALK_FAST)
			{
				return;
			}
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(GetComponent<Animation>());
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				if (!(obj2 is AnimationState))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(AnimationState));
				}
				AnimationState animationState2 = (AnimationState)obj2;
				animationState2.speed = 1f;
				UnityRuntimeServices.Update(enumerator2, animationState2);
			}
			if ((bool)GetComponent<Animation>())
			{
				GetComponent<Animation>().Play(walkAnimation);
			}
			chaseState = ChaseState.WALK_FAST;
			return;
		}
		navAgent.stoppingDistance = stopDist;
		thisTransform.LookAt(new Vector3(transform.position.x, thisTransform.position.y, transform.position.z));
		navAgent.destination = transform.position;
		if (!(timeInRange < 0f))
		{
			if (attackAnimation != string.Empty && chaseState != ChaseState.ATTACK)
			{
				if ((bool)GetComponent<Animation>())
				{
					GetComponent<Animation>().Play(attackAnimation);
				}
				chaseState = ChaseState.ATTACK;
			}
			if ((!doneFirstAttack && timeInRange >= firstAttackSpeed) || !(timeInRange < subsequentAttackSpeed))
			{
				gameObject.SendMessage(attackFunc, (!chaseSecondaryTarget) ? targetObject : secondaryTarget);
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

	public virtual void Wander(float timeDelta)
	{
		if (timeWandering < 0f || !(timeWandering <= wanderDuration))
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			Vector3 destination = new Vector3(thisTransform.position.x + insideUnitCircle.x * wanderDistance, thisTransform.position.y, thisTransform.position.z + insideUnitCircle.y * wanderDistance);
			navAgent.destination = destination;
			navAgent.speed = wanderSpeed;
			timeWandering = 0f;
		}
		else
		{
			timeWandering += timeDelta;
		}
	}

	public virtual void SetTarget(GameObject target)
	{
		targetObject = target;
	}

	public virtual void ChaseSecondaryTarget(GameObject secondary)
	{
		if (!noSecondariesAllowed)
		{
			secondaryTarget = secondary;
			chaseSecondaryTarget = true;
		}
	}

	public virtual void ChasePrimaryTarget()
	{
		chaseSecondaryTarget = false;
	}

	public virtual void BeenHit(float damage)
	{
		maxAwarenessDistance = 500f;
	}

	public virtual void Main()
	{
	}
}
