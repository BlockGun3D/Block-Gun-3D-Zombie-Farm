using System;
using UnityEngine;

[Serializable]
public class UpgradeManager : MonoBehaviour
{
	public GameObject tapjoyPrefab;

	public GUITexture[] upgradeBars;

	public GUITexture[] gunImages;

	public GUIText[] gunUpgradeCostText;

	public GUITexture[] gunColorCubes;

	public GUITexture[] gunSilverCubes;

	public GUITexture[] lockImages;

	public GUIText armorUpgradeCostText;

	public GUITexture armorColorCube;

	public GUITexture armorSilverCube;

	public GUITexture armorUpgradeBar;

	public GUIText luckUpgradeCostText;

	public GUITexture luckColorCube;

	public GUITexture luckSilverCube;

	public GUITexture luckUpgradeBar;

	private int[,] upgradeCost;

	private BlockType[,] blockCostType;

	private int[] armorUpgradeCost;

	private BlockType[] armorBlockCostType;

	private int[] luckUpgradeCost;

	private BlockType[] luckBlockCostType;

	private int NUM_LEVELS;

	private float gunBarWidth;

	private float armorBarWidth;

	private float luckBarWidth;

	public UpgradeManager()
	{
		upgradeBars = new GUITexture[4];
		gunImages = new GUITexture[4];
		gunUpgradeCostText = new GUIText[4];
		gunColorCubes = new GUITexture[4];
		gunSilverCubes = new GUITexture[4];
		lockImages = new GUITexture[4];
		NUM_LEVELS = 4;
	}

	public virtual void Start()
	{
		gunBarWidth = upgradeBars[0].pixelInset.width;
		armorBarWidth = (luckBarWidth = armorUpgradeBar.pixelInset.width);
		upgradeCost = (int[,])Array.CreateInstance(typeof(int), new int[2] { 4, NUM_LEVELS });
		blockCostType = (BlockType[,])Array.CreateInstance(typeof(BlockType), new int[2] { 4, NUM_LEVELS });
		armorUpgradeCost = new int[3];
		armorBlockCostType = new BlockType[3];
		luckUpgradeCost = new int[3];
		luckBlockCostType = new BlockType[3];
		armorUpgradeCost[0] = 0;
		armorUpgradeCost[1] = 800;
		armorUpgradeCost[2] = 20;
		armorBlockCostType[0] = BlockType.GREEN;
		armorBlockCostType[1] = BlockType.GREEN;
		armorBlockCostType[2] = BlockType.SILVER;
		luckUpgradeCost[0] = 0;
		luckUpgradeCost[1] = 800;
		luckUpgradeCost[2] = 20;
		luckBlockCostType[0] = BlockType.GREEN;
		luckBlockCostType[1] = BlockType.GREEN;
		luckBlockCostType[2] = BlockType.SILVER;
		upgradeCost[0, 0] = 0;
		upgradeCost[0, 1] = 400;
		upgradeCost[0, 2] = 600;
		upgradeCost[0, 3] = 10;
		blockCostType[0, 0] = BlockType.GREEN;
		blockCostType[0, 1] = BlockType.GREEN;
		blockCostType[0, 2] = BlockType.GREEN;
		blockCostType[0, 3] = BlockType.SILVER;
		upgradeCost[3, 0] = 2000;
		upgradeCost[3, 1] = 1000;
		upgradeCost[3, 2] = 1200;
		upgradeCost[3, 3] = 20;
		blockCostType[3, 0] = BlockType.GREEN;
		blockCostType[3, 1] = BlockType.GREEN;
		blockCostType[3, 2] = BlockType.GREEN;
		blockCostType[3, 3] = BlockType.SILVER;
		upgradeCost[2, 0] = 1500;
		upgradeCost[2, 1] = 800;
		upgradeCost[2, 2] = 1000;
		upgradeCost[2, 3] = 15;
		blockCostType[2, 0] = BlockType.GREEN;
		blockCostType[2, 1] = BlockType.GREEN;
		blockCostType[2, 2] = BlockType.GREEN;
		blockCostType[2, 3] = BlockType.SILVER;
		upgradeCost[1, 0] = 0;
		upgradeCost[1, 1] = 400;
		upgradeCost[1, 2] = 700;
		upgradeCost[1, 3] = 12;
		blockCostType[1, 0] = BlockType.GREEN;
		blockCostType[1, 1] = BlockType.GREEN;
		blockCostType[1, 2] = BlockType.GREEN;
		blockCostType[1, 3] = BlockType.SILVER;
	}

