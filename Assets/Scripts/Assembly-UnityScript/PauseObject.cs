using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class PauseObject : MonoBehaviour
{
	public virtual void Update()
	{
		GameState gameState = Global.gm.GetGameState();
		if (gameState == GameState.PLAYING)
		{
			if ((bool)GetComponent<AudioSource>() && !GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}
			if (!GetComponent<Animation>())
			{
				return;
			}
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(GetComponent<Animation>());
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is AnimationState))
				{
					obj = RuntimeServices.Coerce(obj, typeof(AnimationState));
				}
				AnimationState animationState = (AnimationState)obj;
				animationState.speed = 1f;
				UnityRuntimeServices.Update(enumerator, animationState);
			}
			return;
		}
		if ((bool)GetComponent<Animation>())
		{
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(GetComponent<Animation>());
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				if (!(obj2 is AnimationState))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(AnimationState));
				}
				AnimationState animationState2 = (AnimationState)obj2;
				animationState2.speed = 0f;
				UnityRuntimeServices.Update(enumerator2, animationState2);
			}
		}
		if ((bool)GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Pause();
		}
	}

	public virtual void Main()
	{
	}
}
