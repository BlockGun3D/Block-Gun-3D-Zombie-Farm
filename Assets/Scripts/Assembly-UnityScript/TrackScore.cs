using System;
using UnityEngine;

[Serializable]
public class TrackScore : MonoBehaviour
{
	public virtual void Update()
	{
		int score = Global.gm.GetScore();
		string text = string.Empty + score;
		if (score < 10)
		{
			text = "00000" + text;
		}
		else if (score < 100)
		{
			text = "0000" + text;
		}
		else if (score < 1000)
		{
			text = "000" + text;
		}
		else if (score < 10000)
		{
			text = "00" + text;
		}
		else if (score < 100000)
		{
			text = "0" + text;
		}
		GetComponent<GUIText>().text = text;
	}

	public virtual void Main()
	{
	}
}
