using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class ResetGame : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Activate_0024304 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					Global.pm.Reset();
					Global.gm.SetReviveCost(1);
					Global.gm.ResetLevelBlockCount();
					Global.gm.ResetScore();
					result = (Yield(2, new WaitForSeconds(0.6f)) ? 1 : 0);
					break;
				case 2:
					if (Global.gm.IsTutorialDone())
					{
						Global.gm.SetGameState(GameState.PLAYING);
					}
					else
					{
						Global.gm.SetGameState(GameState.TUTORIAL_LOOK);
					}
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			//yield-return decompiler failed: Method not found
			return new _0024();
		}
	}

	private GameManager gameManager;

	public virtual IEnumerator Activate()
	{
		return new _0024Activate_0024304().GetEnumerator();
	}

	public virtual void Deactivate()
	{
		gameObject.SetActive(false);
	}

	public virtual void Main()
	{
	}
}
