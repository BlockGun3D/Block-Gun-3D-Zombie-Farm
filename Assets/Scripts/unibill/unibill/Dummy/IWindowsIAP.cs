namespace unibill.Dummy
{
	public interface IWindowsIAP
	{
		void Initialise(IWindowsIAPCallback callback, int delayInMilliseconds);

		void Purchase(string productId);

		void EnumerateLicenses();
	}
}
