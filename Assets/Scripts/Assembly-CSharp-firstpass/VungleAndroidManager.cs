using System;
using Prime31;

public class VungleAndroidManager : AbstractManager
{
	public static event Action onVungleAdStartEvent;

	public static event Action onVungleAdEndEvent;

	public static event Action<double, double> onVungleViewEvent;

	static VungleAndroidManager()
	{
		AbstractManager.initialize(typeof(VungleAndroidManager));
	}

	public void onVungleAdStart(string empty)
	{
		if (VungleAndroidManager.onVungleAdStartEvent != null)
		{
			VungleAndroidManager.onVungleAdStartEvent();
		}
	}

	public void onVungleAdEnd(string empty)
	{
		if (VungleAndroidManager.onVungleAdEndEvent != null)
		{
			VungleAndroidManager.onVungleAdEndEvent();
		}
	}

	public void onVungleView(string str)
	{
		if (VungleAndroidManager.onVungleViewEvent != null)
		{
			string[] array = str.Split('-');
			if (array.Length == 2)
			{
				VungleAndroidManager.onVungleViewEvent(double.Parse(array[0]), double.Parse(array[1]));
			}
		}
	}
}
