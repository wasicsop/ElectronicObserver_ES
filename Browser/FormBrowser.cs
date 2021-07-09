using Browser.CefOp;
using BrowserLibCore;
using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Browser.ExtraBrowser;
using Browser.Properties;
using Grpc.Core;
using MagicOnion.Client;
using Translation = Browser.Properties.Resources;

namespace Browser
{
	/// <summary>
	/// ブラウザを表示するフォームです。
	/// </summary>
	/// <remarks>thx KanColleViewer!</remarks>
	// [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single/*, IncludeExceptionDetailInFaults = true*/)]
	public partial class FormBrowser : Form, BrowserLibCore.IBrowser
	{
		private readonly Size KanColleSize = new Size(1200, 720);
		private string BrowserCachePath => BrowserConstants.CachePath;

		private readonly string StyleClassID = Guid.NewGuid().ToString().Substring(0, 8);
		private bool RestoreStyleSheet = false;

		private string Host { get; }
		private int Port { get; }

		private BrowserLibCore.IBrowserHost BrowserHost { get; set; }

		private BrowserConfiguration Configuration { get; set; }

		// 親プロセスが生きているか定期的に確認するためのタイマー
		private System.Windows.Forms.Timer HeartbeatTimer { get; } = new System.Windows.Forms.Timer();

		private ChromiumWebBrowser Browser { get; set; }

		private string ProxySettings { get; set; } = null;


		private bool _styleSheetApplied;

		/// <summary>
		/// スタイルシートの変更が適用されているか
		/// </summary>
		private bool StyleSheetApplied
		{
			get => _styleSheetApplied;
			set
			{
				if (value)
				{
					//Browser.Anchor = AnchorStyles.None;
					ApplyZoom();
					SizeAdjuster_SizeChanged(null, new EventArgs());
				}
				else
				{
					SizeAdjuster.SuspendLayout();
					if (IsBrowserInitialized)
					{
						//Browser.Anchor = AnchorStyles.Top | AnchorStyles.Left;
						Browser.Location = new Point(0, 0);
						Browser.MinimumSize = new Size(0, 0);
						Browser.Size = SizeAdjuster.Size;
					}

					SizeAdjuster.ResumeLayout();
				}

				_styleSheetApplied = value;
			}
		}

		/// <summary>
		/// 艦これが読み込まれているかどうか
		/// </summary>
		private bool IsKanColleLoaded { get; set; }

		private VolumeManager _volumeManager = null;

		private string _lastScreenShotPath;

		private readonly string KanColleUrl = "http://www.dmm.com/netgame/social/-/gadgets/=/app_id=854854/";
		private NumericUpDown ToolMenu_Other_Volume_VolumeControl =>
			(NumericUpDown) ((ToolStripControlHost) ToolMenu_Other_Volume.DropDownItems[
				"ToolMenu_Other_Volume_VolumeControlHost"]).Control;

		private PictureBox ToolMenu_Other_LastScreenShot_Control =>
			(PictureBox) ((ToolStripControlHost) ToolMenu_Other_LastScreenShot.DropDownItems[
				"ToolMenu_Other_LastScreenShot_ImageHost"]).Control;


		/// <summary>
		/// </summary>
		/// <param name="serverUri">ホストプロセスとの通信用URL</param>
		public FormBrowser(string host, int port)
		{
			// Debugger.Launch();

			Host = host;
			Port = port;

			CultureInfo c = CultureInfo.CurrentCulture;
			CultureInfo ui = CultureInfo.CurrentUICulture;
			if (c.Name != "en-US" && c.Name != "ja-JP" && c.Name != "ko-KR")
			{
				c = new CultureInfo("en-US");
			}

			if (ui.Name != "en-US" && ui.Name != "ja-JP" && ui.Name != "ko-KR")
			{
				ui = new CultureInfo("en-US");
			}

			Thread.CurrentThread.CurrentCulture = c;
			Thread.CurrentThread.CurrentUICulture = ui;

			InitializeComponent();

			StyleSheetApplied = false;


			// 音量設定用コントロールの追加
			{
				NumericUpDown control = new NumericUpDown
				{
					Name = "ToolMenu_Other_Volume_VolumeControl",
					Maximum = 100,
					TextAlign = HorizontalAlignment.Right,
					Font = ToolMenu_Other_Volume.Font,
					Tag = false
				};

				control.ValueChanged += ToolMenu_Other_Volume_ValueChanged;

				var toolStripControlHost = new ToolStripControlHost(control, "ToolMenu_Other_Volume_VolumeControlHost");

				control.Size = new Size(toolStripControlHost.Width - control.Margin.Horizontal,
					toolStripControlHost.Height - control.Margin.Vertical);
				control.Location = new Point(control.Margin.Left, control.Margin.Top);


				ToolMenu_Other_Volume.DropDownItems.Add(toolStripControlHost);
			}

			// スクリーンショットプレビューコントロールの追加
			{
				double zoomrate = 0.5;
				PictureBox control = new PictureBox
				{
					Name = "ToolMenu_Other_LastScreenShot_Image",
					SizeMode = PictureBoxSizeMode.Zoom,
					Size = new Size((int) (KanColleSize.Width * zoomrate), (int) (KanColleSize.Height * zoomrate)),
					Margin = new Padding(),
					Image = new Bitmap((int) (KanColleSize.Width * zoomrate), (int) (KanColleSize.Height * zoomrate),
						PixelFormat.Format24bppRgb)
				};
				using (var g = Graphics.FromImage(control.Image))
				{
					g.Clear(SystemColors.Control);
					g.DrawString(Translation.NoScreenshotYet + "\r\n", Font, Brushes.Black, new Point(4, 4));
				}

				var toolStripControlHost = new ToolStripControlHost(control, "ToolMenu_Other_LastScreenShot_ImageHost")
				{
					Size = new Size(control.Width + control.Margin.Horizontal,
						control.Height + control.Margin.Vertical),
					AutoSize = false
				};

				control.Location = new Point(control.Margin.Left, control.Margin.Top);

				toolStripControlHost.Click += ToolMenu_Other_LastScreenShot_ImageHost_Click;

				ToolMenu_Other_LastScreenShot.DropDownItems.Insert(0, toolStripControlHost);
			}

			Translate();
		}

