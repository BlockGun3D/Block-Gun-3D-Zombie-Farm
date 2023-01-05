using System.Collections.Generic;
using UnityEngine;

public abstract class RevMob
{
	public enum Test
	{
		DISABLED = 0,
		WITH_ADS = 1,
		WITHOUT_ADS = 2
	}

	public enum Position
	{
		BOTTOM = 0,
		TOP = 1
	}

	protected static readonly string REVMOB_VERSION = "7.4.2";

	protected string gameObjectName;

	public abstract void PrintEnvironmentInformation();

	public abstract void SetTestingMode(Test test);

	public abstract void SetTimeoutInSeconds(int timeout);

	public abstract bool IsDevice();

	public abstract RevMobFullscreen ShowFullscreen(string placementId);

	public abstract RevMobFullscreen CreateFullscreen(string placementId);

	public abstract RevMobBanner CreateBanner(Position position, int x, int y, int w, int h);

	public abstract void ShowBanner(Position position, int x, int y, int w, int h);

	public abstract void HideBanner();

	public abstract RevMobLink OpenAdLink(string placementId);

	public abstract RevMobLink CreateAdLink(string placementId);

	public abstract RevMobPopup ShowPopup(string placementId);

	public abstract RevMobPopup CreatePopup(string placementId);

	public static RevMob Start(Dictionary<string, string> appIds)
	{
		return Start(appIds, null);
	}

	public static RevMob Start(Dictionary<string, string> appIds, string gameObjectName)
	{
		Debug.Log("Creating RevMob Session");
		return new RevMobAndroid(appIds["Android"], gameObjectName);
	}

	public RevMobFullscreen ShowFullscreen()
	{
		return ShowFullscreen(null);
	}

	public RevMobFullscreen CreateFullscreen()
	{
		return CreateFullscreen(null);
	}

	public RevMobBanner CreateBanner()
	{
		return CreateBanner(Position.BOTTOM, 0, 0, 0, 0);
	}

	public RevMobBanner CreateBanner(Position position)
	{
		return CreateBanner(position, 0, 0, 0, 0);
	}

	public void ShowBanner()
	{
		ShowBanner(Position.BOTTOM, 0, 0, 0, 0);
	}

	public void ShowBanner(Position position)
	{
		ShowBanner(position, 0, 0, 0, 0);
	}

	public RevMobLink OpenAdLink()
	{
		return OpenAdLink(null);
	}

	public RevMobLink CreateAdLink()
	{
		return CreateAdLink(null);
	}

	public RevMobPopup ShowPopup()
	{
		return ShowPopup(null);
	}

	public RevMobPopup CreatePopup()
	{
		return CreatePopup(null);
	}
}
