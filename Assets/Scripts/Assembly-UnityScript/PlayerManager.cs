using System;
using UnityEngine;

[Serializable]
public class PlayerManager : MonoBehaviour
{
	private Vector3 initPos;

	private Quaternion initRot;

	public virtual void Start()
	{
		initPos = transform.position;
		initRot = transform.rotation;
	}

	public virtual void Reset()
	{
		transform.position = initPos;
		transform.rotation = initRot;
	}

	public virtual void Main()
	{
	}
}
