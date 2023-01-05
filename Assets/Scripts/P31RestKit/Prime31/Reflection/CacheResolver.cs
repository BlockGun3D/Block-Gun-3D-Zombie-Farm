using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Prime31.Reflection
{
	public class CacheResolver
	{
		private delegate object CtorDelegate();

		public sealed class MemberMap
		{
			public readonly MemberInfo MemberInfo;

			public readonly Type Type;

			public readonly GetHandler Getter;

			public readonly SetHandler Setter;

			public MemberMap(PropertyInfo propertyInfo)
			{
				MemberInfo = propertyInfo;
				Type = propertyInfo.PropertyType;
				Getter = createGetHandler(propertyInfo);
				Setter = createSetHandler(propertyInfo);
			}

			public MemberMap(FieldInfo fieldInfo)
			{
				MemberInfo = fieldInfo;
				Type = fieldInfo.FieldType;
				Getter = createGetHandler(fieldInfo);
				Setter = createSetHandler(fieldInfo);
			}
		}

		[CompilerGenerated]
		private sealed class _003CgetNewInstance_003Ec__AnonStorey5
		{
			internal ConstructorInfo constructorInfo;

			internal object _003C_003Em__4()
			{
				return constructorInfo.Invoke(null);
			}
		}

		[CompilerGenerated]
		private sealed class _003CcreateGetHandler_003Ec__AnonStorey6
		{
			internal FieldInfo fieldInfo;

			internal object _003C_003Em__5(object instance)
			{
				return fieldInfo.GetValue(instance);
			}
		}

		[CompilerGenerated]
		private sealed class _003CcreateSetHandler_003Ec__AnonStorey7
		{
			internal FieldInfo fieldInfo;

			internal void _003C_003Em__6(object instance, object value)
			{
				fieldInfo.SetValue(instance, value);
			}
		}

		[CompilerGenerated]
		private sealed class _003CcreateGetHandler_003Ec__AnonStorey8
		{
			internal MethodInfo getMethodInfo;

			internal object _003C_003Em__7(object instance)
			{
				return getMethodInfo.Invoke(instance, Type.EmptyTypes);
			}
		}

		[CompilerGenerated]
		private sealed class _003CcreateSetHandler_003Ec__AnonStorey9
		{
			internal MethodInfo setMethodInfo;

			internal void _003C_003Em__8(object instance, object value)
			{
				setMethodInfo.Invoke(instance, new object[1] { value });
			}
		}

		private readonly MemberMapLoader _memberMapLoader;

		private readonly SafeDictionary<Type, SafeDictionary<string, MemberMap>> _memberMapsCache = new SafeDictionary<Type, SafeDictionary<string, MemberMap>>();

		private static readonly SafeDictionary<Type, CtorDelegate> constructorCache = new SafeDictionary<Type, CtorDelegate>();

		public CacheResolver(MemberMapLoader memberMapLoader)
		{
			_memberMapLoader = memberMapLoader;
		}

		public static object getNewInstance(Type type)
		{
			_003CgetNewInstance_003Ec__AnonStorey5 _003CgetNewInstance_003Ec__AnonStorey = new _003CgetNewInstance_003Ec__AnonStorey5();
			CtorDelegate value;
			if (constructorCache.tryGetValue(type, out value))
			{
				return value();
			}
			_003CgetNewInstance_003Ec__AnonStorey.constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			value = _003CgetNewInstance_003Ec__AnonStorey._003C_003Em__4;
			constructorCache.add(type, value);
			return value();
		}

		public SafeDictionary<string, MemberMap> loadMaps(Type type)
		{
			if (type == null || type == typeof(object))
			{
				return null;
			}
			SafeDictionary<string, MemberMap> value;
			if (_memberMapsCache.tryGetValue(type, out value))
			{
				return value;
			}
			value = new SafeDictionary<string, MemberMap>();
			_memberMapLoader(type, value);
			_memberMapsCache.add(type, value);
			return value;
		}

		private static GetHandler createGetHandler(FieldInfo fieldInfo)
		{
			_003CcreateGetHandler_003Ec__AnonStorey6 _003CcreateGetHandler_003Ec__AnonStorey = new _003CcreateGetHandler_003Ec__AnonStorey6();
			_003CcreateGetHandler_003Ec__AnonStorey.fieldInfo = fieldInfo;
			return _003CcreateGetHandler_003Ec__AnonStorey._003C_003Em__5;
		}

		private static SetHandler createSetHandler(FieldInfo fieldInfo)
		{
			_003CcreateSetHandler_003Ec__AnonStorey7 _003CcreateSetHandler_003Ec__AnonStorey = new _003CcreateSetHandler_003Ec__AnonStorey7();
			_003CcreateSetHandler_003Ec__AnonStorey.fieldInfo = fieldInfo;
			if (_003CcreateSetHandler_003Ec__AnonStorey.fieldInfo.IsInitOnly || _003CcreateSetHandler_003Ec__AnonStorey.fieldInfo.IsLiteral)
			{
				return null;
			}
			return _003CcreateSetHandler_003Ec__AnonStorey._003C_003Em__6;
		}

		private static GetHandler createGetHandler(PropertyInfo propertyInfo)
		{
			_003CcreateGetHandler_003Ec__AnonStorey8 _003CcreateGetHandler_003Ec__AnonStorey = new _003CcreateGetHandler_003Ec__AnonStorey8();
			_003CcreateGetHandler_003Ec__AnonStorey.getMethodInfo = propertyInfo.GetGetMethod(true);
			if (_003CcreateGetHandler_003Ec__AnonStorey.getMethodInfo == null)
			{
				return null;
			}
			return _003CcreateGetHandler_003Ec__AnonStorey._003C_003Em__7;
		}

		private static SetHandler createSetHandler(PropertyInfo propertyInfo)
		{
			_003CcreateSetHandler_003Ec__AnonStorey9 _003CcreateSetHandler_003Ec__AnonStorey = new _003CcreateSetHandler_003Ec__AnonStorey9();
			_003CcreateSetHandler_003Ec__AnonStorey.setMethodInfo = propertyInfo.GetSetMethod(true);
			if (_003CcreateSetHandler_003Ec__AnonStorey.setMethodInfo == null)
			{
				return null;
			}
			return _003CcreateSetHandler_003Ec__AnonStorey._003C_003Em__8;
		}
	}
}
