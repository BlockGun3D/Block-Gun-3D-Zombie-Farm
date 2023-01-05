using System;
using UnityEngine;

[Serializable]
public class UpgradeCountManager : MonoBehaviour
{
	public GUITexture alertGui;

	public GUIText count;

	public virtual void Activate()
	{
		int upgradesLeft = Global.gm.GetUpgradesLeft();
		if (upgradesLeft == 0)
		{
			count.text = string.Empty;
			alertGui.enabled = false;
		}
		else
		{
			count.text = string.Empty + upgradesLeft;
		}
	}

	public virtual void Deactivate()
	{
		gameObject.SetActive(false);
	}

	public virtual void Main()
	{
	}
}
