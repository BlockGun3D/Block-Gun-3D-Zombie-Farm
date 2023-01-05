using System;
using UnityEngine;

[Serializable]
public class WandFlicker : MonoBehaviour
{
	public float flickerAmount;

	public float flickerFrequency;

	public float freqVariance;

	private float lastFlick;

	private float randInterval;

	private float originalVal;

	public virtual void Start()
	{
		originalVal = GetComponent<Light>().intensity;
		randInterval = GetInterval();
	}

	public virtual void Update()
	{
		if (!(Time.time - lastFlick <= randInterval))
		{
			GetComponent<Light>().intensity = StaticFuncs.RandomVal(originalVal, flickerAmount);
			lastFlick = Time.time;
			randInterval = GetInterval();
		}
	}

	private float GetInterval()
	{
		return StaticFuncs.RandomVal(flickerFrequency, freqVariance);
	}

	public virtual void Main()
	{
	}
}
