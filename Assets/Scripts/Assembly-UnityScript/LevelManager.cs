using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class LevelManager : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Success_0024280 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal Transform _0024player_0024281;

			internal GameObject[] _0024enemies_0024282;

			internal GameObject _0024enemy_0024283;

			internal int _0024_0024104_0024284;

			internal GameObject[] _0024_0024105_0024285;

			internal int _0024_0024106_0024286;

			internal LevelManager _0024self__0024287;

			public _0024(LevelManager self_)
			{
				_0024self__0024287 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024287.levelMusic.Stop();
					_0024self__0024287.successText.text = "MISSION ACCOMPLISHED";
					_0024self__0024287.successText.enabled = true;
					_0024player_0024281 = Global.pm.gameObject.transform;
					_0024enemies_0024282 = GameObject.FindGameObjectsWithTag("Enemy");
					_0024_0024104_0024284 = 0;
					_0024_0024105_0024285 = _0024enemies_0024282;
					for (_0024_0024106_0024286 = _0024_0024105_0024285.Length; _0024_0024104_0024284 < _0024_0024106_0024286; _0024_0024104_0024284++)
					{
						if (!((_0024player_0024281.transform.position - _0024_0024105_0024285[_0024_0024104_0024284].transform.position).magnitude >= 30f))
						{
							_0024_0024105_0024285[_0024_0024104_0024284].SendMessage("Die");
						}
					}
					result = (Yield(2, new WaitForSeconds(_0024self__0024287.pauseOnSuccessTime)) ? 1 : 0);
					break;
				case 2:
					Global.gm.SetGameState(GameState.SUCCESS);
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal LevelManager _0024self__0024288;

		public _0024Success_0024280(LevelManager self_)
		{
			_0024self__0024288 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024288);
		}
	}

	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Fail_0024289 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal LevelManager _0024self__0024290;

			public _0024(LevelManager self_)
			{
				_0024self__0024290 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024290.levelMusic.Stop();
					_0024self__0024290.successText.text = "MISSION FAILED";
					_0024self__0024290.successText.enabled = true;
					result = (Yield(2, new WaitForSeconds(_0024self__0024290.pauseOnSuccessTime)) ? 1 : 0);
					break;
				case 2:
					Global.gm.SetGameState(GameState.FAILED);
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal LevelManager _0024self__0024291;

		public _0024Fail_0024289(LevelManager self_)
		{
			_0024self__0024291 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024291);
		}
	}

	public int levelNum;

	public Vector3 playerStartPos;

	public float playerStartRot;

	public Quaternion playerStartLook;

	public AudioSource levelMusic;

	public GUIText successText;

	public float pauseOnSuccessTime;

	public GameObject[] hideObjects;

	public int[] hideObjMissions;

	public virtual void Activate()
	{
		SetupLevel();
	}

	public virtual void Awake()
	{
		SetupLevel();
	}

	public virtual void Deactivate()
	{
		successText.enabled = false;
		gameObject.SetActive(false);
	}

	public virtual IEnumerator Success()
	{
		return new _0024Success_0024280(this).GetEnumerator();
	}

	public virtual IEnumerator Fail()
	{
		return new _0024Fail_0024289(this).GetEnumerator();
	}

	private void SetupLevel()
	{
		successText.enabled = false;
		Transform transform = Global.pm.gameObject.transform;
		transform.position = playerStartPos;
		transform.rotation = Quaternion.AngleAxis(playerStartRot, Vector3.up);
		Global.playerStartPos = playerStartPos;
		Global.playerStartRot = Quaternion.AngleAxis(playerStartRot, Vector3.up);
		Global.levelNum = levelNum;
		Global.levelMusic = levelMusic;
		for (int i = 0; i < hideObjects.Length; i++)
		{
			if (hideObjMissions[i] == Global.missionNum)
			{
				hideObjects[i].SetActive(false);
			}
			else
			{
				hideObjects[i].SetActive(true);
			}
		}
	}

	public virtual void Main()
	{
	}
}
