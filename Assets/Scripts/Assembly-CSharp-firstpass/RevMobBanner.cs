public abstract class RevMobBanner
{
	public abstract void Show();

	public abstract void Hide();

	public virtual void Release()
	{
		Hide();
	}
}
