using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class HealthController : MonoBehaviour
{
	public float health;

	public float timeDamageIsVisible;

	public Color damageColor;

	public bool isBoss;

	public Renderer colorChangeObject;

	private float timeHit;

	private GameManager gm;

	private bool dead;

	private float normHealth;

	public HealthController()
	{
		health = 1f;
		timeDamageIsVisible = 0.2f;
		damageColor = Color.red;
		timeHit = -1f;
	}

	public virtual void Awake()
	{
		gm = GameManager.GetInstance();
		normHealth = health;
		health = normHealth + normHealth * 0.35f * (float)(Global.difficulty - 1);
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
				ChangeColor(damageColor);
			}
			else
			{
				ChangeColor(Color.white);
			}
		}
	}

	public virtual void BeenHit(float damage)
	{
		if (enabled)
		{
			timeHit = Time.time;
			health -= damage;
			if (!(health > 0f) && !dead)
			{
				dead = true;
				SendMessage("Die");
			}
		}
	}

	public virtual void ChangeColor(Color color)
	{
		if ((bool)colorChangeObject)
		{
			int i = 0;
			Material[] materials = colorChangeObject.materials;
			for (int length = materials.Length; i < length; i++)
			{
				color.a = materials[i].color.a;
				materials[i].color = color;
			}
			return;
		}
		if ((bool)GetComponent<Renderer>())
		{
			color.a = GetComponent<Renderer>().material.color.a;
			GetComponent<Renderer>().material.color = color;
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
			if ((bool)transform.GetComponent<Renderer>())
			{
				color.a = transform.GetComponent<Renderer>().material.color.a;
				UnityRuntimeServices.Update(enumerator, transform);
				transform.GetComponent<Renderer>().material.color = color;
				UnityRuntimeServices.Update(enumerator, transform);
			}
		}
	}

	public virtual void Reset(float h)
	{
		dead = false;
		health = normHealth + normHealth * 0.25f * (float)(Global.difficulty - 1);
	}

	public virtual void DieIfNotBoss()
	{
		if (!isBoss)
		{
			gameObject.SendMessage("Die");
		}
	}

	public virtual void Main()
	{
	}
}
