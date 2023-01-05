using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Debug
{
	public bool SubmitErrors;

	public int MaxErrorCount;

	public bool SubmitErrorStackTrace;

	public bool SubmitErrorSystemInfo;

	private int _errorCount;

	private bool _showLogOnGUI;

	public List<string> Messages;

	public void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (_showLogOnGUI)
		{
			if (Messages == null)
			{
				Messages = new List<string>();
			}
			Messages.Add(logString);
		}
		if (!SubmitErrors || _errorCount >= MaxErrorCount || (type != LogType.Exception && type != 0))
		{
			return;
		}
		_errorCount++;
		bool flag = false;
		string eventName = "Exception";
		if (SubmitErrorStackTrace)
		{
			SubmitError(eventName, logString.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ') + " " + stackTrace.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' '));
			flag = true;
		}
		if (SubmitErrorSystemInfo)
		{
			List<Hashtable> genericInfo = GA.API.GenericInfo.GetGenericInfo(logString);
			foreach (Hashtable item in genericInfo)
			{
				GA_Queue.AddItem(item, GA_Submit.CategoryType.GA_Log, false);
			}
			flag = true;
		}
		if (!flag)
		{
			SubmitError(eventName, null);
		}
	}

	public void SubmitError(string eventName, string message)
	{
		Vector3 vector = Vector3.zero;
		if (GA.SettingsGA.TrackTarget != null)
		{
			vector = GA.SettingsGA.TrackTarget.position;
		}
		GA.API.Quality.NewErrorEvent(eventName, message, vector.x, vector.y, vector.z);
	}
}
