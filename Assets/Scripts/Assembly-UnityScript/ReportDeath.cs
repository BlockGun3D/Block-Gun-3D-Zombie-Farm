using System;
using UnityEngine;

[Serializable]
public class ReportDeath : MonoBehaviour
{
	public string[] messages;

	public GameObject[] reportees;

	public int maxNum;

	private int idxReportee;

	private int idxMessage;

	private bool initialized;

	public ReportDeath()
	{
		maxNum = 1;
	}

	public virtual void Initialize()
	{
		messages = new string[maxNum];
		reportees = new GameObject[maxNum];
		initialized = true;
	}

	public virtual void Die()
	{
		for (int i = 0; i < reportees.Length; i++)
		{
			if ((bool)reportees[i])
			{
				reportees[i].SendMessage(messages[i]);
			}
		}
	}

	public virtual void AddReportMessage(string message)
	{
		if (!initialized)
		{
			Initialize();
		}
		if (idxMessage < maxNum)
		{
			messages[idxMessage] = message;
			idxMessage++;
		}
	}

	public virtual void AddReportee(GameObject @object)
	{
		if (!initialized)
		{
			Initialize();
		}
		if (idxReportee < maxNum)
		{
			reportees[idxReportee] = @object;
			idxReportee++;
		}
	}

	public virtual void Main()
	{
	}
}
