using System;
using UnityEngine;

[Serializable]
public class KeepAround : MonoBehaviour
{
	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(transform.gameObject);
	}

	public virtual void Main()
	{
	}
}
