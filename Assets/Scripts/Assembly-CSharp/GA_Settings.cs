using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Settings : ScriptableObject
{
	public enum HelpTypes
	{
		None = 0,
		FpsCriticalAndTrackTargetHelp = 1,
		GuiAndTrackTargetHelp = 2,
		IncludeSystemSpecsHelp = 3,
		ProvideCustomUserID = 4
	}

	public enum MessageTypes
	{
		None = 0,
		Error = 1,
		Info = 2,
		Warning = 3
	}

	public struct HelpInfo
	{
		public string Message;

		public MessageTypes MsgType;

		public HelpTypes HelpType;
	}

	public enum InspectorStates
	{
		Basic = 0,
		QA = 1,
		Debugging = 2,
		Data = 3,
		Pref = 4
	}

	[HideInInspector]
	public static string VERSION = "0.4.6";

	public int TotalMessagesSubmitted;

	public int TotalMessagesFailed;

	public int DesignMessagesSubmitted;

	public int DesignMessagesFailed;

	public int QualityMessagesSubmitted;

	public int QualityMessagesFailed;

	public int BusinessMessagesSubmitted;

	public int BusinessMessagesFailed;

	public int UserMessagesSubmitted;

	public int UserMessagesFailed;

	public string CustomArea = string.Empty;

	public Transform TrackTarget;

	[SerializeField]
	public string GameKey = string.Empty;

	[SerializeField]
	public string SecretKey = string.Empty;

	[SerializeField]
	public string ApiKey = string.Empty;

	[SerializeField]
	public string Build = "0.1";

	public bool DebugMode = true;

	public bool SendExampleGameDataToMyGame;

	public bool RunInEditorPlayMode = true;

	public bool AllowRoaming;

	public bool ArchiveData;

	public bool NewSessionOnResume = true;

	public Vector3 HeatmapGridSize = Vector3.one;

	public long ArchiveMaxFileSize = 2000L;

	public bool CustomUserID;

	public float SubmitInterval = 10f;

	public bool InternetConnectivity;

	public InspectorStates CurrentInspectorState;

	public List<HelpTypes> ClosedHints = new List<HelpTypes>();

	public bool DisplayHints;

	public Vector2 DisplayHintsScrollState;

	public Texture2D Logo;

	public List<HelpInfo> GetHelpMessageList()
	{
		List<HelpInfo> list = new List<HelpInfo>();
		if (GameKey.Equals(string.Empty) || SecretKey.Equals(string.Empty))
		{
			list.Add(new HelpInfo
			{
				Message = "Please fill in your Game Key and Secret Key, obtained from the GameAnalytics website where you created your game.",
				MsgType = MessageTypes.Warning
			});
		}
		if (Build.Equals(string.Empty))
		{
			list.Add(new HelpInfo
			{
				Message = "Please fill in a name for your build, representing the current version of the game. Updating the build name for each version of the game will allow you to filter by build when viewing your data on the GA website.",
				MsgType = MessageTypes.Warning
			});
		}
		if (CustomUserID && !ClosedHints.Contains(HelpTypes.ProvideCustomUserID))
		{
			list.Add(new HelpInfo
			{
				Message = "You have indicated that you will provide a custom user ID - no data will be submitted until it is provided. This should be defined from code through: GA.Settings.SetCustomUserID",
				MsgType = MessageTypes.Info,
				HelpType = HelpTypes.ProvideCustomUserID
			});
		}
		return list;
	}

	public HelpInfo GetHelpMessage()
	{
		if (GameKey.Equals(string.Empty) || SecretKey.Equals(string.Empty))
		{
			HelpInfo result = default(HelpInfo);
			result.Message = "Please fill in your Game Key and Secret Key, obtained from the GameAnalytics website where you created your game.";
			result.MsgType = MessageTypes.Warning;
			return result;
		}
		if (Build.Equals(string.Empty))
		{
			HelpInfo result2 = default(HelpInfo);
			result2.Message = "Please fill in a name for your build, representing the current version of the game. Updating the build name for each version of the game will allow you to filter by build when viewing your data on the GA website.";
			result2.MsgType = MessageTypes.Warning;
			return result2;
		}
		if (CustomUserID && !ClosedHints.Contains(HelpTypes.ProvideCustomUserID))
		{
			HelpInfo result3 = default(HelpInfo);
			result3.Message = "You have indicated that you will provide a custom user ID - no data will be submitted until it is provided. This should be defined from code through: GA.Settings.SetCustomUserID";
			result3.MsgType = MessageTypes.Info;
			result3.HelpType = HelpTypes.ProvideCustomUserID;
			return result3;
		}
		HelpInfo result4 = default(HelpInfo);
		result4.Message = "No hints to display. The \"Reset Hints\" button resets closed hints.";
		return result4;
	}

	public IEnumerator CheckInternetConnectivity(bool startQueue)
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork && !GA.SettingsGA.AllowRoaming)
		{
			InternetConnectivity = false;
		}
		else
		{
			WWW www = new WWW(GA.API.Submit.GetBaseURL(true) + "/ping");
			yield return www;
			try
			{
				if (GA.API.Submit.CheckServerReply(www))
				{
					InternetConnectivity = true;
				}
				else if (www.error != null)
				{
					InternetConnectivity = false;
				}
				else
				{
					Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
					if (returnParam != null && returnParam.ContainsKey("status") && returnParam["status"].ToString().Equals("ok"))
					{
						InternetConnectivity = true;
					}
					else
					{
						InternetConnectivity = false;
					}
				}
			}
			catch
			{
				InternetConnectivity = false;
			}
		}
		if (startQueue)
		{
			if (InternetConnectivity)
			{
				GA.Log("GA initialized, waiting for events..");
			}
			else
			{
				GA.Log("GA detects no internet connection..");
			}
			AddUniqueIDs();
			GA.RunCoroutine(GA_Queue.SubmitQueue());
			GA.Log("GameAnalytics: Submission queue started.");
		}
	}

	private void AddUniqueIDs()
	{
		string empty = string.Empty;
		string[] array = SystemInfo.operatingSystem.Split(' ');
		if (array.Length > 0)
		{
			empty = array[0];
		}
	}

	public string GetUniqueIDiOS()
	{
		return string.Empty;
	}

	public void SetCustomUserID(string customID)
	{
		if (customID != string.Empty)
		{
			GA.API.GenericInfo.SetCustomUserID(customID);
		}
	}

	public void SetCustomArea(string customArea)
	{
		CustomArea = customArea;
	}
}
