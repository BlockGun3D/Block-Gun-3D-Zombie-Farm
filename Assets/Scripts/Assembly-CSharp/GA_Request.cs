using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GA_Request
{
	public enum RequestType
	{
		GA_GetHeatmapGameInfo = 0,
		GA_GetHeatmapData = 1
	}

	public delegate void SubmitSuccessHandler(RequestType requestType, Hashtable returnParam, SubmitErrorHandler errorEvent);

	public delegate void SubmitErrorHandler(string message);

	[CompilerGenerated]
	private sealed class _003CRequestGameInfo_003Ec__AnonStorey8
	{
		internal WWW www;

		internal bool _003C_003Em__1()
		{
			return www.isDone;
		}
	}

	[CompilerGenerated]
	private sealed class _003CRequestHeatmapData_003Ec__AnonStorey9
	{
		internal WWW www;

		internal bool _003C_003Em__2()
		{
			return www.isDone;
		}
	}

	public Dictionary<RequestType, string> Requests = new Dictionary<RequestType, string>
	{
		{
			RequestType.GA_GetHeatmapGameInfo,
			"game"
		},
		{
			RequestType.GA_GetHeatmapData,
			"heatmap"
		}
	};

	private string _baseURL = "http://data-api.gameanalytics.com";

	public WWW RequestGameInfo(SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		_003CRequestGameInfo_003Ec__AnonStorey8 _003CRequestGameInfo_003Ec__AnonStorey = new _003CRequestGameInfo_003Ec__AnonStorey8();
		string gameKey = GA.SettingsGA.GameKey;
		string text = "game_key=" + gameKey + "&keys=area%7Cevent_id%7Cbuild";
		text = text.Replace(" ", "%20");
		string uRL = GetURL(Requests[RequestType.GA_GetHeatmapGameInfo]);
		uRL = uRL + "/?" + text;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Authorization", GA.API.Submit.CreateMD5Hash(text + GA.SettingsGA.ApiKey));
		_003CRequestGameInfo_003Ec__AnonStorey.www = new WWW(uRL, new byte[1], hashtable);
		GA.RunCoroutine(Request(_003CRequestGameInfo_003Ec__AnonStorey.www, RequestType.GA_GetHeatmapGameInfo, successEvent, errorEvent), _003CRequestGameInfo_003Ec__AnonStorey._003C_003Em__1);
		return _003CRequestGameInfo_003Ec__AnonStorey.www;
	}

	public WWW RequestHeatmapData(List<string> events, string area, string build, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		return RequestHeatmapData(events, area, build, null, null, successEvent, errorEvent);
	}

	public WWW RequestHeatmapData(List<string> events, string area, string build, DateTime? startDate, DateTime? endDate, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		_003CRequestHeatmapData_003Ec__AnonStorey9 _003CRequestHeatmapData_003Ec__AnonStorey = new _003CRequestHeatmapData_003Ec__AnonStorey9();
		string gameKey = GA.SettingsGA.GameKey;
		string text = string.Empty;
		for (int i = 0; i < events.Count; i++)
		{
			text = ((i != events.Count - 1) ? (text + events[i] + "|") : (text + events[i]));
		}
		string text2 = "game_key=" + gameKey + "&event_ids=" + text + "&area=" + area;
		if (!build.Equals(string.Empty))
		{
			text2 = text2 + "&build=" + build;
		}
		text2 = text2.Replace(" ", "%20");
		if (startDate.HasValue && endDate.HasValue)
		{
			DateTime dateTime = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 0, 0, 0);
			DateTime dateTime2 = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 0, 0, 0);
			string text3 = text2;
			text2 = text3 + "&start_ts=" + DateTimeToUnixTimestamp(dateTime) + "&end_ts=" + DateTimeToUnixTimestamp(dateTime2);
		}
		string uRL = GetURL(Requests[RequestType.GA_GetHeatmapData]);
		uRL = uRL + "/?" + text2;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Authorization", GA.API.Submit.CreateMD5Hash(text2 + GA.SettingsGA.ApiKey));
		_003CRequestHeatmapData_003Ec__AnonStorey.www = new WWW(uRL, new byte[1], hashtable);
		GA.RunCoroutine(Request(_003CRequestHeatmapData_003Ec__AnonStorey.www, RequestType.GA_GetHeatmapData, successEvent, errorEvent), _003CRequestHeatmapData_003Ec__AnonStorey._003C_003Em__2);
		return _003CRequestHeatmapData_003Ec__AnonStorey.www;
	}

	public static double DateTimeToUnixTimestamp(DateTime dateTime)
	{
		return (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
	}

	private IEnumerator Request(WWW www, RequestType requestType, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		yield return www;
		GA.Log("GameAnalytics: URL " + www.url);
		try
		{
			if (www.error != null)
			{
				throw new Exception(www.error);
			}
			string text = www.text;
			text = text.Replace("null", "0");
			Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(text);
			if (returnParam != null)
			{
				GA.Log("GameAnalytics: Result: " + text);
				if (successEvent != null)
				{
					successEvent(requestType, returnParam, errorEvent);
				}
				yield break;
			}
			throw new Exception(text);
		}
		catch (Exception e)
		{
			if (errorEvent != null)
			{
				errorEvent(e.Message);
			}
		}
	}

	private string GetURL(string category)
	{
		return _baseURL + "/" + category;
	}
}
