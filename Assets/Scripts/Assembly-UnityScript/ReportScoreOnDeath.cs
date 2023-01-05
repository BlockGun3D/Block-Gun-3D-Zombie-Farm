using System;
using UnityEngine;

[Serializable]
public class ReportScoreOnDeath : MonoBehaviour
{
	public int scoreValue;

	public virtual void Die()
	{
		Global.gm.AddToScore(scoreValue);
	}

	public virtual void Main()
	{
	}
}
