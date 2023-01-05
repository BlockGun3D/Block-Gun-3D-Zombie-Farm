using System;
using UnityEngine;

[Serializable]
public class warpIn : MonoBehaviour
{
	public Vector3 initialScale;

	public bool specifyFinalScale;

	public Vector3 finalScale;

	public float warpDuration;

	public float height;

	private Transform thisTransform;

	private float initTime;

	private bool doneWarp;

	public virtual void Start()
	{
		thisTransform = transform;
		transform.localScale = initialScale;
		float y = transform.position.y + height;
		Vector3 position = transform.position;
		float num = (position.y = y);
		Vector3 vector2 = (transform.position = position);
		initTime = Time.time;
	}

	public virtual void Update()
	{
		if (!doneWarp)
		{
			float num = (Time.time - initTime) / warpDuration;
			if (!specifyFinalScale)
			{
				thisTransform.localScale = Vector3.Lerp(initialScale, new Vector3(1f, 1f, 1f), num);
			}
			else
			{
				thisTransform.localScale = Vector3.Lerp(initialScale, finalScale, num);
			}
			if (!(num <= 1f))
			{
				doneWarp = true;
			}
		}
	}

	public virtual void Main()
	{
	}
}
