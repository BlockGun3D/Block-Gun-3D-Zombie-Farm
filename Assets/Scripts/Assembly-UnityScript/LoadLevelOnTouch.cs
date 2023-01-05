using System;
using UnityEngine;

[Serializable]
public class LoadLevelOnTouch : MonoBehaviour
{
	public int level;

	private float initTime;

	public virtual void Start()
	{
		initTime = Time.time;
	}

	public virtual void Update()
	{
		if (Time.time - initTime <= 0.5f)
		{
			return;
		}
		int touchCount = Input2.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input2.GetTouch(i);
			GUITexture gUITexture = (GUITexture)gameObject.GetComponent(typeof(GUITexture));
			if (gUITexture.HitTest(touch.position))
			{
				Application.LoadLevel(level);
			}
		}
	}

	public virtual void Main()
	{
	}
}
