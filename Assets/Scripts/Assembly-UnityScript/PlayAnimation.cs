using System;
using UnityEngine;

[Serializable]
public class PlayAnimation : MonoBehaviour
{
	public virtual void Start()
	{
		GetComponent<Animation>().Play();
	}

	public virtual void Update()
	{
	}

	public virtual void Main()
	{
	}
}
