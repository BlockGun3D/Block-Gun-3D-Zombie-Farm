using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class WeaponManager : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024ChangeWeapon_0024314 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal int _0024indx_0024315;

			internal WeaponManager _0024self__0024316;

			public _0024(int indx, WeaponManager self_)
			{
				_0024indx_0024315 = indx;
				_0024self__0024316 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					canFire = false;
					_0024self__0024316.currentWeapon = _0024indx_0024315;
					_0024self__0024316.GetComponent<Animation>().PlayQueued("GunDown", QueueMode.CompleteOthers);
					_0024self__0024316.GetComponent<Animation>().PlayQueued("GunUp");
					result = (Yield(2, new WaitForSeconds(0.5f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024316.SetActiveWeapon(_0024self__0024316.weapons[_0024indx_0024315]);
					_0024self__0024316.weapons[_0024indx_0024315].SendMessage("AddAmmo", _0024self__0024316.ammoToAdd[_0024indx_0024315]);
					_0024self__0024316.ammoToAdd[_0024indx_0024315] = 0;
					canFire = true;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal int _0024indx_0024317;

		internal WeaponManager _0024self__0024318;

		public _0024ChangeWeapon_0024314(int indx, WeaponManager self_)
		{
			_0024indx_0024317 = indx;
			_0024self__0024318 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024indx_0024317, _0024self__0024318);
		}
	}

	[NonSerialized]
	public static bool canFire = true;

	public int startWithWeaponIndx;

	public GameObject[] weapons;

	public GUITexture changeWeaponButton;

	public AudioClip gunPickUpSound;

	private Transform thisTransform;

	private GameObject activeGun;

	private bool switching;

	private int currentWeapon;

	private GameManager gm;

	private bool[] availableWeapons;

	private int[] ammoToAdd;

	public WeaponManager()
	{
		weapons = new GameObject[4];
		availableWeapons = new bool[4];
		ammoToAdd = new int[4];
	}

	public virtual void Start()
	{
		gm = GameManager.GetInstance();
		thisTransform = transform;
		currentWeapon = startWithWeaponIndx;
		SetActiveWeapon(weapons[startWithWeaponIndx]);
		availableWeapons[startWithWeaponIndx] = true;
	}

	public virtual void Reset()
	{
		ResetWeapons();
		currentWeapon = startWithWeaponIndx;
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
			transform.gameObject.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
			UnityRuntimeServices.Update(enumerator, transform);
		}
		SetActiveWeapon(weapons[startWithWeaponIndx]);
	}

	public virtual void Update()
	{
		GameState gameState = gm.GetGameState();
		if (gameState == GameState.PLAYING)
		{
			UpdateGameplay();
		}
	}

	public virtual void UpdateGameplay()
	{
		if (NextAvailableWeapon(currentWeapon) != currentWeapon && (StaticFuncs.TestButtonTouchBegan(changeWeaponButton) || Input.GetKeyDown("e")))
		{
			StartCoroutine(ChangeWeapon(NextAvailableWeapon(currentWeapon)));
		}
	}

	public virtual IEnumerator ChangeWeapon(int indx)
	{
		return new _0024ChangeWeapon_0024314(indx, this).GetEnumerator();
	}

	public virtual void SetActiveWeapon(GameObject weapon)
	{
		int i = 0;
		GameObject[] array = weapons;
		for (int length = array.Length; i < length; i++)
		{
			if (array[i] == weapon)
			{
				array[i].SetActive(true);
			}
			else
			{
				array[i].SetActive(false);
			}
		}
	}

	public virtual void GotWeapon(GunType type)
	{
		if (!availableWeapons[(int)type])
		{
			availableWeapons[(int)type] = true;
			StartCoroutine(ChangeWeapon((int)type));
			ammoToAdd[(int)type] = ammoToAdd[(int)type] + 1;
		}
		else if (type == (GunType)currentWeapon)
		{
			weapons[(int)type].SendMessage("AddAmmo", 1);
		}
		else
		{
			ammoToAdd[(int)type] = ammoToAdd[(int)type] + 1;
		}
		AudioSource.PlayClipAtPoint(gunPickUpSound, transform.position);
	}

	public virtual void ResetWeapons()
	{
		int i = 0;
		bool[] array = availableWeapons;
		for (int length = array.Length; i < length; i++)
		{
			array[i] = false;
		}
		availableWeapons[0] = true;
		int j = 0;
		int[] array2 = ammoToAdd;
		for (int length2 = array2.Length; j < length2; j++)
		{
			array2[j] = 0;
		}
	}

	public virtual int NextAvailableWeapon(int curIdx)
	{
		int num = 0;
		int result;
		while (true)
		{
			if (num < availableWeapons.Length)
			{
				int num2 = (num + curIdx + 1) % availableWeapons.Length;
				if (availableWeapons[num2])
				{
					result = num2;
					break;
				}
				num++;
				continue;
			}
			result = curIdx;
			break;
		}
		return result;
	}

	public virtual void Activate()
	{
		Reset();
		SetActiveWeapon(weapons[0]);
		GetComponent<Animation>().Play("GunUp");
	}

	public virtual void Deactivate()
	{
		gameObject.SetActive(false);
	}

	public virtual void Main()
	{
	}
}