		public void Translate()
		{
			ContextMenuTool_ShowToolMenu.Text = Translation.ShowToolMenu;
			ToolMenu_ScreenShot.Text = Translation.Strip_ScreenShot;
			ToolMenu_Zoom.Text = Translation.Strip_Zoom;
			ToolMenu_Mute.Text = Translation.Strip_Mute;
			ToolMenu_Refresh.Text = Translation.Strip_Refresh;
			ToolMenu_NavigateToLogInPage.Text = Translation.NavigateToLogInPage;
			ToolMenu_Other.Text = Translation.Other;

			ToolMenu_Other_ScreenShot.Text = Translation.ToolMenu_Other_ScreenShot;
			
			ToolMenu_Other_LastScreenShot.Text = Translation.ToolMenu_Other_LastScreenShot;
			ToolMenu_Other_LastScreenShot_OpenScreenShotFolder.Text = Translation.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder;
			ToolMenu_Other_LastScreenShot_CopyToClipboard.Text = Translation.LastScreenShot_CopyToClipboard;

			ToolMenu_Other_Zoom.Text = Translation.ToolMenu_Other_Zoom;
			ToolMenu_Other_Zoom_Current.Text = Translation.Other_Zoom_Current;
			ToolMenu_Other_Zoom_Fit.Text = Translation.Zoom_to_Fit;

			ToolMenu_Other_Volume.Text = Translation.ToolMenu_Other_Volume;
			ToolMenu_Other_Mute.Text = Translation.ToolMenu_Other_Mute;
			ToolMenu_Other_Refresh.Text = Translation.ToolMenu_Other_Refresh;
			ToolMenu_Other_NavigateToLogInPage.Text = Translation.ToolMenu_Other_NavigateToLogInPage;
			ToolMenu_Other_Navigate.Text = Translation.ToolMenu_Other_Navigate;
			ToolMenu_Other_AppliesStyleSheet.Text = Translation.ToolMenu_Other_AppliesStyleSheet;

			ToolMenu_Other_Alignment.Text = Translation.ToolMenu_Other_Alignment;
			ToolMenu_Other_Alignment_Top.Text = Translation.Alignment_Top;
			ToolMenu_Other_Alignment_Bottom.Text = Translation.Alignment_Bottom;
			ToolMenu_Other_Alignment_Left.Text = Translation.Alignment_Left;
			ToolMenu_Other_Alignment_Right.Text = Translation.Alignment_Right;
			ToolMenu_Other_Alignment_Invisible.Text = Translation.Alignment_Invisible;

			ToolMenu_Other_ClearCache.Text = $"{Translation.ClearCache}(&H)";
			ToolMenu_Other_OpenDevTool.Text = Translation.OpenDevTool;

			Text = Translation.Title;
		}

		private void FormBrowser_Load(object sender, EventArgs e)
		{
			SetWindowLong(Handle, GWL_STYLE, WS_CHILD);

			// ホストプロセスに接続
			Channel grpChannel = new Channel(Host, Port, ChannelCredentials.Insecure);
			BrowserHost =
				StreamingHubClient.Connect<BrowserLibCore.IBrowserHost, BrowserLibCore.IBrowser>(grpChannel, this);

			ConfigurationChanged();

			// ウィンドウの親子設定＆ホストプロセスから接続してもらう
			Task.Run(async () => await BrowserHost.ConnectToBrowser((long) Handle)).Wait();

			// 親ウィンドウが生きているか確認
			HeartbeatTimer.Tick += async (sender2, e2) =>
			{
				try
				{
					bool alive = await BrowserHost.IsServerAlive();
				}
				catch (Exception e)
				{
					Debug.WriteLine("host died");
					Exit();
				}
			};
			HeartbeatTimer.Interval = 2000; // 2秒ごと　
			HeartbeatTimer.Start();


			SetIconResource();

			InitializeBrowser();
		}


