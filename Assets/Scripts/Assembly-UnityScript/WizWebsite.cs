using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class WizWebsite : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Start_0024319 : GenericGenerator<WWW>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WWW>, IEnumerator
		{
			internal WWW _0024www_0024320;

			internal string _0024str_0024321;

			internal WizWebsite _0024self__0024322;

			public _0024(WizWebsite self_)
			{
				_0024self__0024322 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024www_0024320 = new WWW(_0024self__0024322.urlAndroid);
					result = (Yield(2, _0024www_0024320) ? 1 : 0);
					break;
				case 2:
					if (!string.IsNullOrEmpty(_0024www_0024320.error))
					{
						Debug.Log("MATTTTTTTT: " + _0024www_0024320.error);
					}
					else
					{
						_0024str_0024321 = _0024www_0024320.text;
						Debug.Log("MATTTTTTTTT str: " + _0024str_0024321);
						Global.adNetworkChoose = int.Parse(_0024str_0024321);
						YieldDefault(1);
					}
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal WizWebsite _0024self__0024323;

		public _0024Start_0024319(WizWebsite self_)
		{
			_0024self__0024323 = self_;
		}

		public override IEnumerator<WWW> GetEnumerator()
		{
			return new _0024(_0024self__0024323);
		}
	}

	public string urlAndroid;

	public string urlIOS;

	public WizWebsite()
	{
		urlAndroid = "http://wizardgames.ca/cbPosibility.txt";
		urlIOS = string.Empty;
	}

	public virtual IEnumerator Start()
	{
		return new _0024Start_0024319(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
