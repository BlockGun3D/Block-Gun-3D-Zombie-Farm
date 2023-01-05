public abstract class RevMobFullscreen
{
	public abstract void Show();

	public abstract void Hide();

	public virtual void Release()
	{
		Hide();
	}
}
