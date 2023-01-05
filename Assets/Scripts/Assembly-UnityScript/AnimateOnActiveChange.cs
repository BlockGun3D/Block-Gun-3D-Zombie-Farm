using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class AnimateOnActiveChange : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Deactivate_0024250 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal Transform _0024child_0024251;

			internal IEnumerator _0024_0024iterator_002473_0024252;

			internal AnimateOnActiveChange _0024self__0024253;

			public _0024(AnimateOnActiveChange self_)
			{
				_0024self__0024253 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (_0024self__0024253.sendActivateToChildren)
					{
						_0024_0024iterator_002473_0024252 = UnityRuntimeServices.GetEnumerator(_0024self__0024253.transform);
						while (_0024_0024iterator_002473_0024252.MoveNext())
						{
							object obj = _0024_0024iterator_002473_0024252.Current;
							if (!(obj is Transform))
							{
								obj = RuntimeServices.Coerce(obj, typeof(Transform));
							}
							_0024child_0024251 = (Transform)obj;
							_0024child_0024251.gameObject.SendMessage("Deactivate");
							UnityRuntimeServices.Update(_0024_0024iterator_002473_0024252, _0024child_0024251);
						}
					}
					if (_0024self__0024253.outAnimation != string.Empty)
					{
						_0024self__0024253.GetComponent<Animation>().Play(_0024self__0024253.outAnimation);
						result = (Yield(2, new WaitForSeconds(_0024self__0024253.GetComponent<Animation>().clip.length)) ? 1 : 0);
						break;
					}
					goto case 2;
				case 2:
					_0024self__0024253.gameObject.SetActive(false);
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal AnimateOnActiveChange _0024self__0024254;

		public _0024Deactivate_0024250(AnimateOnActiveChange self_)
		{
			_0024self__0024254 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024254);
		}
	}

	public string inAnimation;

	public string outAnimation;

	public Vector3 inPosition;

	public bool useInPos;

	public bool sendActivateToChildren;

	public virtual void Activate()
	{
		if (inAnimation != string.Empty)
		{
			if (useInPos)
			{
				this.transform.position = inPosition;
			}
			GetComponent<Animation>().Play(inAnimation);
		}
		else
		{
			this.transform.position = inPosition;
		}
		if (!sendActivateToChildren)
		{
			return;
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform = (Transform)obj;
			transform.gameObject.SetActive(true);
			UnityRuntimeServices.Update(enumerator, transform);
			transform.gameObject.SendMessage("Activate");
			UnityRuntimeServices.Update(enumerator, transform);
		}
	}

	public virtual IEnumerator Deactivate()
	{
		return new _0024Deactivate_0024250(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
