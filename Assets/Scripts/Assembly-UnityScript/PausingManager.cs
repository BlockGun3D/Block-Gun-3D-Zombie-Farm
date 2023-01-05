using System;
using UnityEngine;

[Serializable]
public class PausingManager : MonoBehaviour
{
	public AudioSource[] audioToPause;

	public Animation[] animationsToPause;

	private float timePaused;

	public virtual void PauseGamePlay(bool pausing)
	{
		AudioSource levelMusic = Global.levelMusic;
		if (pausing)
		{
			timePaused = Time.time;
			if ((bool)levelMusic && levelMusic.isPlaying)
			{
				levelMusic.Pause();
			}
			int i = 0;
			AudioSource[] array = audioToPause;
			for (int length = array.Length; i < length; i++)
			{
				if (array[i].gameObject.active)
				{
					array[i].Pause();
				}
			}
			int j = 0;
			Animation[] array2 = animationsToPause;
			for (int length2 = array2.Length; j < length2; j++)
			{
				if (array2[j].gameObject.active)
				{
					array2[j].enabled = false;
				}
			}
		}
		else
		{
			Global.gm.SubtractPauseTime(Time.time - timePaused);
			if ((bool)levelMusic && Global.gm.GetGameState() == GameState.PLAYING)
			{
				levelMusic.Play();
			}
		}
		int k = 0;
		AudioSource[] array3 = audioToPause;
		for (int length3 = array3.Length; k < length3; k++)
		{
			if (array3[k].gameObject.active)
			{
				array3[k].Play();
			}
		}
		int l = 0;
		Animation[] array4 = animationsToPause;
		for (int length4 = array4.Length; l < length4; l++)
		{
			if (array4[l].gameObject.active)
			{
				array4[l].enabled = true;
			}
		}
	}

	public virtual void StartLevelTimer(bool start)
	{
		AudioSource levelMusic = Global.levelMusic;
		if (start)
		{
			Global.gm.setTimeLevelStarted(Time.time);
			if ((bool)levelMusic)
			{
				levelMusic.Stop();
				levelMusic.Play();
			}
		}
	}

	public virtual void Main()
	{
	}
}
