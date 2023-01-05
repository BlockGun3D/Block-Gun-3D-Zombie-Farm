using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class GA_Submit
{
	public enum CategoryType
	{
		GA_User = 0,
		GA_Event = 1,
		GA_Log = 2,
		GA_Purchase = 3
	}

	public struct Item
	{
		public CategoryType Type;

		public Hashtable Parameters;

		public float AddTime;

		public int Count;
	}

	public delegate void SubmitSuccessHandler(List<Item> items, bool success);

	public delegate void SubmitErrorHandler(List<Item> items);

	public Dictionary<CategoryType, string> Categories;

	private string _publicKey;

	private string _privateKey;

	private string _baseURL = "http://api.gameanalytics.com";

	private string _version = "1";

	public void SetupKeys(string publicKey, string privateKey)
	{
		_publicKey = publicKey;
		_privateKey = privateKey;
		Categories = new Dictionary<CategoryType, string>
		{
			{
				CategoryType.GA_User,
				"user"
			},
			{
				CategoryType.GA_Event,
				"design"
			},
			{
				CategoryType.GA_Log,
				"quality"
			},
			{
				CategoryType.GA_Purchase,
				"business"
			}
		};
	}

	public void SubmitQueue(List<Item> queue, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		if (_publicKey.Equals(string.Empty) || _privateKey.Equals(string.Empty))
		{
			GA.LogError("Game Key and/or Secret Key not set. Open GA_Settings to set keys.");
			return;
		}
		Dictionary<CategoryType, List<Item>> dictionary = new Dictionary<CategoryType, List<Item>>();
		foreach (Item item in queue)
		{
			if (dictionary.ContainsKey(item.Type))
			{
				if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]))
				{
					item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID);
				}
				if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID]))
				{
					item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA.API.GenericInfo.SessionID);
				}
				if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build]))
				{
					item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.SettingsGA.Build);
				}
				dictionary[item.Type].Add(item);
				continue;
			}
			if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]))
			{
				item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID);
			}
			if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID]))
			{
				item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA.API.GenericInfo.SessionID);
			}
			if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build]))
			{
				item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.SettingsGA.Build);
			}
			dictionary.Add(item.Type, new List<Item> { item });
		}
		GA.RunCoroutine(Submit(dictionary, successEvent, errorEvent));
	}

	public IEnumerator Submit(Dictionary<CategoryType, List<Item>> categories, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		foreach (KeyValuePair<CategoryType, List<Item>> category in categories)
		{
			List<Item> items = category.Value;
			if (items.Count == 0)
			{
				break;
			}
			CategoryType serviceType = items[0].Type;
			string url = GetURL(Categories[serviceType]);
			List<Hashtable> itemsParameters = new List<Hashtable>();
			for (int i = 0; i < items.Count; i++)
			{
				if (serviceType != items[i].Type)
				{
					GA.LogWarning("GA Error: All messages in a submit must be of the same service/category type.");
					if (errorEvent != null)
					{
						errorEvent(items);
					}
					yield break;
				}
				if (!items[i].Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]))
				{
					items[i].Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID);
				}
				else if (items[i].Parameters[GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]] == null)
				{
					items[i].Parameters[GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]] = GA.API.GenericInfo.UserID;
				}
				Hashtable parameters = ((items[i].Count <= 1) ? items[i].Parameters : items[i].Parameters);
				itemsParameters.Add(parameters);
			}
			string json = DictToJson(itemsParameters);
			if (GA.SettingsGA.ArchiveData && !GA.SettingsGA.InternetConnectivity)
			{
				if (GA.SettingsGA.DebugMode)
				{
					GA.Log("GA: Archiving data (no network connection).");
				}
				GA.API.Archive.ArchiveData(json, serviceType);
				if (successEvent != null)
				{
					successEvent(items, true);
				}
				break;
			}
			if (!GA.SettingsGA.InternetConnectivity)
			{
				GA.LogWarning("GA Error: No network connection.");
				if (errorEvent != null)
				{
					errorEvent(items);
				}
				break;
			}
			byte[] data = Encoding.UTF8.GetBytes(json);
			WWW www = new WWW(url, data, new Hashtable
			{
				{
					"Authorization",
					CreateMD5Hash(json + _privateKey)
				},
				{ "Content-Length", data.Length }
			})
			{
				threadPriority = ThreadPriority.Low
			};
			yield return www;
			if (GA.SettingsGA.DebugMode)
			{
				GA.Log("GA URL: " + url);
				GA.Log("GA Submit: " + json);
				GA.Log("GA Hash: " + CreateMD5Hash(json + _privateKey));
			}
			try
			{
				if (www.error != null && !CheckServerReply(www))
				{
					throw new Exception(www.error);
				}
				Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
				if ((returnParam != null && returnParam.ContainsKey("status") && returnParam["status"].ToString().Equals("ok")) || CheckServerReply(www))
				{
					if (GA.SettingsGA.DebugMode)
					{
						GA.Log("GA Result: " + www.text);
					}
					if (successEvent != null)
					{
						successEvent(items, true);
					}
				}
				else if (returnParam != null && returnParam.ContainsKey("message") && returnParam["message"].ToString().Equals("Game not found") && returnParam.ContainsKey("code") && returnParam["code"].ToString().Equals("400"))
				{
					GA.LogWarning("GA Error: " + www.text + " (NOTE: make sure your Game Key and Secret Key match the keys you recieved from the Game Analytics website. It might take a few minutes before a newly added game will be able to recieve data.)");
					if (errorEvent != null)
					{
						errorEvent(null);
					}
				}
				else
				{
					GA.LogWarning("GA Error: " + www.text);
					if (errorEvent != null)
					{
						errorEvent(items);
					}
				}
			}
			catch (Exception e)
			{
				GA.LogWarning("GA Error: " + e.Message);
				if (e.Message.Contains("400 Bad Request"))
				{
					if (errorEvent != null)
					{
						errorEvent(null);
					}
				}
				else if (errorEvent != null)
				{
					errorEvent(items);
				}
			}
		}
	}

	public string GetBaseURL(bool inclVersion)
	{
		if (inclVersion)
		{
			return _baseURL + "/" + _version;
		}
		return _baseURL;
	}

	public string GetURL(string category)
	{
		return _baseURL + "/" + _version + "/" + _publicKey + "/" + category;
	}

	public string CreateMD5Hash(string input)
	{
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] array = mD.ComputeHash(bytes);
		string text = string.Empty;
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			text += string.Format("{0:x2}", b);
		}
		return text;
	}

	public string CreateSha1Hash(string input)
	{
		SHA1 sHA = new SHA1CryptoServiceProvider();
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] inArray = sHA.ComputeHash(bytes);
		return Convert.ToBase64String(inArray);
	}

	public string GetPrivateKey()
	{
		return _privateKey;
	}

	public bool CheckServerReply(WWW www)
	{
		//Discarded unreachable code: IL_00ff, IL_010d
		try
		{
			if (www.error != null)
			{
				string text = www.error.Substring(0, 3);
				if (text.Equals("201") || text.Equals("202") || text.Equals("203") || text.Equals("204") || text.Equals("205") || text.Equals("206"))
				{
					return true;
				}
			}
			if (!www.responseHeaders.ContainsKey("STATUS"))
			{
				return false;
			}
			string text2 = www.responseHeaders["STATUS"];
			string[] array = text2.Split(' ');
			int result;
			if (array.Length > 1 && int.TryParse(array[1], out result) && result >= 200 && result < 300)
			{
				return true;
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static string DictToJson(List<Hashtable> list)
	{
		StringBuilder stringBuilder = new StringBuilder("[");
		int num = 0;
		int num2 = 0;
		foreach (Hashtable item in list)
		{
			stringBuilder.Append('{');
			num2 = 0;
			foreach (object key in item.Keys)
			{
				num2++;
				stringBuilder.AppendFormat("\"{0}\":\"{1}\"", key, item[key]);
				if (num2 < item.Keys.Count)
				{
					stringBuilder.Append(',');
				}
			}
			stringBuilder.Append('}');
			num++;
			if (num < list.Count)
			{
				stringBuilder.Append(',');
			}
		}
		return stringBuilder.Append("]").ToString();
	}
}
