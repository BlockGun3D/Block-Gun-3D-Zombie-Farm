using System;
using UnityEngine;

[Serializable]
public class StarManager : MonoBehaviour
{
	public int level;

	public int mission;

	public GUITexture star1;

	public GUITexture star1Empty;

	public GUITexture star2;

	public GUITexture star2Empty;

	public GUITexture star3;

	public GUITexture star3Empty;

	public GUITexture box1;

	public GUITexture box2;

	public GUITexture box3;

	public GUIText normal;

	public GUIText hard;

	public GUIText brutal;

	private int dif;

	public virtual void OnEnable()
	{
		dif = Global.gm.GetDificultyBeat(level, mission);
		if (dif == 0)
		{
			star1.enabled = false;
			star2.enabled = false;
			star3.enabled = false;
			star1Empty.enabled = true;
			star2Empty.enabled = true;
			star3Empty.enabled = true;
			SelectDifficulty(1);
		}
		else if (dif == 1)
		{
			star1.enabled = true;
			star2.enabled = false;
			star3.enabled = false;
			star1Empty.enabled = false;
			star2Empty.enabled = true;
			star3Empty.enabled = true;
			SelectDifficulty(2);
		}
		else if (dif == 2)
		{
			star1.enabled = true;
			star2.enabled = true;
			star3.enabled = false;
			star1Empty.enabled = false;
			star2Empty.enabled = false;
			star3Empty.enabled = true;
			SelectDifficulty(3);
		}
		else if (dif == 3)
		{
			star1.enabled = true;
			star2.enabled = true;
			star3.enabled = true;
			star1Empty.enabled = false;
			star2Empty.enabled = false;
			star3Empty.enabled = false;
			SelectDifficulty(3);
		}
	}

	public virtual void SelectDifficulty(int difficulty)
	{
		dif = difficulty;
		if (dif == 1)
		{
			box1.enabled = true;
			box2.enabled = false;
			box3.enabled = false;
			normal.enabled = true;
			hard.enabled = false;
			brutal.enabled = false;
			Global.difficulty = 1;
		}
		else if (dif == 2)
		{
			box1.enabled = false;
			box2.enabled = true;
			box3.enabled = false;
			normal.enabled = false;
			hard.enabled = true;
			brutal.enabled = false;
			Global.difficulty = 2;
		}
		else if (dif == 3)
		{
			box1.enabled = false;
			box2.enabled = false;
			box3.enabled = true;
			normal.enabled = false;
			hard.enabled = false;
			brutal.enabled = true;
			Global.difficulty = 3;
		}
	}

	public virtual int GetDifficulty()
	{
		return dif;
	}

	public virtual void Main()
	{
	}
}
