using BrowserLibCore;
using MagicOnion.Server.Hubs;
using System.Threading.Tasks;
using ElectronicObserver.Window;
using System;

namespace BrowserHost
{
	public class BrowserHostHub : StreamingHubBase<IBrowserHost, IBrowser>, IBrowserHost
	{
		private IGroup Browsers { get; set; }
		public IBrowser Browser => Broadcast(Browsers);

		public Task<BrowserConfiguration> Configuration()
		{
			return Task.Run(() => FormBrowserHost.Instance.ConfigurationCore);
		} 

		public async Task ConnectToBrowser(long handle)
		{
			Browsers = await Group.AddAsync("browser");
			await Task.Run(() => FormBrowserHost.Instance.Connect(this));
			FormBrowserHost.Instance.ConnectToBrowser((IntPtr)handle);
		}

		public async Task SendErrorReport(string exceptionName, string message)
		{
			await Task.Run(() => FormBrowserHost.Instance.SendErrorReport(exceptionName, message));
		}

		public async Task AddLog(int priority, string message)
		{
			await Task.Run(() => FormBrowserHost.Instance.AddLog(priority, message));
		}

		public async Task ConfigurationUpdated(BrowserConfiguration configuration)
		{
			await Task.Run(() => FormBrowserHost.Instance.ConfigurationUpdated(configuration));
		}

		public async Task SetProxyCompleted()
		{
			await Task.Run(() => FormBrowserHost.Instance.SetProxyCompleted());
		}

		public async Task RequestNavigation(string v)
		{
			await Task.Run(() => FormBrowserHost.Instance.RequestNavigation(v));
		}

		public async Task ClearCache()
		{
			await Task.Run(() => FormBrowserHost.Instance.ClearCache());
		}

		public Task<byte[]> GetIconResource()
		{
			return Task.Run(() => FormBrowserHost.Instance.GetIconResource());
		}
	}
}