	public virtual void SetupUpgradesGUI(bool activated)
	{
		if (activated)
		{
			GunGUIUpdate(GunType.PISTOL);
			GunGUIUpdate(GunType.AK47);
			GunGUIUpdate(GunType.UZI);
			GunGUIUpdate(GunType.SHOTGUN);
			ArmorGUIUpdate();
			LuckGUIUpdate();
		}
	}

	public virtual void PistolUpgradePressed()
	{
		GunUpgradePressed(GunType.PISTOL);
	}

	public virtual void Ak47UpgradePressed()
	{
		GunUpgradePressed(GunType.AK47);
	}

	public virtual void UziUpgradePressed()
	{
		GunUpgradePressed(GunType.UZI);
	}

	public virtual void ShotgunUpgradePressed()
	{
		GunUpgradePressed(GunType.SHOTGUN);
	}

	public virtual void GunUpgradePressed(GunType type)
	{
		int gunLevel = Global.gm.GetGunLevel(type);
		if (gunLevel == NUM_LEVELS)
		{
			return;
		}
		BlockType blockType = blockCostType[(int)type, gunLevel];
		int totalBlockCount = Global.gm.GetTotalBlockCount(blockType);
		int num = upgradeCost[(int)type, gunLevel];
		if (totalBlockCount >= num)
		{
			Global.gm.SubtractBlocksFromTotal(num, blockType);
			if ((bool)tapjoyPrefab && blockType == BlockType.GREEN)
			{
				tapjoyPrefab.SendMessage("SpendTJPoints", num);
			}
			Global.gm.IncreaseGunLevel(type);
			GunGUIUpdate(type);
			GetComponent<AudioSource>().Play();
		}
		else
		{
			Global.gm.SetGameState(GameState.GET_BLOCKS_QUESTION_MENU);
		}
	}

	public virtual void GunGUIUpdate(GunType type)
	{
		int gunLevel = Global.gm.GetGunLevel(type);
		if (gunLevel == NUM_LEVELS)
		{
			float num = gunBarWidth;
			Rect pixelInset = upgradeBars[(int)type].pixelInset;
			float num3 = (pixelInset.width = num);
			Rect rect2 = (upgradeBars[(int)type].pixelInset = pixelInset);
			gunUpgradeCostText[(int)type].enabled = false;
			float a = 0.5f;
			Color color = gunImages[(int)type].color;
			float num4 = (color.a = a);
			Color color3 = (gunImages[(int)type].color = color);
			lockImages[(int)type].enabled = false;
			gunSilverCubes[(int)type].enabled = false;
			gunColorCubes[(int)type].enabled = false;
			return;
		}
		BlockType blockType = blockCostType[(int)type, gunLevel];
		int num5 = upgradeCost[(int)type, gunLevel];
		float num6 = gunBarWidth * (float)gunLevel / (float)NUM_LEVELS;
		Rect pixelInset2 = upgradeBars[(int)type].pixelInset;
		float num8 = (pixelInset2.width = num6);
		Rect rect4 = (upgradeBars[(int)type].pixelInset = pixelInset2);
		gunUpgradeCostText[(int)type].text = string.Empty + num5;
		if (gunLevel == 0)
		{
			float a2 = 0.35f;
			Color color4 = gunImages[(int)type].color;
			float num9 = (color4.a = a2);
			Color color6 = (gunImages[(int)type].color = color4);
			lockImages[(int)type].enabled = true;
		}
		else
		{
			float a3 = 0.5f;
			Color color7 = gunImages[(int)type].color;
			float num10 = (color7.a = a3);
			Color color9 = (gunImages[(int)type].color = color7);
			lockImages[(int)type].enabled = false;
		}
		if (blockCostType[(int)type, gunLevel] == BlockType.SILVER)
		{
			gunSilverCubes[(int)type].enabled = true;
			gunColorCubes[(int)type].enabled = false;
		}
		else
		{
			gunSilverCubes[(int)type].enabled = false;
			gunColorCubes[(int)type].enabled = true;
		}
	}

