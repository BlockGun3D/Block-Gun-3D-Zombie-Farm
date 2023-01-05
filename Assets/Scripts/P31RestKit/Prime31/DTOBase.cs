using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Prime31
{
	public class DTOBase
	{
		[CompilerGenerated]
		private sealed class _003CgetMembersWithSetters_003Ec__AnonStorey1
		{
			internal FieldInfo theInfo;

			internal DTOBase _0024this;

			internal void _003C_003Em__0(object val)
			{
				theInfo.SetValue(_0024this, val);
			}
		}

		[CompilerGenerated]
		private sealed class _003CgetMembersWithSetters_003Ec__AnonStorey2
		{
			internal PropertyInfo theInfo;

			internal DTOBase _0024this;

			internal void _003C_003Em__1(object val)
			{
				theInfo.SetValue(_0024this, val, null);
			}
		}

		public static List<T> listFromJson<T>(string json) where T : DTOBase
		{
			List<object> list = json.listFromJson();
			List<T> list2 = new List<T>();
			foreach (object item2 in list)
			{
				T item = Activator.CreateInstance<T>();
				item.setDataFromDictionary(item2 as Dictionary<string, object>);
				list2.Add(item);
			}
			return list2;
		}

		public void setDataFromJson(string json)
		{
			setDataFromDictionary(json.dictionaryFromJson());
		}

		public void setDataFromDictionary(Dictionary<string, object> dict)
		{
			Dictionary<string, Action<object>> membersWithSetters = getMembersWithSetters();
			foreach (KeyValuePair<string, object> item in dict)
			{
				if (membersWithSetters.ContainsKey(item.Key))
				{
					try
					{
						membersWithSetters[item.Key](item.Value);
					}
					catch (Exception obj)
					{
						Utils.logObject(obj);
					}
				}
			}
		}

		private bool shouldIncludeTypeWithSetters(Type type)
		{
			if (type.IsGenericType)
			{
				return false;
			}
			if (type.Namespace.StartsWith("System"))
			{
				return true;
			}
			return false;
		}

		protected Dictionary<string, Action<object>> getMembersWithSetters()
		{
			Dictionary<string, Action<object>> dictionary = new Dictionary<string, Action<object>>();
			FieldInfo[] fields = GetType().GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				_003CgetMembersWithSetters_003Ec__AnonStorey1 _003CgetMembersWithSetters_003Ec__AnonStorey = new _003CgetMembersWithSetters_003Ec__AnonStorey1();
				_003CgetMembersWithSetters_003Ec__AnonStorey._0024this = this;
				if (shouldIncludeTypeWithSetters(fieldInfo.FieldType))
				{
					_003CgetMembersWithSetters_003Ec__AnonStorey.theInfo = fieldInfo;
					dictionary[fieldInfo.Name] = _003CgetMembersWithSetters_003Ec__AnonStorey._003C_003Em__0;
				}
			}
			PropertyInfo[] properties = GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (shouldIncludeTypeWithSetters(propertyInfo.PropertyType) && propertyInfo.CanWrite && propertyInfo.GetSetMethod() != null)
				{
					_003CgetMembersWithSetters_003Ec__AnonStorey2 _003CgetMembersWithSetters_003Ec__AnonStorey2 = new _003CgetMembersWithSetters_003Ec__AnonStorey2();
					_003CgetMembersWithSetters_003Ec__AnonStorey2._0024this = this;
					_003CgetMembersWithSetters_003Ec__AnonStorey2.theInfo = propertyInfo;
					dictionary[propertyInfo.Name] = _003CgetMembersWithSetters_003Ec__AnonStorey2._003C_003Em__1;
				}
			}
			return dictionary;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("[{0}]:", GetType());
			FieldInfo[] fields = GetType().GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				stringBuilder.AppendFormat(", {0}: {1}", fieldInfo.Name, fieldInfo.GetValue(this));
			}
			PropertyInfo[] properties = GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				stringBuilder.AppendFormat(", {0}: {1}", propertyInfo.Name, propertyInfo.GetValue(this, null));
			}
			return stringBuilder.ToString();
		}
	}
}