		/// <summary>
		/// ブラウザを初期化します。
		/// 最初の呼び出しのみ有効です。二回目以降は何もしません。
		/// </summary>
		void InitializeBrowser()
		{
			if (Browser != null) return;
			if (ProxySettings == null) return;

			CefSettings? settings;

			try
			{
				settings = new CefSettings
				{
					CachePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
						BrowserCachePath),
					Locale = "ja",
					AcceptLanguageList = "ja,en-US,en", // todo: いる？
					LogSeverity = Configuration.SavesBrowserLog ? LogSeverity.Error : LogSeverity.Disable,
					LogFile = "BrowserLog.log",
				};
			}
			catch (BadImageFormatException e)
			{
				if (MessageBox.Show(Translation.InstallVisualCpp, Translation.CefSharpLoadErrorTitle, 
					MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
				    == DialogResult.Yes)
				{
					ProcessStartInfo psi = new ProcessStartInfo
					{
						FileName = @"https://support.microsoft.com/en-us/topic/the-latest-supported-visual-c-downloads-2647da03-1eea-4433-9aff-95f26a218cc0",
						UseShellExecute = true
					};
					Process.Start(psi);
				}
				throw;
			}

			if (!Configuration.HardwareAccelerationEnabled)
				settings.DisableGpuAcceleration();

			settings.CefCommandLineArgs.Add("proxy-server", ProxySettings);
			settings.CefCommandLineArgs.Add("limit-fps", "60");

			// prevent CEF from taking over media keys
			if(settings.CefCommandLineArgs.ContainsKey("disable-features"))
			{
				List<string> disabledFeatures = settings.CefCommandLineArgs["disable-features"]
					.Split(",")
					.ToList();

				disabledFeatures.Add("HardwareMediaKeyHandling");

				settings.CefCommandLineArgs["disable-features"] = string.Join(",", disabledFeatures);
			}
			else
			{
				settings.CefCommandLineArgs.Add("disable-features", "HardwareMediaKeyHandling");
			}


			if (Configuration.ForceColorProfile)
				settings.CefCommandLineArgs.Add("force-color-profile", "srgb");
			CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
			Cef.Initialize(settings, false, (IBrowserProcessHandler) null);
			
			var requestHandler = new CustomRequestHandler(pixiSettingEnabled: Configuration.PreserveDrawingBuffer);
			requestHandler.RenderProcessTerminated += (mes) => AddLog(3, mes);
			
			Browser = new ChromiumWebBrowser(KanColleUrl)
			{
				Dock = DockStyle.Fill,
				Size = SizeAdjuster.Size,
				RequestHandler = requestHandler,
				MenuHandler = new MenuHandler(),
				KeyboardHandler = new KeyboardHandler(),
				DragHandler = new DragHandler(),
			};
			Browser.BrowserSettings.StandardFontFamily = "Microsoft YaHei"; // Fixes text rendering position too high
			Browser.LoadingStateChanged += Browser_LoadingStateChanged;
			Browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
			SizeAdjuster.Controls.Add(Browser);
		}


		void Exit()
		{
			// if (!BrowserHost.Closed)
			{
				// BrowserHost.Close();
				HeartbeatTimer.Stop();
				Task.Run(async () => await BrowserHost.DisposeAsync()).Wait();
				Cef.Shutdown();
				Application.Exit();
			}
		}

		void BrowserHostChannel_Faulted(Exception e)
		{
			// 親と通信できなくなったら終了する
			Exit();
		}

		public void CloseBrowser()
		{
			HeartbeatTimer.Stop();
			// リモートコールでClose()呼ぶのばヤバそうなので非同期にしておく
			BeginInvoke((Action) (() => Exit()));
		}

		public async void ConfigurationChanged()
		{
			BrowserConfiguration conf = await BrowserHost.Configuration();

			Configuration = conf;

			SizeAdjuster.AutoScroll = Configuration.IsScrollable;
			ToolMenu_Other_Zoom_Fit.Checked = Configuration.ZoomFit;
			ApplyZoom();
			ToolMenu_Other_AppliesStyleSheet.Checked = Configuration.AppliesStyleSheet;
			ToolMenu.Dock = (DockStyle) Configuration.ToolMenuDockStyle;
			ToolMenu.Visible = Configuration.IsToolMenuVisible;

			SizeAdjuster.BackColor = System.Drawing.Color.FromArgb(unchecked((int) Configuration.BackColor));
			ToolMenu.BackColor = System.Drawing.Color.FromArgb(unchecked((int) Configuration.BackColor));
			ToolMenu_Other_ClearCache.Visible = conf.EnableDebugMenu;
		}

		private void ConfigurationUpdated()
		{
			BrowserHost.ConfigurationUpdated(Configuration);
		}

		private void AddLog(int priority, string message)
		{
			BrowserHost.AddLog(priority, message);
		}

		private void SendErrorReport(string exceptionName, string message)
		{
			BrowserHost.SendErrorReport(exceptionName, message);
		}


		public void InitialAPIReceived()
		{
			IsKanColleLoaded = true;

			//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
			ApplyStyleSheet();
			ApplyZoom();
			DestroyDMMreloadDialog();

			//起動直後はまだ音声が鳴っていないのでミュートできないため、この時点で有効化
			SetVolumeState();
		}


		private void SizeAdjuster_SizeChanged(object sender, EventArgs e)
		{
			if (!StyleSheetApplied)
			{
				if (Browser != null)
				{
					Browser.Location = new Point(0, 0);
					Browser.Size = SizeAdjuster.Size;
				}

				return;
			}

			ApplyZoom();
		}

