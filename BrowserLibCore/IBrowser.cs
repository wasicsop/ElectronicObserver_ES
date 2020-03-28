namespace BrowserLibCore
{
	public interface IBrowser
	{
		void ConfigurationChanged(BrowserConfiguration config);
		void InitialAPIReceived();
		void Navigate(string url);
		void CloseBrowser();
		void SetProxy(string v);
	}
}