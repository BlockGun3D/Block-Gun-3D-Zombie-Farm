public interface IRevMobListener
{
	void SessionIsStarted();

	void SessionNotStarted(string message);

	void AdDidReceive(string revMobAdType);

	void AdDidFail(string revMobAdType);

	void AdDisplayed(string revMobAdType);

	void UserClickedInTheAd(string revMobAdType);

	void UserClosedTheAd(string revMobAdType);

	void InstallDidReceive(string message);

	void InstallDidFail(string message);
}
