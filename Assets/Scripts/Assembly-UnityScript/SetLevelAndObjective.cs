using System;
using UnityEngine;

[Serializable]
public class SetLevelAndObjective : MonoBehaviour
{
	public int levelNum;

	public string levelName;

	public ObjectiveType objective;

	public int forceDifficultyLevel;

	public StarManager stars;

	public SetLevelAndObjective()
	{
		levelName = string.Empty;
	}

	public virtual void SetLevelAndObjectiveVoid()
	{
		Global.objective = objective;
		if (forceDifficultyLevel > 0)
		{
			Global.difficulty = (short)forceDifficultyLevel;
		}
		else
		{
			Global.difficulty = (short)stars.GetDifficulty();
		}
		if (Global.levelNum != levelNum)
		{
			Global.levelNum = levelNum;
			Application.LoadLevel(levelName);
		}
		if (!stars)
		{
			Global.missionNum = 0;
		}
		else
		{
			Global.missionNum = stars.mission;
		}
	}

	public virtual void Main()
	{
	}
}
