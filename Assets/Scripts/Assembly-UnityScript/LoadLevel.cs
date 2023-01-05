using System;
using UnityEngine;

[Serializable]
public class LoadLevel : MonoBehaviour
{
	public string level;

	public LoadLevel()
	{
		level = string.Empty;
	}

	public virtual void LoadLevelVoid()
	{
		Application.LoadLevel(level);
	}

	public virtual void Main()
	{
	}
}
