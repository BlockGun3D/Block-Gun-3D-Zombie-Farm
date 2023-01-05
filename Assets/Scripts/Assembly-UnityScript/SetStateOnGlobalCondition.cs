using System;
using UnityEngine;

[Serializable]
public class SetStateOnGlobalCondition : MonoBehaviour
{
	public virtual void DoneTutorialStateSet()
	{
		if (Global.gm.IsTutorialDone())
		{
			Global.gm.SetGameState(GameState.PLAYING);
		}
		else
		{
			Global.gm.SetGameState(GameState.TUTORIAL_LOOK);
		}
	}

	public virtual void Main()
	{
	}
}
