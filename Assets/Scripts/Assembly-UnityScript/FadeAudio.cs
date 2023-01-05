using System;
using UnityEngine;

[Serializable]
public class FadeAudio : MonoBehaviour
{
	private float inTime;

	public FadeAudio()
	{
		inTime = -1f;
	}

	public virtual void Fade(bool fadeIn)
	{
		if (!fadeIn)
		{
		}
	}

	public virtual void Main()
	{
	}
}
