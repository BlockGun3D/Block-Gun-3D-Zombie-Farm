using System;
using UnityEngine;

[Serializable]
public class BoundaryMatt
{
	public Vector2 min;

	public Vector2 max;

	public BoundaryMatt()
	{
		min = Vector2.zero;
		max = Vector2.zero;
	}
}