		private void CenteringBrowser()
		{
			if (SizeAdjuster.Width == 0 || SizeAdjuster.Height == 0) return;
			int x = Browser.Location.X, y = Browser.Location.Y;
			bool isScrollable = Configuration.IsScrollable;
			Browser.Dock = DockStyle.None;

			if (!isScrollable || Browser.Width <= SizeAdjuster.Width)
			{
				x = (SizeAdjuster.Width - Browser.Width) / 2;
			}

			if (!isScrollable || Browser.Height <= SizeAdjuster.Height)
			{
				y = (SizeAdjuster.Height - Browser.Height) / 2;
			}

			//if ( x != Browser.Location.X || y != Browser.Location.Y )
			Browser.Location = new Point(x, y);
		}

		private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
		{
			// DocumentCompleted に相当?
			// note: 非 UI thread からコールされるので、何かしら UI に触る場合は適切な処置が必要

			if (e.IsLoading)
				return;

			if (Browser.Address.Contains("redirect"))
			{
				SetCookie();
				Browser.Refresh();
			}

			/*if (Browser.Address.Contains("login/=/path="))
		    {
		        SetCookie();
                Browser.ExecuteScriptAsync(Properties.Resources.RemoveWelcomePopup);
		        Browser.ExecuteScriptAsync(Properties.Resources.RemoveServicePopup);
            }*/

			BeginInvoke((Action) (() =>
			{
				ApplyStyleSheet();

				ApplyZoom();
				DestroyDMMreloadDialog();
			}));
		}


		private bool IsBrowserInitialized =>
			Browser != null &&
			Browser.IsBrowserInitialized;


		public Action<Exception> Faulted
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		private IFrame GetMainFrame()
		{
			if (!IsBrowserInitialized) return null;

			var browser = Browser.GetBrowser();
			var frame = browser.MainFrame;

			if (frame?.Url?.Contains(@"http://www.dmm.com/netgame/social/") ?? false)
				return frame;

			return null;
		}

		private IFrame GetGameFrame()
		{
			if (!IsBrowserInitialized) return null;

			var browser = Browser.GetBrowser();
			var frames = browser.GetFrameIdentifiers()
				.Select(id => browser.GetFrame(id));

			return frames.FirstOrDefault(f => f?.Url?.Contains(@"http://osapi.dmm.com/gadgets/") ?? false);
		}

		private IFrame GetKanColleFrame()
		{
			if (!IsBrowserInitialized)
				return null;

			var browser = Browser.GetBrowser();
			var frames = browser.GetFrameIdentifiers()
				.Select(id => browser.GetFrame(id));

			return frames.FirstOrDefault(f => f?.Url?.Contains(@"/kcs2/index.php") ?? false);
		}


		/// <summary>
		/// スタイルシートを適用します。
		/// </summary>
		private void ApplyStyleSheet()
		{
			if (!IsBrowserInitialized) return;
			if (!Configuration.AppliesStyleSheet && !RestoreStyleSheet) return;

			try
			{
				var mainframe = GetMainFrame();
				var gameframe = GetGameFrame();
				if (mainframe == null || gameframe == null)
					return;

				if (RestoreStyleSheet)
				{
					mainframe.EvaluateScriptAsync(string.Format(Properties.Resources.RestoreScript, StyleClassID));
					gameframe.EvaluateScriptAsync(string.Format(Properties.Resources.RestoreScript, StyleClassID));
					gameframe.EvaluateScriptAsync("document.body.style.backgroundColor = \"#000000\";");
					StyleSheetApplied = false;
					RestoreStyleSheet = false;
				}
				else
				{
					mainframe.EvaluateScriptAsync(string.Format(Properties.Resources.PageScript, StyleClassID));
					gameframe.EvaluateScriptAsync(string.Format(Properties.Resources.FrameScript, StyleClassID));
					gameframe.EvaluateScriptAsync("document.body.style.backgroundColor = \"#000000\";");
				}

				StyleSheetApplied = true;
			}
			catch (Exception ex)
			{
				SendErrorReport(ex.ToString(), Resources.FailedToApplyStylesheet);
			}
		}

		/// <summary>
		/// DMMによるページ更新ダイアログを非表示にします。
		/// </summary>
		private void DestroyDMMreloadDialog()
		{
			if (!IsBrowserInitialized)
				return;

			if (!Configuration.IsDMMreloadDialogDestroyable)
				return;

			try
			{
				var mainframe = GetMainFrame();
				if (mainframe == null)
					return;

				mainframe.EvaluateScriptAsync(Properties.Resources.DMMScript);
			}
			catch (Exception ex)
			{
				SendErrorReport(ex.ToString(), Translation.FailedToHideDmmRefreshDialog);
			}
		}


		// タイミングによっては(特に起動時)、ブラウザの初期化が完了する前に Navigate() が呼ばれることがある
		// その場合ロードに失敗してブラウザが白画面でスタートしてしまう（手動でログインページを開けば続行は可能だが）
		// 応急処置として失敗したとき後で再試行するようにしてみる
		private string navigateCache = null;

		private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
		{
			if (IsBrowserInitialized && navigateCache != null)
			{
				// ロードが完了したので再試行
				string url = navigateCache; // 非同期コールするのでコピーを取っておく必要がある
				BeginInvoke((Action) (() => Navigate(url)));
				navigateCache = null;
			}
		}

		/// <summary>
		/// 指定した URL のページを開きます。
		/// </summary>
		public void Navigate(string url)
		{
			if (url != Configuration.LogInPageURL || !Configuration.AppliesStyleSheet)
				StyleSheetApplied = false;
			Browser.Load(url);

			if (!IsBrowserInitialized)
			{
				// 大方ロードできないのであとで再試行する
				navigateCache = url;
			}
		}

