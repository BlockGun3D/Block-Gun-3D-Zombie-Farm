using System;
using UnityEngine;

[Serializable]
public class CameraRotationSave : MonoBehaviour
{
	public Quaternion initRot;

	public virtual void Start()
	{
		initRot = transform.rotation;
	}

	public virtual void CameraRotation(bool activated)
	{
		if (activated)
		{
			transform.rotation = initRot;
		}
	}

	public virtual void Main()
	{
	}
}
