using System;
using UnityEngine;

[Serializable]
public class ScaleTime : MonoBehaviour
{
	public float scale;

	public ScaleTime()
	{
		scale = 1f;
	}

	public virtual void ScaleTimeVoid()
	{
		Time.timeScale = scale;
	}

	public virtual void Main()
	{
	}
}
