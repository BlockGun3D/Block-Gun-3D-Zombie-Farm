using System;
using UnityEngine;

[Serializable]
public class DeactivateFunc : MonoBehaviour
{
	public virtual void Deactivate()
	{
		gameObject.SetActive(false);
	}

	public virtual void Main()
	{
	}
}
