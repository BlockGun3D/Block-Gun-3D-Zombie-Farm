using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class TapjoyCB : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024TJViewClosed_0024310 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal TapjoyCB _0024self__0024311;

			public _0024(TapjoyCB self_)
			{
				_0024self__0024311 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					result = (Yield(2, new WaitForSeconds(1f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024311.tapjoyPlugin.SendMessage("GetTJPoints");
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal TapjoyCB _0024self__0024312;

		public _0024TJViewClosed_0024310(TapjoyCB self_)
		{
			_0024self__0024312 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024312);
		}
	}

	public GameObject tapjoyPlugin;

	private bool adding;

	public virtual void TJConnectSuccess()
	{
	}

	public virtual void TJConnectFailed()
	{
	}

	public virtual void TJGetPointsSucceeded(int points)
	{
		int totalBlockCount = Global.gm.GetTotalBlockCount(BlockType.GREEN);
		if (points >= totalBlockCount)
		{
			Debug.Log("MATT: GetPointsSucceeded Callback - Points: " + points);
			Global.gm.SetTotalBlockCount(BlockType.GREEN, points);
			adding = true;
		}
		else if (!adding)
		{
			Global.gm.SetTotalBlockCount(BlockType.GREEN, totalBlockCount);
			tapjoyPlugin.SendMessage("AwardTJPoints", totalBlockCount - points);
		}
		Global.gm.SaveGameData();
	}

	public virtual void TJGetPointsFailed()
	{
	}

	public virtual void TJSpendPointsSucceeded(int points)
	{
	}

	public virtual void TJSpendPointsFailed()
	{
	}

	public virtual void TJAwardPointsSucceeded(int points)
	{
		adding = false;
	}

	public virtual void TJAwardPointsFailed()
	{
	}

	public virtual void TJPointsEarned(int points)
	{
	}

	public virtual void TJViewOpened()
	{
	}

	public virtual IEnumerator TJViewClosed()
	{
		return new _0024TJViewClosed_0024310(this).GetEnumerator();
	}

	public virtual void TJViewOffersFailed()
	{
	}

	public virtual void TJDisplayAdSucceeded()
	{
		GameState gameState = Global.gm.GetGameState();
		if (gameState == GameState.PLAYING || gameState == GameState.MENU_UPGRADES || gameState == GameState.PAUSED_UPGRADES)
		{
			tapjoyPlugin.SendMessage("HideDisplayAd");
		}
	}

	public virtual void Main()
	{
	}
}
