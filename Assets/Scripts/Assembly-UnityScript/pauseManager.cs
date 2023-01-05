using System;
using UnityEngine;

[Serializable]
public class pauseManager : MonoBehaviour
{
	private float wentPausedTime;

	public pauseManager()
	{
		wentPausedTime = -1f;
	}

	public virtual void PauseStatus(bool wentPaused)
	{
		if (wentPaused)
		{
			if (!(wentPausedTime >= 0f))
			{
				wentPausedTime = Time.time;
			}
			return;
		}
		GameState gameState = Global.gm.GetGameState();
		if (gameState == GameState.PLAYING)
		{
			Global.gm.SubtractPauseTime(Time.time - wentPausedTime);
			wentPausedTime = -1f;
		}
	}

	public virtual void Main()
	{
	}
}
