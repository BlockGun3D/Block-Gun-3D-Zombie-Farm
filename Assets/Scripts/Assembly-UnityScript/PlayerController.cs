using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using Boo.Lang.Runtime;
using CompilerGenerated;
using UnityEngine;

[Serializable]
public class PlayerController : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024BeenHit_0024295 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal float _0024_0024178_0024296;

			internal Rect _0024_0024179_0024297;

			internal int _0024_0024180_0024298;

			internal Rect _0024_0024181_0024299;

			internal float _0024damage_0024300;

			internal PlayerController _0024self__0024301;

			public _0024(float damage, PlayerController self_)
			{
				_0024damage_0024300 = damage;
				_0024self__0024301 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024301.timeHit = Time.time;
					_0024self__0024301.health -= _0024damage_0024300;
					if (_0024self__0024301.health <= 0f)
					{
						int num = (_0024_0024180_0024298 = 0);
						Rect rect = (_0024_0024181_0024299 = _0024self__0024301.healthBar.pixelInset);
						float num3 = (_0024_0024181_0024299.width = _0024_0024180_0024298);
						Rect rect3 = (_0024self__0024301.healthBar.pixelInset = _0024_0024181_0024299);
					}
					else
					{
						float num4 = (_0024_0024178_0024296 = _0024self__0024301.healthBarWidth * (_0024self__0024301.health / (_0024self__0024301.fullHealth + (float)_0024self__0024301.gm.GetArmorLevel() * _0024self__0024301.fullHealth / 2f)));
						Rect rect4 = (_0024_0024179_0024297 = _0024self__0024301.healthBar.pixelInset);
						float num6 = (_0024_0024179_0024297.width = _0024_0024178_0024296);
						Rect rect6 = (_0024self__0024301.healthBar.pixelInset = _0024_0024179_0024297);
					}
					if (!(_0024self__0024301.health > 0.001f))
					{
						_0024self__0024301.GetComponent<AudioSource>().Play();
						result = (Yield(2, new WaitForSeconds(_0024self__0024301.timeDamageIsVisible / 2f)) ? 1 : 0);
						break;
					}
					goto IL_01b4;
				case 2:
					_0024self__0024301.gm.SetGameState(_0024self__0024301.deathState);
					goto IL_01b4;
				case 1:
					{
						result = 0;
						break;
					}
					IL_01b4:
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal float _0024damage_0024302;

		internal PlayerController _0024self__0024303;

		public _0024BeenHit_0024295(float damage, PlayerController self_)
		{
			_0024damage_0024302 = damage;
			_0024self__0024303 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024damage_0024302, _0024self__0024303);
		}
	}

	public float health;

	public float timeDamageIsVisible;

	public GUITexture healthBar;

	public GUITexture hurtImage;

	public GameState deathState;

	private float timeHit;

	private float healthBarWidth;

	private float fullHealth;

	private Color hurtImageColor;

	private GameManager gm;

	public PlayerController()
	{
		health = 0.8f;
		timeDamageIsVisible = 0.2f;
		timeHit = -1f;
	}

	public virtual void Start()
	{
		gm = GameManager.GetInstance();
		healthBarWidth = healthBar.pixelInset.width;
		int width = Screen.width;
		Rect pixelInset = hurtImage.pixelInset;
		float num2 = (pixelInset.width = width);
		Rect rect2 = (hurtImage.pixelInset = pixelInset);
		int height = Screen.height;
		Rect pixelInset2 = hurtImage.pixelInset;
		float num4 = (pixelInset2.height = height);
		Rect rect4 = (hurtImage.pixelInset = pixelInset2);
		hurtImageColor = hurtImage.color;
		hurtImage.enabled = false;
		fullHealth = health;
		health = fullHealth + (float)gm.GetArmorLevel() * fullHealth / 2f;
		if (RuntimeServices.EqualityOperator(new __PlayerController_Start_0024callable0_002427_16__(gm.GetArmorLevel), 1))
		{
			healthBar.color = new Color(0.6f, 0.6f, 1f, 1f);
		}
		else if (RuntimeServices.EqualityOperator(new __PlayerController_Start_0024callable0_002427_16__(gm.GetArmorLevel), 2))
		{
			healthBar.color = new Color(0.3f, 0.3f, 1f, 1f);
		}
	}

	public virtual void Reset()
	{
		transform.position = Global.playerStartPos;
		transform.rotation = Global.playerStartRot;
		ResetHealth();
	}

	public virtual void ResetHealth()
	{
		health = fullHealth + (float)gm.GetArmorLevel() * fullHealth / 2f;
		float num = healthBarWidth;
		Rect pixelInset = healthBar.pixelInset;
		float num3 = (pixelInset.width = num);
		Rect rect2 = (healthBar.pixelInset = pixelInset);
		hurtImage.enabled = false;
		timeHit = -1f;
		if (RuntimeServices.EqualityOperator(new __PlayerController_Start_0024callable0_002427_16__(gm.GetArmorLevel), 1))
		{
			healthBar.color = new Color(0.6f, 0.6f, 1f, 1f);
		}
		else if (RuntimeServices.EqualityOperator(new __PlayerController_Start_0024callable0_002427_16__(gm.GetArmorLevel), 2))
		{
			healthBar.color = new Color(0.3f, 0.3f, 1f, 1f);
		}
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
		if (!(timeHit <= 0f))
		{
			if (!(Time.time - timeHit >= timeDamageIsVisible))
			{
				hurtImage.enabled = true;
				hurtImage.color = Color.Lerp(hurtImageColor, new Color(0f, 0f, 0f, 0f), (Time.time - timeHit) / timeDamageIsVisible);
			}
			else
			{
				hurtImage.enabled = false;
				timeHit = -1f;
			}
		}
	}

	public virtual IEnumerator BeenHit(float damage)
	{
		return new _0024BeenHit_0024295(damage, this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
