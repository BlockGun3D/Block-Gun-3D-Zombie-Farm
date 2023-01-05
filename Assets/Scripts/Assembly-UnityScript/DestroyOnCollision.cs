using System;
using UnityEngine;

[Serializable]
public class DestroyOnCollision : MonoBehaviour
{
	public virtual void OnCollisionEnter(Collision collision)
	{
		UnityEngine.Object.Destroy(gameObject, 0f);
	}

	public virtual void Main()
	{
	}
}