	public virtual void ArmorUpgradePressed()
	{
		int armorLevel = Global.gm.GetArmorLevel();
		if (armorLevel == 3)
		{
			return;
		}
		BlockType blockType = armorBlockCostType[armorLevel];
		int totalBlockCount = Global.gm.GetTotalBlockCount(blockType);
		int num = armorUpgradeCost[armorLevel];
		if (totalBlockCount >= num)
		{
			Global.gm.SubtractBlocksFromTotal(num, blockType);
			if ((bool)tapjoyPrefab && blockType == BlockType.GREEN)
			{
				tapjoyPrefab.SendMessage("SpendTJPoints", num);
			}
			Global.gm.IncreaseArmorLevel();
			ArmorGUIUpdate();
			GetComponent<AudioSource>().Play();
		}
		else
		{
			Global.gm.SetGameState(GameState.GET_BLOCKS_QUESTION_MENU);
		}
	}

	public virtual void ArmorGUIUpdate()
	{
		int armorLevel = Global.gm.GetArmorLevel();
		if (armorLevel == 3)
		{
			float num = armorBarWidth;
			Rect pixelInset = armorUpgradeBar.pixelInset;
			float num3 = (pixelInset.width = num);
			Rect rect2 = (armorUpgradeBar.pixelInset = pixelInset);
			armorUpgradeCostText.enabled = false;
			armorSilverCube.enabled = false;
			armorColorCube.enabled = false;
			return;
		}
		BlockType blockType = armorBlockCostType[armorLevel];
		int num4 = armorUpgradeCost[armorLevel];
		float num5 = armorBarWidth * (float)armorLevel / 3f;
		Rect pixelInset2 = armorUpgradeBar.pixelInset;
		float num7 = (pixelInset2.width = num5);
		Rect rect4 = (armorUpgradeBar.pixelInset = pixelInset2);
		armorUpgradeCostText.text = string.Empty + num4;
		if (armorBlockCostType[armorLevel] == BlockType.SILVER)
		{
			armorSilverCube.enabled = true;
			armorColorCube.enabled = false;
		}
		else
		{
			armorSilverCube.enabled = false;
			armorColorCube.enabled = true;
		}
	}

	public virtual void LuckUpgradePressed()
	{
		int luckLevel = Global.gm.GetLuckLevel();
		if (luckLevel == 3)
		{
			return;
		}
		BlockType blockType = luckBlockCostType[luckLevel];
		int totalBlockCount = Global.gm.GetTotalBlockCount(blockType);
		int num = luckUpgradeCost[luckLevel];
		if (totalBlockCount >= num)
		{
			Global.gm.SubtractBlocksFromTotal(num, blockType);
			if ((bool)tapjoyPrefab && blockType == BlockType.GREEN)
			{
				tapjoyPrefab.SendMessage("SpendTJPoints", num);
			}
			Global.gm.IncreaseLuckLevel();
			LuckGUIUpdate();
			GetComponent<AudioSource>().Play();
		}
		else
		{
			Global.gm.SetGameState(GameState.GET_BLOCKS_QUESTION_MENU);
		}
	}

	public virtual void LuckGUIUpdate()
	{
		int luckLevel = Global.gm.GetLuckLevel();
		if (luckLevel == 3)
		{
			float num = luckBarWidth;
			Rect pixelInset = luckUpgradeBar.pixelInset;
			float num3 = (pixelInset.width = num);
			Rect rect2 = (luckUpgradeBar.pixelInset = pixelInset);
			luckUpgradeCostText.enabled = false;
			luckSilverCube.enabled = false;
			luckColorCube.enabled = false;
			return;
		}
		BlockType blockType = luckBlockCostType[luckLevel];
		int num4 = luckUpgradeCost[luckLevel];
		float num5 = luckBarWidth * (float)luckLevel / 3f;
		Rect pixelInset2 = luckUpgradeBar.pixelInset;
		float num7 = (pixelInset2.width = num5);
		Rect rect4 = (luckUpgradeBar.pixelInset = pixelInset2);
		luckUpgradeCostText.text = string.Empty + num4;
		if (luckBlockCostType[luckLevel] == BlockType.SILVER)
		{
			luckSilverCube.enabled = true;
			luckColorCube.enabled = false;
		}
		else
		{
			luckSilverCube.enabled = false;
			luckColorCube.enabled = true;
		}
	}

	public virtual void Main()
	{
	}
}
