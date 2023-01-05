using System;
using UnityEngine;

[Serializable]
public class DoneManager : MonoBehaviour
{
	public GUIText scoreText;

	public GUIText blocksText;

	public GUIText timeText;

	public GUIText newBest;

	public AudioSource stopMusic;

	public bool submitScore;

	public GUITexture star2;

	public GUITexture star3;

	public bool showStars;

	public DoneManager()
	{
		submitScore = true;
	}

	public virtual void DoneState(bool done)
	{
		if (!done)
		{
			return;
		}
		scoreText.text = "Score: " + Global.gm.GetScore();
		blocksText.text = "Blocks: " + Global.gm.GetLevelBlockCount(BlockType.GREEN);
		int num = (int)Global.gm.GetTimeInLevel();
		int num2 = num % 60;
		int num3 = num / 60;
		timeText.text = "Time: " + num3 + "m " + num2 + "s";
		if (submitScore)
		{
			if (Global.gm.SubmitScore(Global.levelNum))
			{
				newBest.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				newBest.color = new Color(1f, 1f, 1f, 0f);
			}
		}
		if ((bool)stopMusic)
		{
			stopMusic.Stop();
		}
		if (showStars)
		{
			short difficulty = Global.difficulty;
			switch (difficulty)
			{
			case 1:
				star2.enabled = false;
				star3.enabled = false;
				break;
			case 2:
				star2.enabled = true;
				star3.enabled = false;
				break;
			case 3:
				star2.enabled = true;
				star3.enabled = true;
				break;
			}
			Global.gm.SetDificultyBeat(Global.levelNum, Global.missionNum, difficulty);
		}
		Global.SaveGameData();
	}

	public virtual void Main()
	{
	}
}
