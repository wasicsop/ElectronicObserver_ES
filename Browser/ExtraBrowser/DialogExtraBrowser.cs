using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BrowserLibCore;
using CefSharp;
using CefSharp.WinForms;

namespace Browser.ExtraBrowser
{
	public partial class DialogExtraBrowser : Form
	{
		private ChromiumWebBrowser Browser { get; }

		public DialogExtraBrowser()
		{
			InitializeComponent();

			Text = "CefSharp";
			WindowState = FormWindowState.Normal;

			Browser = new ChromiumWebBrowser("www.duckduckgo.com");
			toolStripContainer.ContentPanel.Controls.Add(Browser);

			Browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
			Browser.LoadingStateChanged += OnLoadingStateChanged;
			Browser.ConsoleMessage += OnBrowserConsoleMessage;
			Browser.StatusMessage += OnBrowserStatusMessage;
			Browser.TitleChanged += OnBrowserTitleChanged;
			Browser.AddressChanged += OnBrowserAddressChanged;

			string version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
				Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

			string environment = string.Format("Environment: {0}, Runtime: {1}",
				System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant(),
				System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

			DisplayOutput(string.Format("{0}, {1}", version, environment));
		}

		private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
		{
			var b = ((ChromiumWebBrowser)sender);

			this.InvokeOnUiThreadIfRequired(() => b.Focus());
		}

		private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
		{
			DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
		}

		private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
		{
			this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
		}

		private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
		{
			SetCanGoBack(args.CanGoBack);
			SetCanGoForward(args.CanGoForward);

			this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
		}

		private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
		{
			this.InvokeOnUiThreadIfRequired(() => Text = args.Title);
		}

		private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
		{
			this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
		}

		private void SetCanGoBack(bool canGoBack)
		{
			this.InvokeOnUiThreadIfRequired(() => backButton.Enabled = canGoBack);
		}

		private void SetCanGoForward(bool canGoForward)
		{
			this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
		}

		private void SetIsLoading(bool isLoading)
		{
			goButton.Text = isLoading ?
				"Stop" :
				"Go";
			/*goButton.Image = isLoading ?
				Properties.Resources.nav_plain_red :
				Properties.Resources.nav_plain_green;*/

			HandleToolStripLayout();
		}

		public void DisplayOutput(string output)
		{
			this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
		}

		private void HandleToolStripLayout(object sender, LayoutEventArgs e)
		{
			HandleToolStripLayout();
		}

		private void HandleToolStripLayout()
		{
			var width = toolStrip1.Width;
			foreach (ToolStripItem item in toolStrip1.Items)
			{
				if (item != urlTextBox)
				{
					width -= item.Width - item.Margin.Horizontal;
				}
			}
			urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
		}

		private void ExitMenuItemClick(object sender, EventArgs e)
		{
			Browser.Dispose();
			Cef.Shutdown();
			Close();
		}

		private void GoButtonClick(object sender, EventArgs e)
		{
			LoadUrl(urlTextBox.Text);
		}

		private void BackButtonClick(object sender, EventArgs e)
		{
			Browser.Back();
		}

		private void ForwardButtonClick(object sender, EventArgs e)
		{
			Browser.Forward();
		}

		private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter)
			{
				return;
			}

			LoadUrl(urlTextBox.Text);
		}

		private void LoadUrl(string url)
		{
			if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
			{
				Browser.Load(url);
			}
		}

		private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
		{
			Browser.ShowDevTools();
		}

		private void DmmPointsButtonClick(object sender, EventArgs e)
		{
			LoadUrl("https://point.dmm.com/choice/pay");
		}

		private void AkashiListButtonClick(object sender, EventArgs e)
		{
			LoadUrl("https://akashi-list.me/");
		}
	}

	public static class ControlExtensions
	{
		/// <summary>
		/// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
		/// </summary>
		/// <param name="control">the control for which the update is required</param>
		/// <param name="action">action to be performed on the control</param>
		public static void InvokeOnUiThreadIfRequired(this System.Windows.Forms.Control control, Action action)
		{
			//If you are planning on using a similar function in your own code then please be sure to
			//have a quick read over https://stackoverflow.com/questions/1874728/avoid-calling-invoke-when-the-control-is-disposed
			//No action
			if (control.Disposing || control.IsDisposed || !control.IsHandleCreated)
			{
				return;
			}

			if (control.InvokeRequired)
			{
				control.BeginInvoke(action);
			}
			else
			{
				action.Invoke();
			}
		}
	}
}
