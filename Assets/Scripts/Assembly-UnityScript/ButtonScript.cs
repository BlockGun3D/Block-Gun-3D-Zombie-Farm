using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ButtonScript : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Deactivate_0024255 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal Transform _0024child_0024256;

			internal IEnumerator _0024_0024iterator_002475_0024257;

			internal ButtonScript _0024self__0024258;

			public _0024(ButtonScript self_)
			{
				_0024self__0024258 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (_0024self__0024258.sendActivateToChildren)
					{
						_0024_0024iterator_002475_0024257 = UnityRuntimeServices.GetEnumerator(_0024self__0024258.transform);
						while (_0024_0024iterator_002475_0024257.MoveNext())
						{
							object obj = _0024_0024iterator_002475_0024257.Current;
							if (!(obj is Transform))
							{
								obj = RuntimeServices.Coerce(obj, typeof(Transform));
							}
							_0024child_0024256 = (Transform)obj;
							_0024child_0024256.gameObject.SendMessage("Deactivate");
							UnityRuntimeServices.Update(_0024_0024iterator_002475_0024257, _0024child_0024256);
						}
					}
					if (_0024self__0024258.outAnimation != string.Empty)
					{
						_0024self__0024258.GetComponent<Animation>().Play(_0024self__0024258.outAnimation);
						_0024self__0024258.isDeactivating = true;
						result = (Yield(2, new WaitForSeconds(_0024self__0024258.GetComponent<Animation>().clip.length)) ? 1 : 0);
						break;
					}
					_0024self__0024258.gameObject.SetActive(false);
					goto IL_0137;
				case 2:
					_0024self__0024258.gameObject.SetActive(false);
					goto IL_0137;
				case 1:
					{
						result = 0;
						break;
					}
					IL_0137:
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal ButtonScript _0024self__0024259;

		public _0024Deactivate_0024255(ButtonScript self_)
		{
			_0024self__0024259 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024259);
		}
	}

	public Color pressColor;

	public bool changeState;

	public GameState changeToState;

	public string key;

	public string inAnimation;

	public string outAnimation;

	public Vector3 inPosition;

	public string pressedMessage;

	public GameObject objectToSendMessageTo;

	public bool saveDataOnPress;

	public bool sendActivateToChildren;

	public bool resetOnChangeState;

	public AudioClip sound;

	private bool touching;

	private GameManager gm;

	private Color originalColor;

	private bool isDeactivating;

	public ButtonScript()
	{
		pressColor = new Color(1f, 1f, 1f, 0.3f);
		pressedMessage = string.Empty;
	}

	public virtual void Start()
	{
		gm = GameManager.GetInstance();
		originalColor = GetComponent<GUITexture>().color;
		if (!(originalColor.a >= 0.01f))
		{
			originalColor.a = 0.5f;
		}
	}

	public virtual void Update()
	{
		if (isDeactivating)
		{
			return;
		}
		if (!touching)
		{
			if (StaticFuncs.TestButtonTouchBegan(GetComponent<GUITexture>()) || Input.GetKeyDown(key))
			{
				touching = true;
				GetComponent<GUITexture>().color = pressColor;
			}
		}
		else
		{
			if (!Input.GetKeyUp(key) && StaticFuncs.TestButtonTouch(GetComponent<GUITexture>()))
			{
				return;
			}
			touching = false;
			GetComponent<GUITexture>().color = originalColor;
			if (pressedMessage != string.Empty)
			{
				if ((bool)objectToSendMessageTo)
				{
					objectToSendMessageTo.SendMessage(pressedMessage);
				}
				else
				{
					gameObject.SendMessage(pressedMessage);
				}
			}
			if ((bool)sound)
			{
				AudioSource.PlayClipAtPoint(sound, transform.position);
			}
			if (saveDataOnPress)
			{
				Global.SaveGameData();
			}
			if (changeState && resetOnChangeState)
			{
				Global.ResetAndChangeState(changeToState);
			}
			else if (changeState)
			{
				gm.SetGameState(changeToState);
			}
		}
	}

	public virtual void Activate()
	{
		if (inAnimation != string.Empty)
		{
			GetComponent<Animation>().Play(inAnimation);
		}
		else if (outAnimation != string.Empty)
		{
			this.transform.position = inPosition;
		}
		if (sendActivateToChildren)
		{
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
		isDeactivating = false;
	}

	public virtual IEnumerator Deactivate()
	{
		return new _0024Deactivate_0024255(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
