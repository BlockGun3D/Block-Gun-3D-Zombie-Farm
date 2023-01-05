using System;
using UnityEngine;

[Serializable]
public class EnemyCounter : MonoBehaviour
{
	private int totalEnemies;

	private int curEnemies;

	public virtual void OnEnable()
	{
		Reset();
	}

	public virtual void Reset()
	{
		totalEnemies = 0;
		curEnemies = 0;
	}

	public virtual void AddEnemies(int num)
	{
		totalEnemies += num;
		curEnemies += num;
		GetComponent<GUIText>().text = string.Empty + curEnemies + "/" + totalEnemies;
	}

	public virtual void EnemyKilled()
	{
		curEnemies--;
		if (curEnemies < 0)
		{
			curEnemies = 0;
		}
		GetComponent<GUIText>().text = string.Empty + curEnemies + "/" + totalEnemies;
	}

	public virtual void Main()
	{
	}
}
