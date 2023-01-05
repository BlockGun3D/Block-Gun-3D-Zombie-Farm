using System;
using UnityEngine;

[Serializable]
public class SetDifficulty : MonoBehaviour
{
	public StarManager diffManager;

	public int difficulty;

	public virtual void SetDifficultyVoid()
	{
		diffManager.SelectDifficulty(difficulty);
	}

	public virtual void Main()
	{
	}
}
