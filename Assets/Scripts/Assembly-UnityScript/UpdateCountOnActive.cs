using System;
using UnityEngine;

[Serializable]
public class UpdateCountOnActive : MonoBehaviour
{
	public BlockType blockType;

	public virtual void Update()
	{
		GetComponent<GUIText>().text = string.Empty + Global.gm.GetTotalBlockCount(blockType);
	}

	public virtual void Main()
	{
	}
}