		/// <summary>
		/// ブラウザを再読み込みします。
		/// </summary>
		private void RefreshBrowser() => RefreshBrowser(false);

		/// <summary>
		/// ブラウザを再読み込みします。
		/// </summary>
		/// <param name="ignoreCache">キャッシュを無視するか。</param>
		private void RefreshBrowser(bool ignoreCache)
		{
			if (!Configuration.AppliesStyleSheet)
				StyleSheetApplied = false;

			Browser.Reload(ignoreCache);
		}

		/// <summary>
		/// ズームを適用します。
		/// </summary>
		private void ApplyZoom()
		{
			if (!IsBrowserInitialized) return;

			double zoomRate = Configuration.ZoomRate;
			bool fit = Configuration.ZoomFit && StyleSheetApplied;

			double zoomFactor;

			if (fit)
			{
				double rateX = (double) SizeAdjuster.Width / KanColleSize.Width;
				double rateY = (double) SizeAdjuster.Height / KanColleSize.Height;
				zoomFactor = Math.Min(rateX, rateY);
			}
			else
			{
				zoomFactor = Math.Clamp(zoomRate, 0.1, 10);
			}


			Browser.SetZoomLevel(Math.Log(zoomFactor, 1.2));


			if (StyleSheetApplied)
			{
				Browser.Size = Browser.MinimumSize = new Size(
					(int) (KanColleSize.Width * zoomFactor),
					(int) (KanColleSize.Height * zoomFactor)
				);

				CenteringBrowser();
			}

			if (fit)
			{
				ToolMenu_Other_Zoom_Current.Text = Resources.Other_Zoom_Current_Fit;
			}
			else
			{
				ToolMenu_Other_Zoom_Current.Text = Resources.Other_Zoom_Current + $" {zoomRate:p1}";
			}
		}


		/// <summary>
		/// スクリーンショットを撮影します。
		/// </summary>
		private async Task<Bitmap> TakeScreenShot()
		{
			var kancolleFrame = GetKanColleFrame();
			if (kancolleFrame == null)
			{
				AddLog(3, Translation.KancolleNotLoadedCannotTakeScreenshot);
				System.Media.SystemSounds.Beep.Play();
				return null;
			}


			Task<ScreenShotPacket> InternalTakeScreenShot()
			{
				var request = new ScreenShotPacket();

				if (Browser == null || !Browser.IsBrowserInitialized)
					return request.TaskSource.Task;


				string script = $@"
(async function()
{{
	await CefSharp.BindObjectAsync('{request.ID}');

	let canvas = document.querySelector('canvas');
	requestAnimationFrame(() =>
	{{
		let dataurl = canvas.toDataURL('image/png');
		{request.ID}.complete(dataurl);
	}});
}})();
";

				Browser.JavascriptObjectRepository.Register(request.ID, request);
				kancolleFrame.ExecuteJavaScriptAsync(script);

				return request.TaskSource.Task;
			}

			var result = await InternalTakeScreenShot();

			// ごみ掃除
			Browser.JavascriptObjectRepository.UnRegister(result.ID);
			kancolleFrame.ExecuteJavaScriptAsync($@"delete {result.ID}");

			return result.GetImage();
		}


		/// <summary>
		/// スクリーンショットを撮影し、設定で指定された保存先に保存します。
		/// </summary>
		private async Task SaveScreenShot()
		{
			int savemode = Configuration.ScreenShotSaveMode;
			int format = Configuration.ScreenShotFormat;
			string folderPath = Configuration.ScreenShotPath;
			bool is32bpp = format != 1 && Configuration.AvoidTwitterDeterioration;

			Bitmap image = null;
			try
			{
				image = await TakeScreenShot();


				if (image == null)
					return;

				if (is32bpp)
				{
					if (image.PixelFormat != PixelFormat.Format32bppArgb)
					{
						var imgalt = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
						using (var g = Graphics.FromImage(imgalt))
						{
							g.DrawImage(image, new Rectangle(0, 0, imgalt.Width, imgalt.Height));
						}

						image.Dispose();
						image = imgalt;
					}

					// 不透明ピクセルのみだと jpeg 化されてしまうため、1px だけわずかに透明にする
					Color temp = image.GetPixel(image.Width - 1, image.Height - 1);
					image.SetPixel(image.Width - 1, image.Height - 1, Color.FromArgb(252, temp.R, temp.G, temp.B));
				}
				else
				{
					if (image.PixelFormat != PixelFormat.Format24bppRgb)
					{
						var imgalt = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
						using (var g = Graphics.FromImage(imgalt))
						{
							g.DrawImage(image, new Rectangle(0, 0, imgalt.Width, imgalt.Height));
						}

						image.Dispose();
						image = imgalt;
					}
				}


				// to file
				if ((savemode & 1) != 0)
				{
					try
					{
						if (!Directory.Exists(folderPath))
							Directory.CreateDirectory(folderPath);

						string ext;
						ImageFormat imgFormat;

						switch (format)
						{
							case 1:
								ext = "jpg";
								imgFormat = ImageFormat.Jpeg;
								break;
							case 2:
							default:
								ext = "png";
								imgFormat = ImageFormat.Png;
								break;
						}

						string path = $"{folderPath}\\{DateTime.Now:yyyyMMdd_HHmmssff}.{ext}";
						image.Save(path, imgFormat);
						_lastScreenShotPath = path;

						AddLog(2, string.Format(Translation.ScreenshotSavedTo, path));
					}
					catch (Exception ex)
					{
						SendErrorReport(ex.ToString(), Translation.FailedToSaveScreenshot);
					}
				}


				// to clipboard
				if ((savemode & 2) != 0)
				{
					try
					{
						Clipboard.SetImage(image);

						if ((savemode & 3) != 3)
							AddLog(2, Translation.ScreenshotCopiedToClipboard);
					}
					catch (Exception ex)
					{
						SendErrorReport(ex.ToString(), Translation.FailedToCopyScreenshotToClipboard);
					}
				}
			}
			catch (Exception ex)
			{
				SendErrorReport(ex.ToString(), Resources.ScreenshotError);
			}
			finally
			{
				image?.Dispose();
			}
		}


