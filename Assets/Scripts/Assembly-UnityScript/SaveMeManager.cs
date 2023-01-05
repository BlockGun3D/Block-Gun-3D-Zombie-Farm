using System;
using UnityEngine;

[Serializable]
public class SaveMeManager : MonoBehaviour
{
	public GUIText costText;

	public PlayerController thePlayer;

	public int deathDistance;

	public VungleExample vunglePlugin;

	public GameObject freeSaveButton;

	private int timeActivated;

	[NonSerialized]
	public static bool saveMePressed;

	public SaveMeManager()
	{
		deathDistance = 5;
		timeActivated = -1;
	}

	public virtual void SaveMe(bool activating)
	{
		if (activating)
		{
			timeActivated = (int)Time.time;
			costText.text = string.Empty + Global.gm.GetReviveCost();
			if (Global.gm.GetReviveCost() == 1 && vunglePlugin.IsAdAvailable())
			{
				freeSaveButton.SetActive(true);
			}
			else
			{
				freeSaveButton.SetActive(false);
			}
		}
		else
		{
			timeActivated = -1;
			saveMePressed = false;
		}
	}

	public virtual void Update()
	{
		if (timeActivated >= 0 && !saveMePressed && !(Time.time - (float)timeActivated <= 4.5f))
		{
			Global.gm.SetGameState(GameState.DIED);
			Global.gm.SetReviveCost(1);
		}
		if (freeSaveButton.active && Global.gm.GetReviveCost() != 1)
		{
			freeSaveButton.SetActive(false);
		}
	}

	public virtual void SaveMePressed()
	{
		saveMePressed = true;
		int totalBlockCount = Global.gm.GetTotalBlockCount(BlockType.SILVER);
		if (totalBlockCount >= Global.gm.GetReviveCost())
		{
			Global.gm.SubtractBlocksFromTotal(Global.gm.GetReviveCost(), BlockType.SILVER);
			Global.gm.IncrementReviveCost();
			thePlayer.ResetHealth();
			GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
			int i = 0;
			GameObject[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				if (!((thePlayer.transform.position - array2[i].transform.position).magnitude >= (float)deathDistance))
				{
					array2[i].SendMessage("DieIfNotBoss");
				}
			}
			Global.gm.SetGameState(GameState.PLAYING);
		}
		else
		{
			Global.gm.SetGameState(GameState.GET_BLOCKS_QUESTION);
		}
	}

	public virtual void FreeSaveButtonPressed()
	{
		saveMePressed = true;
		Global.SaveGameData();
		vunglePlugin.DisplayInsentiveAd();
	}

	public virtual void InsentiveAdFinished()
	{
		Global.gm.AddBlocksToTotal(1, BlockType.SILVER);
		SaveMePressed();
		freeSaveButton.SetActive(false);
	}

	public virtual void Main()
	{
	}
}
