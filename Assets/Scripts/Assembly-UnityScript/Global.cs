using System;
using UnityEngine;

[Serializable]
public class Global : MonoBehaviour
{
	[NonSerialized]
	public static Vector2 defaultScreen = new Vector2(480f, 320f);

	[NonSerialized]
	public static GameManager gm;

	[NonSerialized]
	public static PlayerController pm;

	[NonSerialized]
	public static WeaponManager wm;

	[NonSerialized]
	public static bool storeRetrieved;

	[NonSerialized]
	public static bool bannerShowing;

	[NonSerialized]
	public static GameObject tapPref;

	[NonSerialized]
	public static int adNetworkChoose;

	[NonSerialized]
	public static int levelNum;

	[NonSerialized]
	public static int missionNum;

	[NonSerialized]
	public static ObjectiveType objective;

	[NonSerialized]
	public static short difficulty;

	[NonSerialized]
	public static AudioSource levelMusic;

	[NonSerialized]
	public static Vector3 playerStartPos;

	[NonSerialized]
	public static Quaternion playerStartRot;

	[NonSerialized]
	public static bool popupEnabled;

	public PlayerController playerController;

	public WeaponManager weaponManager;

	public FirstPersonControlCustom fpsController;

	public GameObject tapjoyPrefab;

	public string firstLevelToLoad;

	public Global()
	{
		firstLevelToLoad = "level_0";
	}

	public virtual void Awake()
	{
		Application.targetFrameRate = 60;
		gm = GameManager.GetInstance();
		pm = playerController;
		wm = weaponManager;
		gm.LoadGameData();
		gm.SetFPSController(fpsController);
		if ((bool)tapjoyPrefab)
		{
			tapPref = tapjoyPrefab;
		}
		Application.LoadLevel(firstLevelToLoad);
		gm.openedCount++;
	}

	public virtual void OnApplicationPause(bool pauseStatus)
	{
		Debug.Log("MATT: OnApplicationPause - pauseStatus: " + pauseStatus);
		if (gm == null)
		{
			return;
		}
		if (!pauseStatus)
		{
			gm.SaveGameData();
			return;
		}
		gm.LoadGameData();
		if ((bool)tapPref)
		{
			tapPref.SendMessage("GetTJPoints");
		}
		if (gm.GetGameState() == GameState.PLAYING)
		{
			gm.SetGameState(GameState.PAUSED);
		}
	}

	public virtual void OnApplicationFocus(bool focusStatus)
	{
		Debug.Log("MATT: OnApplicationFocus - pauseStatus: " + focusStatus);
		if (gm == null)
		{
			return;
		}
		if (focusStatus)
		{
			gm.SaveGameData();
			return;
		}
		gm.LoadGameData();
		if ((bool)tapPref)
		{
			tapPref.SendMessage("GetTJPoints");
		}
		if (gm.GetGameState() == GameState.PLAYING)
		{
			gm.SetGameState(GameState.PAUSED);
		}
	}

	public virtual void OnApplicationQuit()
	{
		gm.SaveGameData();
	}

	public virtual void Update()
	{
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Screen.lockCursor = !Screen.lockCursor;
        }
        GameState gameState = gm.GetGameState();
		if (Input.GetKeyDown("escape"))
		{
			switch (gameState)
			{
			case GameState.START:
				Application.Quit();
				break;
			case GameState.PLAYING:
			case GameState.PAUSED_UPGRADES:
				gm.SetGameState(GameState.PAUSED);
				break;
			case GameState.PAUSED:
				gm.SetGameState(GameState.PLAYING);
				break;
			case GameState.TUTORIAL_LOOK:
			case GameState.TUTORIAL_MOVE:
			case GameState.TUTORIAL_FIRE:
			case GameState.TUTORIAL_RELOAD:
			case GameState.MENU_MAIN:
				gm.SetGameState(GameState.START);
				break;
			case GameState.SAVEME:
			case GameState.GET_BLOCKS_QUESTION:
				gm.SetGameState(GameState.DIED);
				break;
			case GameState.DIED:
			case GameState.MENU_UPGRADES:
				gm.SetGameState(GameState.MENU_MAIN);
				break;
			default:
				gm.SetGameState(GameState.LAST_STATE);
				break;
			}
		}
	}

	public static void ResetAndChangeState(GameState state)
	{
		pm.Reset();
		gm.SetReviveCost(1);
		gm.ResetLevelBlockCount();
		gm.ResetScore();
		gm.SetGameState(state);
	}

	public static void SaveGameData()
	{
		gm.SaveGameData();
		if ((bool)tapPref)
		{
			tapPref.SendMessage("GetTJPoints");
		}
	}

	public virtual void Main()
	{
	}
}
