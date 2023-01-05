using System;
using UnityEngine;

[Serializable]
public class PlaySoundOnActivate : MonoBehaviour
{
	public virtual void PlaySound(bool activated)
	{
		if (activated)
		{
			GetComponent<AudioSource>().Play();
		}
	}

	public virtual void Main()
	{
	}
}
