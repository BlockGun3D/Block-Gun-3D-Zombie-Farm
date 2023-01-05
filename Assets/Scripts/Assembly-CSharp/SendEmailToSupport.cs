using UnityEngine;

public class SendEmailToSupport : MonoBehaviour
{
	public string appName = "appName";

	private void SendEmail()
	{
		string text = "support@wizardgames.ca";
		string text2 = MyEscapeURL("Feedback on " + appName);
		string text3 = MyEscapeURL(string.Empty);
		Application.OpenURL("mailto:" + text + "?subject=" + text2 + "&body=" + text3);
	}

	private string MyEscapeURL(string url)
	{
		return WWW.EscapeURL(url).Replace("+", "%20");
	}
}
