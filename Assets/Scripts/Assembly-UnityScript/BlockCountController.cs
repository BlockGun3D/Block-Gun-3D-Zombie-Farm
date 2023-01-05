using System;
using UnityEngine;

[Serializable]
public class BlockCountController : MonoBehaviour
{
	public bool localCount;

	public BlockType blockColor;

	private GameManager gm;

	private int num;

	public BlockCountController()
	{
		localCount = true;
	}

	public virtual void Start()
	{
		gm = Global.gm;
	}

	public virtual void Update()
	{
		UpdateGameplay();
	}

	public virtual void UpdateGameplay()
	{
		int levelBlockCount = gm.GetLevelBlockCount(blockColor);
		if (levelBlockCount != num)
		{
			GetComponent<Animation>().Play();
			num = levelBlockCount;
			GetComponent<GUIText>().text = string.Empty + num;
			num = levelBlockCount;
		}
	}

	public virtual void Main()
	{
	}
}
