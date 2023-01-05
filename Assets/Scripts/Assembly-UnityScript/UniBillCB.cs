using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class UniBillCB : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024StoreConnected_0024313 : GenericGenerator<WaitForSeconds>
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
					Global.storeRetrieved = true;
					result = (Yield(2, new WaitForSeconds(0.51f)) ? 1 : 0);
					break;
				case 2:
					Global.gm.SetGameState(GameState.MENU_GET_BLOCKS);
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

	public virtual IEnumerator StoreConnected()
	{
		return new _0024StoreConnected_0024313().GetEnumerator();
	}

	public virtual void StoreCouldNotConnect()
	{
		Global.gm.SetGameState(GameState.NO_STORE_ACCESS);
	}

	public virtual void TransactionsRestored(int flags)
	{
		if (((uint)flags & (true ? 1u : 0u)) != 0)
		{
			Global.gm.TurnOnBlockDoubler();
		}
		if (((uint)flags & 2u) != 0)
		{
			Global.gm.TurnOffAds();
		}
	}

	public virtual void PurchaseComplete(IAPItems index)
	{
		switch (index)
		{
		case IAPItems.BLOCKS_1000:
			Global.gm.AddBlocksToTotal(1000, BlockType.GREEN);
			break;
		case IAPItems.BLOCKS_10000:
			Global.gm.AddBlocksToTotal(10000, BlockType.GREEN);
			break;
		case IAPItems.BLOCKS_3000:
			Global.gm.AddBlocksToTotal(3000, BlockType.GREEN);
			break;
		case IAPItems.CUBES_5:
			Global.gm.AddBlocksToTotal(5, BlockType.SILVER);
			break;
		case IAPItems.CUBES_15:
			Global.gm.AddBlocksToTotal(15, BlockType.SILVER);
			break;
		case IAPItems.CUBES_50:
			Global.gm.AddBlocksToTotal(50, BlockType.SILVER);
			break;
		case IAPItems.DOUBLER:
			Global.gm.TurnOnBlockDoubler();
			break;
		case IAPItems.NO_ADS:
			Global.gm.TurnOffAds();
			break;
		}
		Global.gm.SetGameState(GameState.PURCHASE_SUCCESS);
		Global.SaveGameData();
	}

	public virtual void PurchaseCanceled(int index)
	{
	}

	public virtual void PurchaseFailed(int index)
	{
	}

	public virtual void Main()
	{
	}
}
