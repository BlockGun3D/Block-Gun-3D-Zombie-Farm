using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(AudioSource))]
public class GunShoot : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Shoot_0024267 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal int _0024i_0024268;

			internal Vector3 _0024pos_0024269;

			internal float _0024varianceX_0024270;

			internal float _0024varianceY_0024271;

			internal GameObject _0024clone_0024272;

			internal Vector3 _0024vec_0024273;

			internal GunShoot _0024self__0024274;

			public _0024(GunShoot self_)
			{
				_0024self__0024274 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (_0024self__0024274.clipLeft != 0f)
					{
						_0024self__0024274.clipLeft -= 1f;
						for (_0024i_0024268 = 0; _0024i_0024268 < _0024self__0024274.numProjectiles; _0024i_0024268++)
						{
							_0024pos_0024269 = _0024self__0024274.thisTransform.TransformDirection(_0024self__0024274.bulletSpawnOffset);
							_0024varianceX_0024270 = _0024self__0024274.accuracy * StaticFuncs.RandomVal(0f, _0024self__0024274.aimVariance);
							_0024varianceY_0024271 = _0024self__0024274.accuracy * StaticFuncs.RandomVal(0f, _0024self__0024274.aimVariance);
							_0024clone_0024272 = PoolsManager.GetObject(_0024self__0024274.projectileType, _0024self__0024274.thisTransform.position + _0024pos_0024269, _0024self__0024274.thisTransform.rotation);
							_0024clone_0024272.SendMessage("SetEnemy", "Enemy");
							_0024vec_0024273 = new Vector3(_0024varianceX_0024270, _0024varianceY_0024271, 1f);
							Vector3.Normalize(_0024vec_0024273);
							_0024clone_0024272.GetComponent<Rigidbody>().velocity = _0024self__0024274.thisTransform.TransformDirection(_0024vec_0024273 * _0024self__0024274.speed);
							_0024clone_0024272.SendMessage("SetDamage", _0024self__0024274.damage);
						}
						_0024self__0024274.parent.GetComponent<Animation>().Play(_0024self__0024274.recoilAnimationName);
						if ((bool)_0024self__0024274.childAnimation)
						{
							_0024self__0024274.childAnimation.Play();
						}
						_0024self__0024274.shotSound.Play();
						if ((bool)_0024self__0024274.particleSys)
						{
							_0024self__0024274.particleSys.Emit(1);
						}
						if ((bool)_0024self__0024274.thisLight)
						{
							_0024self__0024274.thisLight.enabled = true;
							result = (Yield(2, new WaitForSeconds(0.2f)) ? 1 : 0);
							break;
						}
					}
					else
					{
						_0024self__0024274.clickSound.Play();
						if (_0024self__0024274.ammoLeft > 0 || _0024self__0024274.infiniteAmmo)
						{
							_0024self__0024274.reloadButton.GetComponent<Animation>().Play("flashRed");
						}
						else
						{
							_0024self__0024274.changeWeaponButton.GetComponent<Animation>().Play("flashRed");
						}
					}
					goto IL_02e7;
				case 2:
					_0024self__0024274.thisLight.enabled = false;
					goto IL_02e7;
				case 1:
					{
						result = 0;
						break;
					}
					IL_02e7:
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal GunShoot _0024self__0024275;

		public _0024Shoot_0024267(GunShoot self_)
		{
			_0024self__0024275 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024275);
		}
	}

	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Reload_0024276 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal float _0024clipEmpty_0024277;

			internal GunShoot _0024self__0024278;

			public _0024(GunShoot self_)
			{
				_0024self__0024278 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (_0024self__0024278.ammoLeft > 0)
					{
						_0024self__0024278.reloadButton.GetComponent<Animation>().Stop();
						WeaponManager.canFire = false;
						_0024self__0024278.parent.GetComponent<Animation>().PlayQueued("Reload", QueueMode.CompleteOthers);
						result = (Yield(2, new WaitForSeconds(0.2f)) ? 1 : 0);
						break;
					}
					_0024self__0024278.clickSound.Play();
					if (_0024self__0024278.clipLeft == 0f && !_0024self__0024278.infiniteAmmo)
					{
						_0024self__0024278.changeWeaponButton.GetComponent<Animation>().Play("flashRed");
					}
					goto IL_0218;
				case 2:
					_0024self__0024278.reloadSound.Play();
					result = (Yield(3, new WaitForSeconds(0.3f)) ? 1 : 0);
					break;
				case 3:
					WeaponManager.canFire = true;
					if (_0024self__0024278.infiniteAmmo)
					{
						_0024self__0024278.clipLeft = _0024self__0024278.clipSize;
					}
					else
					{
						_0024clipEmpty_0024277 = (float)_0024self__0024278.clipSize - _0024self__0024278.clipLeft;
						if (_0024self__0024278.ammoLeft >= _0024self__0024278.clipSize)
						{
							_0024self__0024278.clipLeft = _0024self__0024278.clipSize;
							_0024self__0024278.ammoLeft = (int)((float)_0024self__0024278.ammoLeft - _0024clipEmpty_0024277);
						}
						else if (!(_0024clipEmpty_0024277 <= (float)_0024self__0024278.ammoLeft))
						{
							_0024self__0024278.clipLeft += (float)_0024self__0024278.ammoLeft;
							_0024self__0024278.ammoLeft = 0;
						}
						else
						{
							_0024self__0024278.clipLeft = _0024self__0024278.clipSize;
							_0024self__0024278.ammoLeft = (int)((float)_0024self__0024278.ammoLeft - _0024clipEmpty_0024277);
						}
					}
					goto IL_0218;
				case 1:
					{
						result = 0;
						break;
					}
					IL_0218:
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal GunShoot _0024self__0024279;

		public _0024Reload_0024276(GunShoot self_)
		{
			_0024self__0024279 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024279);
		}
	}

	public GunType gunType;

	public PoolType projectileType;

	public int numProjectiles;

	public float speed;

	public float damage;

	public GUITexture fireButton;

	public GUITexture reloadButton;

	public GUITexture changeWeaponButton;

	public GUIText ammoGUI;

	public bool continuous;

	public Vector3 bulletSpawnOffset;

	public ParticleSystem particleSys;

	public string recoilAnimationName;

	public bool startingWithOneAmmo;

	public float aimVariance;

	public bool infiniteAmmo;

	public bool infiniteClip;

	public int maxAmmo;

	public int[] clipSizes;

	public float[] fireRates;

	public float[] accuracies;

	public AudioSource shotSound;

	public AudioSource clickSound;

	public AudioSource reloadSound;

	public Animation childAnimation;

	internal float nextFire;

	private Transform thisTransform;

	private int shootingFinger;

	private GameObject parent;

	private Light thisLight;

	private int ammoLeft;

	private int clipSize;

	private float fireRate;

	private float accuracy;

	private float clipLeft;

	private GameManager gm;

	public GunShoot()
	{
		numProjectiles = 1;
		speed = 150f;
		damage = 1f;
		bulletSpawnOffset = new Vector3(0f, 0f, 0f);
		clipSizes = new int[4];
		fireRates = new float[4];
		accuracies = new float[4];
		shootingFinger = -1;
	}

	public virtual void Start()
	{
		gm = GameManager.GetInstance();
		thisTransform = transform;
		parent = thisTransform.parent.gameObject;
		thisLight = (Light)parent.GetComponentInChildren(typeof(Light));
		Reset();
	}

	public virtual void Reset()
	{
		int num = Global.gm.GetGunLevel(gunType);
		if (num > 0)
		{
			num--;
		}
		clipSize = clipSizes[num];
		clipLeft = clipSize;
		ammoLeft = clipSize;
		if (startingWithOneAmmo)
		{
			clipLeft = 1f;
		}
		fireRate = fireRates[num];
		accuracy = accuracies[num];
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
		//Discarded unreachable code: IL_01c9
		if (infiniteClip)
		{
			ammoGUI.text = string.Empty + "==";
		}
		else
		{
			ammoGUI.text = string.Empty + clipLeft;
		}
		if (infiniteAmmo)
		{
			ammoGUI.text += "/==";
		}
		else
		{
			ammoGUI.text += "/" + ammoLeft;
		}
		if (!WeaponManager.canFire)
		{
			return;
		}
		if ((StaticFuncs.TestButtonTouchBegan(reloadButton) || Input.GetKeyDown("r")) && clipLeft != (float)clipSize)
		{
			StartCoroutine(Reload());
		}
		//if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WP8Player)
		//{
			int touchCount = Input2.touchCount;
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input2.GetTouch(i);
				if ((fireButton.HitTest(touch.position) || touch.fingerId == shootingFinger) && !(Time.time <= nextFire) && ((continuous && shootingFinger != -1) || touch.phase == TouchPhase.Began) && touch.phase != TouchPhase.Ended)
				{
					shootingFinger = touch.fingerId;
					nextFire = Time.time + fireRate;
					StartCoroutine(Shoot());
					break;
				}
				if (touch.phase == TouchPhase.Ended && touch.fingerId == shootingFinger)
				{
					shootingFinger = -1;
				}
			}
		//}
		/*else */if (!(Time.time <= nextFire) && (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && continuous)))
		{
			nextFire = Time.time + fireRate;
			StartCoroutine(Shoot());
		}
	}

	public virtual IEnumerator Shoot()
	{
		return new _0024Shoot_0024267(this).GetEnumerator();
	}

	public virtual IEnumerator Reload()
	{
		return new _0024Reload_0024276(this).GetEnumerator();
	}

	public virtual void AddAmmo(int numClips)
	{
		if (!infiniteAmmo)
		{
			ammoLeft += clipSize * numClips;
			if (ammoLeft > maxAmmo)
			{
				ammoLeft = maxAmmo;
			}
		}
	}

	public virtual void Main()
	{
	}
}
