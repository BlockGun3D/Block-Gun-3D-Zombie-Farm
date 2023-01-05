using System;
using UnityEngine;

[Serializable]
public class StopMusic : MonoBehaviour
{
	public bool onlyStartOnOneState;

	public GameState stateToStart;

	public virtual void MusicPlay(bool start)
	{
		if (start)
		{
			if (!onlyStartOnOneState)
			{
				GetComponent<AudioSource>().Play();
			}
			else if (stateToStart == Global.gm.GetGameState())
			{
				GetComponent<AudioSource>().Play();
			}
		}
		else
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	public virtual void Main()
	{
	}
}