		public void SetProxy(string proxy)
		{
			ushort port;
			if (ushort.TryParse(proxy, out port))
			{
				// WinInetUtil.SetProxyInProcessForNekoxy(port);
				ProxySettings = "http=127.0.0.1:" + port; // todo: 動くには動くが正しいかわからない
			}
			else
			{
				// WinInetUtil.SetProxyInProcess(proxy, "local");
				ProxySettings = proxy;
			}

			InitializeBrowser();

			BrowserHost.SetProxyCompleted();
		}


		private void SetCookie()
		{
			Browser.ExecuteScriptAsync(Resources.RegionCookie);
		}


		private async void SetIconResource()
		{
			byte[] canvas = await BrowserHost.GetIconResource();

			string[] keys =
			{
				"Browser_ScreenShot",
				"Browser_Zoom",
				"Browser_ZoomIn",
				"Browser_ZoomOut",
				"Browser_Unmute",
				"Browser_Mute",
				"Browser_Refresh",
				"Browser_Navigate",
				"Browser_Other",
			};
			int unitsize = 16 * 16 * 4;

			for (int i = 0; i < keys.Length; i++)
			{
				Bitmap bmp = new Bitmap(16, 16, PixelFormat.Format32bppArgb);

				if (canvas != null)
				{
					BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
						ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
					Marshal.Copy(canvas, unitsize * i, bmpdata.Scan0, unitsize);
					bmp.UnlockBits(bmpdata);
				}

				Icons.Images.Add(keys[i], bmp);
			}

			ToolMenu_ScreenShot.Image = ToolMenu_Other_ScreenShot.Image = Icons.Images["Browser_ScreenShot"];
			ToolMenu_Zoom.Image = ToolMenu_Other_Zoom.Image = Icons.Images["Browser_Zoom"];
			ToolMenu_Other_Zoom_Increment.Image = Icons.Images["Browser_ZoomIn"];
			ToolMenu_Other_Zoom_Decrement.Image = Icons.Images["Browser_ZoomOut"];
			ToolMenu_Refresh.Image = ToolMenu_Other_Refresh.Image = Icons.Images["Browser_Refresh"];
			ToolMenu_NavigateToLogInPage.Image =
				ToolMenu_Other_NavigateToLogInPage.Image = Icons.Images["Browser_Navigate"];
			ToolMenu_Other.Image = Icons.Images["Browser_Other"];

			SetVolumeState();
		}


		private void TryGetVolumeManager()
		{
			_volumeManager = VolumeManager.CreateInstanceByProcessName("CefSharp.BrowserSubprocess");
		}

		private void SetVolumeState()
		{
			bool mute;
			float volume;

			try
			{
				if (_volumeManager == null)
				{
					TryGetVolumeManager();
				}

				mute = _volumeManager.IsMute;
				volume = _volumeManager.Volume * 100;
			}
			catch (Exception)
			{
				// 音量データ取得不能時
				_volumeManager = null;
				mute = false;
				volume = 100;
			}

			ToolMenu_Mute.Image = ToolMenu_Other_Mute.Image =
				Icons.Images[mute ? "Browser_Mute" : "Browser_Unmute"];

			{
				var control = ToolMenu_Other_Volume_VolumeControl;
				control.Tag = false;
				control.Value = (decimal) volume;
				control.Tag = true;
			}

			Configuration.Volume = volume;
			Configuration.IsMute = mute;
			ConfigurationUpdated();
		}


		private async void ToolMenu_Other_ScreenShot_Click(object sender, EventArgs e)
		{
			await SaveScreenShot();
		}

