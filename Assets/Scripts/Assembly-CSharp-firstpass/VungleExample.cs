using UnityEngine;

public class VungleExample : MonoBehaviour
{
	public GameObject callBackObject;

	public string iosId;

	public string androidId;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		Vungle.init(androidId, iosId);
	}

	public bool IsAdAvailable()
	{
		Debug.Log("is ad available: " + Vungle.isAdvertAvailable());
		return Vungle.isAdvertAvailable();
	}

	public void DisplayAd()
	{
		Vungle.displayAdvert(true);
	}

	public void DisplayInsentiveAd()
	{
		Debug.Log("Vungle displaying incentivized ad");
		Vungle.displayIncentivizedAdvert(true, "user-tag");
	}

	private void OnEnable()
	{
		Vungle.onAdStartedEvent += onAdStartedEvent;
		Vungle.onAdEndedEvent += onAdEndedEvent;
		Vungle.onAdViewedEvent += onAdViewedEvent;
	}

	private void OnDisable()
	{
		Vungle.onAdStartedEvent -= onAdStartedEvent;
		Vungle.onAdEndedEvent -= onAdEndedEvent;
		Vungle.onAdViewedEvent -= onAdViewedEvent;
	}

	private void onAdStartedEvent()
	{
		Debug.Log("onAdStartedEvent");
	}

	private void onAdEndedEvent()
	{
		Debug.Log("onAdEndedEvent");
		callBackObject.SendMessage("InsentiveAdFinished");
	}

	private void onAdViewedEvent(double watched, double length)
	{
		Debug.Log("onAdViewedEvent. watched: " + watched + ", length: " + length);
	}
}
