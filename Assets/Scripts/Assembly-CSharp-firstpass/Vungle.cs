using System;
using System.Collections.Generic;
using Prime31;

public class Vungle
{
	public static event Action onAdStartedEvent;

	public static event Action onAdEndedEvent;

	public static event Action<double, double> onAdViewedEvent;

	static Vungle()
	{
		VungleAndroidManager.onVungleAdStartEvent += adStarted;
		VungleAndroidManager.onVungleAdEndEvent += adFinished;
		VungleAndroidManager.onVungleViewEvent += videoViewed;
	}

	private static void adStarted()
	{
		Vungle.onAdStartedEvent.fire();
	}

	private static void adFinished()
	{
		Vungle.onAdEndedEvent.fire();
	}

	private static void videoViewed(double timeWatched, double totalDuration)
	{
		Vungle.onAdViewedEvent.fire(timeWatched, totalDuration);
	}

	private static void vungleMoviePlayedEvent(Dictionary<string, object> data)
	{
		double param = double.Parse(data["videoViewed"].ToString());
		double param2 = double.Parse(data["videoLength"].ToString());
		Vungle.onAdViewedEvent.fire(param, param2);
	}

	public static void init(string androidAppId, string iosAppId)
	{
		init(androidAppId, iosAppId, -1, VungleGender.None);
	}

	public static void init(string androidAppId, string iosAppId, int age, VungleGender gender)
	{
		if (age > 0)
		{
			VungleAndroid.init(androidAppId, age, gender);
		}
		else
		{
			VungleAndroid.init(androidAppId);
		}
	}

	public static void setSoundEnabled(bool isEnabled)
	{
		VungleAndroid.setSoundEnabled(isEnabled);
	}

	public static bool isAdvertAvailable()
	{
		return VungleAndroid.isVideoAvailable();
	}

	public static void displayAdvert(bool showCloseButtonOnIOS)
	{
		VungleAndroid.displayAdvert();
	}

	public static void displayIncentivizedAdvert(bool showCloseButton, string user)
	{
		VungleAndroid.displayIncentivizedAdvert(showCloseButton, user);
	}
}
