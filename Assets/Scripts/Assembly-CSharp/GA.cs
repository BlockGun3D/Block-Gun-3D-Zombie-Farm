using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GA
{
	public class GA_API
	{
		public GA_Quality Quality = new GA_Quality();

		public GA_Design Design = new GA_Design();

		public GA_Business Business = new GA_Business();

		public GA_GenericInfo GenericInfo = new GA_GenericInfo();

		public GA_Debug Debugging = new GA_Debug();

		public GA_Archive Archive = new GA_Archive();

		public GA_Request Request = new GA_Request();

		public GA_Submit Submit = new GA_Submit();

		public GA_User User = new GA_User();
	}

	public static GA_GameObjectManager _GA_controller;

	private static GA_Settings _settings;

	private static GA_API api = new GA_API();

	[CompilerGenerated]
	private static Func<bool> _003C_003Ef__am_0024cache3;

	public static GA_Settings SettingsGA
	{
		get
		{
			if (_settings == null)
			{
				InitAPI();
			}
			return _settings;
		}
		private set
		{
			_settings = value;
		}
	}

	public static GA_GameObjectManager GA_controller
	{
		get
		{
			if (_GA_controller == null)
			{
				GameObject gameObject = new GameObject("GA_Controller");
				_GA_controller = gameObject.AddComponent<GA_GameObjectManager>();
			}
			return _GA_controller;
		}
		private set
		{
			_GA_controller = value;
		}
	}

	public static GA_API API
	{
		get
		{
			if (SettingsGA == null)
			{
				InitAPI();
			}
			return api;
		}
		private set
		{
		}
	}

	private static void InitAPI()
	{
		try
		{
			_settings = (GA_Settings)Resources.Load("GameAnalytics/GA_Settings", typeof(GA_Settings));
			InitializeQueue();
		}
		catch (Exception ex)
		{
			Debug.Log("Error getting GA_Settings in InitAPI: " + ex.Message);
		}
	}

	private static void InitializeQueue()
	{
		API.Submit.SetupKeys(SettingsGA.GameKey, SettingsGA.SecretKey);
		if (Application.isPlaying)
		{
			RunCoroutine(SettingsGA.CheckInternetConnectivity(true));
		}
	}

	public static void RunCoroutine(IEnumerator routine)
	{
		if (_003C_003Ef__am_0024cache3 == null)
		{
			_003C_003Ef__am_0024cache3 = _003CRunCoroutine_003Em__0;
		}
		RunCoroutine(routine, _003C_003Ef__am_0024cache3);
	}

	public static void RunCoroutine(IEnumerator routine, Func<bool> done)
	{
		if (Application.isPlaying || !Application.isEditor)
		{
			GA_controller.RunCoroutine(routine);
		}
	}

	public static void Log(object msg)
	{
		if (SettingsGA.DebugMode)
		{
			Debug.Log(msg);
		}
	}

	public static void LogWarning(object msg)
	{
		Debug.LogWarning(msg);
	}

	public static void LogError(object msg)
	{
		Debug.LogError(msg);
	}

	[CompilerGenerated]
	private static bool _003CRunCoroutine_003Em__0()
	{
		return true;
	}
}