		private void ToolMenu_Other_Zoom_Decrement_Click(object sender, EventArgs e)
		{
			Configuration.ZoomRate = Math.Max(Configuration.ZoomRate - 0.2, 0.1);
			Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked = false;
			ApplyZoom();
			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Zoom_Increment_Click(object sender, EventArgs e)
		{
			Configuration.ZoomRate = Math.Min(Configuration.ZoomRate + 0.2, 10);
			Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked = false;
			ApplyZoom();
			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Zoom_Click(object sender, EventArgs e)
		{
			double zoom;

			if (sender == ToolMenu_Other_Zoom_25)
				zoom = 0.25;
			else if (sender == ToolMenu_Other_Zoom_50)
				zoom = 0.50;
			else if (sender == ToolMenu_Other_Zoom_Classic)
				zoom = 0.667; // 2/3 ジャストだと 799x479 になる
			else if (sender == ToolMenu_Other_Zoom_75)
				zoom = 0.75;
			else if (sender == ToolMenu_Other_Zoom_100)
				zoom = 1;
			else if (sender == ToolMenu_Other_Zoom_150)
				zoom = 1.5;
			else if (sender == ToolMenu_Other_Zoom_200)
				zoom = 2;
			else if (sender == ToolMenu_Other_Zoom_250)
				zoom = 2.5;
			else if (sender == ToolMenu_Other_Zoom_300)
				zoom = 3;
			else if (sender == ToolMenu_Other_Zoom_400)
				zoom = 4;
			else
				zoom = 1;

			Configuration.ZoomRate = zoom;
			Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked = false;
			ApplyZoom();
			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Zoom_Fit_Click(object sender, EventArgs e)
		{
			Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked;
			ApplyZoom();
			ConfigurationUpdated();
		}


		//ズームUIの使いまわし
		private void ToolMenu_Other_DropDownOpening(object sender, EventArgs e)
		{
			var list = ToolMenu_Zoom.DropDownItems.Cast<ToolStripItem>().ToArray();
			ToolMenu_Other_Zoom.DropDownItems.AddRange(list);
		}

		private void ToolMenu_Zoom_DropDownOpening(object sender, EventArgs e)
		{
			var list = ToolMenu_Other_Zoom.DropDownItems.Cast<ToolStripItem>().ToArray();
			ToolMenu_Zoom.DropDownItems.AddRange(list);
		}


		private void ToolMenu_Other_Mute_Click(object sender, EventArgs e)
		{
			if (_volumeManager == null)
			{
				TryGetVolumeManager();
			}

			try
			{
				_volumeManager.ToggleMute();
			}
			catch (Exception)
			{
				System.Media.SystemSounds.Beep.Play();
			}

			SetVolumeState();
		}

		void ToolMenu_Other_Volume_ValueChanged(object sender, EventArgs e)
		{
			var control = ToolMenu_Other_Volume_VolumeControl;

			if (_volumeManager == null)
			{
				TryGetVolumeManager();
			}

			try
			{
				if ((bool) control.Tag)
					_volumeManager.Volume = (float) (control.Value / 100);
				control.BackColor = SystemColors.Window;
			}
			catch (Exception)
			{
				control.BackColor = Color.MistyRose;
			}
		}


		private void ToolMenu_Other_Refresh_Click(object sender, EventArgs e)
		{
			if (!Configuration.ConfirmAtRefresh ||
			    MessageBox.Show(Resources.ReloadDialog, Resources.Confirmation,
				    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
			    == DialogResult.OK)
			{
				RefreshBrowser();
			}
		}

		private void ToolMenu_Other_RefreshIgnoreCache_Click(object sender, EventArgs e)
		{
			if (!Configuration.ConfirmAtRefresh ||
			    MessageBox.Show(Resources.ReloadHardDialog, Resources.Confirmation,
				    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
			    == DialogResult.OK)
			{
				RefreshBrowser(true);
			}
		}

		private void ToolMenu_Other_NavigateToLogInPage_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Resources.LoginDialog, Resources.Confirmation,
				    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			    == DialogResult.OK)
			{
				Navigate(Configuration.LogInPageURL);
			}
		}

		private void ToolMenu_Other_Navigate_Click(object sender, EventArgs e)
		{
			BrowserHost.RequestNavigation(Browser.GetMainFrame()?.Url ?? "");
		}

		private void ToolMenu_Other_AppliesStyleSheet_Click(object sender, EventArgs e)
		{
			Configuration.AppliesStyleSheet = ToolMenu_Other_AppliesStyleSheet.Checked;
			if (!Configuration.AppliesStyleSheet)
				RestoreStyleSheet = true;
			ApplyStyleSheet();
			ApplyZoom();
			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Alignment_Click(object sender, EventArgs e)
		{
			if (sender == ToolMenu_Other_Alignment_Top)
				ToolMenu.Dock = DockStyle.Top;
			else if (sender == ToolMenu_Other_Alignment_Bottom)
				ToolMenu.Dock = DockStyle.Bottom;
			else if (sender == ToolMenu_Other_Alignment_Left)
				ToolMenu.Dock = DockStyle.Left;
			else
				ToolMenu.Dock = DockStyle.Right;

			Configuration.ToolMenuDockStyle = (int) ToolMenu.Dock;

			ConfigurationUpdated();
		}

		private void ToolMenu_Other_Alignment_Invisible_Click(object sender, EventArgs e)
		{
			ToolMenu.Visible =
				Configuration.IsToolMenuVisible = false;
			ConfigurationUpdated();
		}


		private void SizeAdjuster_DoubleClick(object sender, EventArgs e)
		{
			ToolMenu.Visible =
				Configuration.IsToolMenuVisible = true;
			ConfigurationUpdated();
		}

		private void ContextMenuTool_ShowToolMenu_Click(object sender, EventArgs e)
		{
			ToolMenu.Visible =
				Configuration.IsToolMenuVisible = true;
			ConfigurationUpdated();
		}

		private void ContextMenuTool_Opening(object sender, CancelEventArgs e)
		{
			if (IsKanColleLoaded || ToolMenu.Visible)
				e.Cancel = true;
		}


		private void ToolMenu_ScreenShot_Click(object sender, EventArgs e)
		{
			ToolMenu_Other_ScreenShot_Click(sender, e);
		}

		private void ToolMenu_Mute_Click(object sender, EventArgs e)
		{
			ToolMenu_Other_Mute_Click(sender, e);
		}

		private void ToolMenu_Refresh_Click(object sender, EventArgs e)
		{
			ToolMenu_Other_Refresh_Click(sender, e);
		}

		private void ToolMenu_NavigateToLogInPage_Click(object sender, EventArgs e)
		{
			ToolMenu_Other_NavigateToLogInPage_Click(sender, e);
		}


		private void FormBrowser_Activated(object sender, EventArgs e)
		{
			Browser.Focus();
		}

		private void ToolMenu_Other_Alignment_DropDownOpening(object sender, EventArgs e)
		{
			foreach (var item in ToolMenu_Other_Alignment.DropDownItems)
			{
				if (item is ToolStripMenuItem menu)
				{
					menu.Checked = false;
				}
			}

			switch ((DockStyle) Configuration.ToolMenuDockStyle)
			{
				case DockStyle.Top:
					ToolMenu_Other_Alignment_Top.Checked = true;
					break;
				case DockStyle.Bottom:
					ToolMenu_Other_Alignment_Bottom.Checked = true;
					break;
				case DockStyle.Left:
					ToolMenu_Other_Alignment_Left.Checked = true;
					break;
				case DockStyle.Right:
					ToolMenu_Other_Alignment_Right.Checked = true;
					break;
			}

			ToolMenu_Other_Alignment_Invisible.Checked = !Configuration.IsToolMenuVisible;
		}


		private void ToolMenu_Other_LastScreenShot_DropDownOpening(object sender, EventArgs e)
		{
			try
			{
				using (var fs = new FileStream(_lastScreenShotPath, FileMode.Open, FileAccess.Read))
				{
					ToolMenu_Other_LastScreenShot_Control.Image?.Dispose();

					ToolMenu_Other_LastScreenShot_Control.Image = Image.FromStream(fs);
				}
			}
			catch (Exception)
			{
				// *ぷちっ*
			}
		}

		void ToolMenu_Other_LastScreenShot_ImageHost_Click(object sender, EventArgs e)
		{
			if (_lastScreenShotPath != null && File.Exists(_lastScreenShotPath))
			{
				ProcessStartInfo psi = new ProcessStartInfo
				{
					FileName = _lastScreenShotPath,
					UseShellExecute = true
				};
				Process.Start(psi);
			}
		}

		private void ToolMenu_Other_LastScreenShot_OpenScreenShotFolder_Click(object sender, EventArgs e)
		{
			if (Directory.Exists(Configuration.ScreenShotPath))
			{
				ProcessStartInfo psi = new ProcessStartInfo
				{
					FileName = Configuration.ScreenShotPath,
					UseShellExecute = true
				};
				Process.Start(psi);
			}
		}

		private void ToolMenu_Other_LastScreenShot_CopyToClipboard_Click(object sender, EventArgs e)
		{
			if (_lastScreenShotPath != null && File.Exists(_lastScreenShotPath))
			{
				try
				{
					using (var img = new Bitmap(_lastScreenShotPath))
					{
						Clipboard.SetImage(img);
						AddLog(2, string.Format(Translation.LastScreenshotCopiedToClipboard, _lastScreenShotPath));
					}
				}
				catch (Exception ex)
				{
					SendErrorReport(ex.Message, Translation.FailedToCopyScreenshotToClipboard);
				}
			}
		}

		private void ToolMenu_Other_OpenDevTool_Click(object sender, EventArgs e)
		{
			if (!IsBrowserInitialized)
				return;

			Browser.GetBrowser().ShowDevTools();
		}

		private void ToolMenu_Other_ClearCache_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Resources.ClearCacheMessage, Resources.ClearCacheTitle,
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				BrowserHost.ClearCache();
				// BrowserHost.AsyncRemoteRun(() => BrowserHost.Proxy.ClearCache());
			}
		}

		public void OpenExtraBrowser()
		{
			new DialogExtraBrowser().Show(this);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_ERASEBKGND)
				// ignore this message
				return;

			base.WndProc(ref m);
		}


		#region 呪文

		[DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
		private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
		private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

		private const int GWL_STYLE = (-16);
		private const uint WS_CHILD = 0x40000000;
		private const uint WS_VISIBLE = 0x10000000;
		private const int WM_ERASEBKGND = 0x14;

		#endregion
	}


	/// <summary>
	/// ウィンドウが非アクティブ状態から1回のクリックでボタンが押せる ToolStrip です。
	/// </summary>
	internal class ExtraToolStrip : ToolStrip
	{
		public ExtraToolStrip() : base()
		{
		}

		private const uint WM_MOUSEACTIVATE = 0x21;
		private const uint MA_ACTIVATE = 1;
		private const uint MA_ACTIVATEANDEAT = 2;
		private const uint MA_NOACTIVATE = 3;
		private const uint MA_NOACTIVATEANDEAT = 4;

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if (m.Msg == WM_MOUSEACTIVATE && m.Result == (IntPtr) MA_ACTIVATEANDEAT)
				m.Result = (IntPtr) MA_ACTIVATE;
		}
	}
}