using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class LevelSwitcher : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Activate_0024292 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal LevelSwitcher _0024self__0024293;

			public _0024(LevelSwitcher self_)
			{
				_0024self__0024293 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					result = (Yield(2, new WaitForSeconds(0.1f)) ? 1 : 0);
					break;
				case 2:
					Global.gm.SetGameState(_0024self__0024293.levelGameState[_0024self__0024293.curLevel]);
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal LevelSwitcher _0024self__0024294;

		public _0024Activate_0024292(LevelSwitcher self_)
		{
			_0024self__0024294 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024294);
		}
	}

	public GameState[] levelGameState;

	private int curLevel;

	public virtual IEnumerator Activate()
	{
		return new _0024Activate_0024292(this).GetEnumerator();
	}

	public virtual void SwitchLevel()
	{
		curLevel = (curLevel + 1) % levelGameState.Length;
		Global.gm.SetGameState(levelGameState[curLevel]);
	}

	public virtual void Main()
	{
	}
}
