using UnityEngine;

public class OpenAppLink : MonoBehaviour
{
	public string iosAppUrl;

	public string andAppUrl;

	private void Start()
	{
		if (andAppUrl == string.Empty)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void GoToAppUrl()
	{
		Application.OpenURL(andAppUrl);
	}
}
