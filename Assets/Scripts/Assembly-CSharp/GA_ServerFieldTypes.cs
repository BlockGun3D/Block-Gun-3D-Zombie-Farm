using System.Collections.Generic;

public static class GA_ServerFieldTypes
{
	public enum FieldType
	{
		UserID = 0,
		SessionID = 1,
		TimeStamp = 2,
		System = 3,
		Language = 4,
		EventID = 5,
		Value = 6,
		Level = 7,
		X = 8,
		Y = 9,
		Z = 10,
		CrashID = 11,
		Message = 12,
		Currency = 13,
		Amount = 14,
		PurchaseID = 15,
		Build = 16,
		Gender = 17,
		Birth_year = 18,
		Country = 19,
		State = 20,
		Friend_Count = 21,
		Ios_id = 22,
		Android_id = 23,
		Platform = 24,
		Device = 25,
		Os = 26,
		OsVersion = 27,
		Sdk = 28
	}

	public static Dictionary<FieldType, string> Fields = new Dictionary<FieldType, string>
	{
		{
			FieldType.UserID,
			"user_id"
		},
		{
			FieldType.SessionID,
			"session_id"
		},
		{
			FieldType.TimeStamp,
			"ts"
		},
		{
			FieldType.System,
			"system"
		},
		{
			FieldType.Language,
			"language"
		},
		{
			FieldType.EventID,
			"event_id"
		},
		{
			FieldType.Value,
			"value"
		},
		{
			FieldType.Level,
			"area"
		},
		{
			FieldType.X,
			"x"
		},
		{
			FieldType.Y,
			"y"
		},
		{
			FieldType.Z,
			"z"
		},
		{
			FieldType.CrashID,
			"qa_id"
		},
		{
			FieldType.Message,
			"message"
		},
		{
			FieldType.Currency,
			"currency"
		},
		{
			FieldType.Amount,
			"amount"
		},
		{
			FieldType.PurchaseID,
			"business_id"
		},
		{
			FieldType.Build,
			"build"
		},
		{
			FieldType.Gender,
			"gender"
		},
		{
			FieldType.Birth_year,
			"birth_year"
		},
		{
			FieldType.Country,
			"country"
		},
		{
			FieldType.State,
			"state"
		},
		{
			FieldType.Friend_Count,
			"friend_count"
		},
		{
			FieldType.Ios_id,
			"ios_id"
		},
		{
			FieldType.Android_id,
			"android_id"
		},
		{
			FieldType.Platform,
			"platform"
		},
		{
			FieldType.Device,
			"device"
		},
		{
			FieldType.Os,
			"os"
		},
		{
			FieldType.OsVersion,
			"os_version"
		},
		{
			FieldType.Sdk,
			"sdk_version"
		}
	};
}
