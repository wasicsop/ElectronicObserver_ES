using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Browser.CefOp;
using Browser.ExtraBrowser;
using BrowserLibCore;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.Wpf.Internals;
using Grpc.Core;
using MagicOnion.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ModernWpf;

namespace Browser;

public class ImageProvider
{
	public ImageSource? Screenshot { get; }
	public ImageSource? Zoom { get; }
	public ImageSource? ZoomIn { get; }
	public ImageSource? ZoomOut { get; }
	public ImageSource? Unmute { get; }
	public ImageSource? Mute { get; }
	public ImageSource? Refresh { get; }
	public ImageSource? Navigate { get; }
	public ImageSource? Other { get; }

	public ImageProvider(byte[][] images)
	{
		Screenshot = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[0]);
		Zoom = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[1]);
		ZoomIn = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[2]);
		ZoomOut = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[3]);
		Unmute = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[4]);
		Mute = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[5]);
		Refresh = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[6]);
		Navigate = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[7]);
		Other = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[8]);
	}
}

public class BrowserViewModel : ObservableObject, BrowserLibCore.IBrowser
{
	public FormBrowserTranslationViewModel FormBrowser { get; }
	private BrowserConfiguration Configuration { get; set; }

	private string Host { get; }
	private int Port { get; }
	private string Culture { get; }
	private BrowserLibCore.IBrowserHost BrowserHost { get; set; }
	private string? ProxySettings { get; set; }

	public ImageProvider? Icons { get; set; }

	private Size KanColleSize { get; } = new(1200, 720);
	private string KanColleUrl => "http://www.dmm.com/netgame/social/-/gadgets/=/app_id=854854/";
	private string BrowserCachePath => BrowserConstants.CachePath;

	private string StyleClassID { get; } = Guid.NewGuid().ToString().Substring(0, 8);
	public bool StyleSheetApplied { get; set; }
	private bool RestoreStyleSheet { get; set; }

	public Dock ToolMenuDock { get; set; } = Dock.Top;
	public Orientation ToolMenuOrientation => ToolMenuDock switch
	{
		Dock.Left or Dock.Right => Orientation.Vertical,
		_ => Orientation.Horizontal
	};
	public Visibility ToolMenuVisibility { get; set; } = Visibility.Visible;

	public WindowsFormsHost BrowserWrapper { get; } = new();
	public ChromiumWebBrowser? Browser { get; set; }

	public DpiScale DpiScale { get; set; }
	public double ActualWidth { get; set; }
	public double ActualHeight { get; set; }

	private System.Timers.Timer HeartbeatTimer { get; } = new();

	private bool IsKanColleLoaded { get; set; }

	public string? LastScreenShotPath { get; set; }

	private VolumeManager? VolumeManager { get; set; }
	public int Volume { get; set; }
	public ImageSource? MuteStateImage { get; set; }

	public bool ZoomFit { get; set; }
	public string CurrentZoom { get; set; } = "";

	public ICommand ScreenshotCommand { get; }
	public ICommand SetZoomCommand { get; }
	public ICommand ModifyZoomCommand { get; }
	public ICommand MuteCommand { get; }
	public ICommand RefreshCommand { get; }
	public ICommand GoToLoginPageCommand { get; }

	public ICommand OpenLastScreenshotCommand { get; }
	public ICommand OpenScreenshotFolderCommand { get; }
	public ICommand CopyLastScreenshotCommand { get; }

	public ICommand HardRefreshCommand { get; }
	public ICommand GoToCommand { get; }
	public ICommand ClearCacheCommand { get; }

	public ICommand SetToolMenuAlignmentCommand { get; }
	public ICommand SetToolMenuVisibilityCommand { get; }

	public ICommand OpenDevtoolsCommand { get; }

	/// <summary>
	/// </summary>
	/// <param name="serverUri">ホストプロセスとの通信用URL</param>
	public BrowserViewModel(string host, int port, string culture)
	{
		// Debugger.Launch();

		FormBrowser = App.Current.Services.GetService<FormBrowserTranslationViewModel>()!;

		ScreenshotCommand = new RelayCommand(ToolMenu_Other_ScreenShot_Click);
		SetZoomCommand = new RelayCommand<string>(SetZoom);
		ModifyZoomCommand = new RelayCommand<string>(ModifyZoom);
		MuteCommand = new RelayCommand(ToolMenu_Other_Mute_Click);
		RefreshCommand = new RelayCommand(ToolMenu_Other_Refresh_Click);
		GoToLoginPageCommand = new RelayCommand(ToolMenu_Other_NavigateToLogInPage_Click);

		OpenLastScreenshotCommand = new RelayCommand(ToolMenu_Other_LastScreenShot_ImageHost_Click);
		OpenScreenshotFolderCommand = new RelayCommand(ToolMenu_Other_LastScreenShot_OpenScreenShotFolder_Click);
		CopyLastScreenshotCommand = new RelayCommand(ToolMenu_Other_LastScreenShot_CopyToClipboard_Click);

		HardRefreshCommand = new RelayCommand(ToolMenu_Other_RefreshIgnoreCache_Click);
		GoToCommand = new RelayCommand(ToolMenu_Other_Navigate_Click);
		ClearCacheCommand = new RelayCommand(ToolMenu_Other_ClearCache_Click);

		SetToolMenuAlignmentCommand = new RelayCommand<Dock>(SetToolMenuAlignment);
		SetToolMenuVisibilityCommand = new RelayCommand<Visibility>(SetToolMenuVisibility);

		OpenDevtoolsCommand = new RelayCommand(ToolMenu_Other_OpenDevTool_Click);

		Host = host;
		Port = port;
		Culture = culture;

		CultureInfo c = new(culture);

		Thread.CurrentThread.CurrentCulture = c;
		Thread.CurrentThread.CurrentUICulture = c;

		StyleSheetApplied = false;

		PropertyChanged += (sender, args) =>
		{
			if (Configuration is null) return;
			if (args.PropertyName is not nameof(ZoomFit)) return;

			Configuration.ZoomFit = ZoomFit;
			ApplyZoom();
			ConfigurationUpdated();
		};

		PropertyChanged += (sender, args) =>
		{
			if (Configuration is null) return;
			if (args.PropertyName is not nameof(StyleSheetApplied)) return;

			Configuration.AppliesStyleSheet = StyleSheetApplied;
			if (!Configuration.AppliesStyleSheet)
			{
				RestoreStyleSheet = true;
			}

			ApplyStyleSheet();
			ApplyZoom();
			ConfigurationUpdated();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(ActualWidth) or nameof(ActualHeight))) return;

			ApplyZoom();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Volume)) return;

			ToolMenu_Other_Volume_ValueChanged();
		};
	}

	public void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (sender is not Window window) return;

		IntPtr handle = new WindowInteropHelper(window).Handle;
		SetWindowLong(handle, GWL_STYLE, WS_CHILD);

		// ホストプロセスに接続
		Channel grpChannel = new(Host, Port, ChannelCredentials.Insecure);
		BrowserHost = StreamingHubClient.Connect<BrowserLibCore.IBrowserHost, BrowserLibCore.IBrowser>(grpChannel, this);

		ConfigurationChanged();

		// ウィンドウの親子設定＆ホストプロセスから接続してもらう
		Task.Run(async () => await BrowserHost.ConnectToBrowser((long)handle)).Wait();

		// 親ウィンドウが生きているか確認
		HeartbeatTimer.Elapsed += async (sender2, e2) =>
		{
			try
			{
				bool alive = await BrowserHost.IsServerAlive();
			}
			catch (Exception)
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
	private void InitializeBrowser()
	{
		if (Browser != null) return;
		if (ProxySettings == null) return;

		Cef.EnableHighDPISupport();

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
		catch (BadImageFormatException)
		{
			if (MessageBox.Show(FormBrowser.InstallVisualCpp, FormBrowser.CefSharpLoadErrorTitle,
					MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes)
				== MessageBoxResult.Yes)
			{
				ProcessStartInfo psi = new()
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
		if (settings.CefCommandLineArgs.ContainsKey("disable-features"))
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
		{
			settings.CefCommandLineArgs.Add("force-color-profile", "srgb");
		}

		CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
		Cef.Initialize(settings, false, (IBrowserProcessHandler?)null);

		var requestHandler = new CustomRequestHandler(Configuration.PreserveDrawingBuffer, Configuration.UseGadgetRedirect);
		requestHandler.RenderProcessTerminated += (mes) => AddLog(3, mes);

		Browser = new ChromiumWebBrowser(KanColleUrl)
		{
			RequestHandler = requestHandler,
			KeyboardHandler = new KeyboardHandler(),
			MenuHandler = new MenuHandler(),
			DragHandler = new DragHandler(),
		};

		// Browser.WpfKeyboardHandler = new WpfKeyboardHandler(Browser);

		Browser.BrowserSettings.StandardFontFamily = "Microsoft YaHei"; // Fixes text rendering position too high
		Browser.LoadingStateChanged += Browser_LoadingStateChanged;
		Browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;

		BrowserWrapper.Child = Browser;
		Browser.PreviewKeyDown += (sender, args) =>
		{
			CultureInfo c = new(Culture);

			Thread.CurrentThread.CurrentCulture = c;
			Thread.CurrentThread.CurrentUICulture = c;

			switch (args.KeyCode)
			{
				case System.Windows.Forms.Keys.F2:
					ScreenshotCommand.Execute(null);
					break;
				case System.Windows.Forms.Keys.F5:
					// hard refresh if ctrl is pressed
					if ((args.Modifiers & System.Windows.Forms.Keys.Control) == System.Windows.Forms.Keys.Control)
					{
						HardRefreshCommand.Execute(null);
					}
					else
					{
						RefreshCommand.Execute(null);
					}
					break;
				case System.Windows.Forms.Keys.F7:
					MuteCommand.Execute(null);
					break;
				case System.Windows.Forms.Keys.F12:
					OpenDevtoolsCommand.Execute(null);
					break;
			}
		};
	}

	private void Exit()
	{
		// if (!BrowserHost.Closed)
		{
			// BrowserHost.Close();
			App.Current.Dispatcher.Invoke(() =>
			{
				HeartbeatTimer.Stop();
				Task.Run(async () => await BrowserHost.DisposeAsync()).Wait();
				Cef.Shutdown();
				App.Current.Shutdown();
			});
		}
	}

	public void CloseBrowser()
	{
		HeartbeatTimer.Stop();
		// リモートコールでClose()呼ぶのばヤバそうなので非同期にしておく
		App.Current.Dispatcher.BeginInvoke((Action)(() => Exit()));
	}

	public async void ConfigurationChanged()
	{
		Configuration = await BrowserHost.Configuration();

		ZoomFit = Configuration.ZoomFit;
		ApplyZoom();
		StyleSheetApplied = Configuration.AppliesStyleSheet;

		ToolMenuDock = Configuration.ToolMenuDockStyle switch
		{
			1 => Dock.Top,
			2 => Dock.Bottom,
			3 => Dock.Left,
			4 => Dock.Right,
			_ => Dock.Top
		};

		ToolMenuVisibility = Configuration.IsToolMenuVisible switch
		{
			true => Visibility.Visible,
			_ => Visibility.Collapsed
		};

		ThemeManager.Current.ApplicationTheme = (ApplicationTheme)await BrowserHost.GetTheme();

		// SizeAdjuster.BackColor = System.Drawing.Color.FromArgb(unchecked((int)Configuration.BackColor));
		// ToolMenu.BackColor = System.Drawing.Color.FromArgb(unchecked((int)Configuration.BackColor));
		// ToolMenu_Other_ClearCache.Visible = conf.EnableDebugMenu;
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

	// hack: it makes an infinite loop in the wpf version for some reason
	private int Counter { get; set; }

	private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
	{
		// DocumentCompleted に相当?
		// note: 非 UI thread からコールされるので、何かしら UI に触る場合は適切な処置が必要

		if (e.IsLoading) return;

		App.Current.Dispatcher.Invoke(() =>
		{
			if (Counter > 0) return;
			if (!Browser!.Address.Contains("redirect")) return;

			Counter++;

			SetCookie();
			Browser.Reload();
		});

		/*if (Browser.Address.Contains("login/=/path="))
	    {
	        SetCookie();
            Browser.ExecuteScriptAsync(Properties.Resources.RemoveWelcomePopup);
	        Browser.ExecuteScriptAsync(Properties.Resources.RemoveServicePopup);
        }*/

		App.Current.Dispatcher.BeginInvoke((Action)(() =>
		{
			ApplyStyleSheet();

			ApplyZoom();
			DestroyDMMreloadDialog();
		}));
	}

	private IFrame? GetMainFrame()
	{
		if (Browser is not { IsBrowserInitialized: true }) return null;

		var browser = Browser.GetBrowser();
		var frame = browser.MainFrame;

		if (frame?.Url?.Contains(@"http://www.dmm.com/netgame/social/") ?? false)
			return frame;

		return null;
	}

	private IFrame? GetGameFrame()
	{
		if (Browser is not { IsBrowserInitialized: true }) return null;

		var browser = Browser.GetBrowser();
		var frames = browser.GetFrameIdentifiers()
			.Select(id => browser.GetFrame(id));

		return frames.FirstOrDefault(f => f?.Url?.Contains(@"http://osapi.dmm.com/gadgets/") ?? false);
	}

	private IFrame? GetKanColleFrame()
	{
		if (Browser is not { IsBrowserInitialized: true }) return null;

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
		if (Browser is not { IsBrowserInitialized: true }) return;
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
			SendErrorReport(ex.ToString(), FormBrowser.FailedToApplyStylesheet);
		}
	}

	/// <summary>
	/// DMMによるページ更新ダイアログを非表示にします。
	/// </summary>
	private void DestroyDMMreloadDialog()
	{
		if (Browser is not { IsBrowserInitialized: true }) return;
		if (!Configuration.IsDMMreloadDialogDestroyable) return;

		try
		{
			GetMainFrame()?.EvaluateScriptAsync(Properties.Resources.DMMScript);
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.FailedToHideDmmRefreshDialog);
		}
	}


	// タイミングによっては(特に起動時)、ブラウザの初期化が完了する前に Navigate() が呼ばれることがある
	// その場合ロードに失敗してブラウザが白画面でスタートしてしまう（手動でログインページを開けば続行は可能だが）
	// 応急処置として失敗したとき後で再試行するようにしてみる
	private string? NavigateCache { get; set; }

	private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
	{
		if (Browser is not { IsBrowserInitialized: true } && NavigateCache is not null)
		{
			// ロードが完了したので再試行
			string url = NavigateCache; // 非同期コールするのでコピーを取っておく必要がある
			App.Current.Dispatcher.BeginInvoke((Action)(() => Navigate(url)));
			NavigateCache = null;
		}
	}

	/// <summary>
	/// 指定した URL のページを開きます。
	/// </summary>
	public void Navigate(string url)
	{
		if (url != Configuration.LogInPageURL || !Configuration.AppliesStyleSheet)
		{
			StyleSheetApplied = false;
		}

		if (Browser is not { IsBrowserInitialized: true }) return;

		Browser.Load(url);
		// 大方ロードできないのであとで再試行する
		NavigateCache = url;
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
		{
			StyleSheetApplied = false;
		}

		Browser.Reload(ignoreCache);
	}

	/// <summary>
	/// ズームを適用します。
	/// </summary>
	private void ApplyZoom()
	{
		if (Browser is not { IsBrowserInitialized: true }) return;

		double zoomRate = Configuration.ZoomRate;
		bool fit = Configuration.ZoomFit && StyleSheetApplied;

		double zoomFactor;

		if (fit)
		{
			double rateX = ActualWidth * DpiScale.DpiScaleX / KanColleSize.Width;
			double rateY = ActualHeight * DpiScale.DpiScaleY / KanColleSize.Height;

			zoomFactor = Math.Min(rateX, rateY);
		}
		else
		{
			zoomFactor = Math.Clamp(zoomRate, 0.1, 10);
		}

		// DpiScaleX and DpiScaleY should always be the same so it doesn't matter which one you use
		Browser.SetZoomLevel(Math.Log(zoomFactor / DpiScale.DpiScaleX, 1.2));

		if (StyleSheetApplied)
		{
			int newWidth = (int)(KanColleSize.Width * zoomFactor);
			int newHeight = (int)(KanColleSize.Height * zoomFactor);

			Browser.Width = newWidth;
			Browser.Height = newHeight;

			// Browser.MinWidth = newWidth;
			// Browser.MinHeight = newHeight;
		}

		CurrentZoom = fit switch
		{
			true => FormBrowser.Other_Zoom_Current_Fit,
			_ => FormBrowser.Other_Zoom_Current + $" {zoomRate:p1}"
		};
	}

	/// <summary>
	/// スクリーンショットを撮影します。
	/// </summary>
	private async Task<System.Drawing.Bitmap?> TakeScreenShot()
	{
		var kancolleFrame = GetKanColleFrame();
		if (kancolleFrame == null)
		{
			AddLog(3, FormBrowser.KancolleNotLoadedCannotTakeScreenshot);
			System.Media.SystemSounds.Beep.Play();
			return null;
		}


		Task<ScreenShotPacket> InternalTakeScreenShot()
		{
			var request = new ScreenShotPacket();

			if (Browser is not { IsBrowserInitialized: true }) return request.TaskSource.Task;


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
		Browser!.JavascriptObjectRepository.UnRegister(result.ID);
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

		System.Drawing.Bitmap? image = null;
		try
		{
			image = await TakeScreenShot();


			if (image == null) return;

			if (is32bpp)
			{
				if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
				{
					var imgalt = new System.Drawing.Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
					using (var g = System.Drawing.Graphics.FromImage(imgalt))
					{
						g.DrawImage(image, new System.Drawing.Rectangle(0, 0, imgalt.Width, imgalt.Height));
					}

					image.Dispose();
					image = imgalt;
				}

				// 不透明ピクセルのみだと jpeg 化されてしまうため、1px だけわずかに透明にする
				System.Drawing.Color temp = image.GetPixel(image.Width - 1, image.Height - 1);
				image.SetPixel(image.Width - 1, image.Height - 1, System.Drawing.Color.FromArgb(252, temp.R, temp.G, temp.B));
			}
			else
			{
				if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb)
				{
					var imgalt = new System.Drawing.Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
					using (var g = System.Drawing.Graphics.FromImage(imgalt))
					{
						g.DrawImage(image, new System.Drawing.Rectangle(0, 0, imgalt.Width, imgalt.Height));
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
					System.Drawing.Imaging.ImageFormat imgFormat;

					switch (format)
					{
						case 1:
							ext = "jpg";
							imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
							break;
						case 2:
						default:
							ext = "png";
							imgFormat = System.Drawing.Imaging.ImageFormat.Png;
							break;
					}

					string path = $"{folderPath}\\{DateTime.Now:yyyyMMdd_HHmmssff}.{ext}";
					image.Save(path, imgFormat);
					LastScreenShotPath = Path.GetFullPath(path);

					AddLog(2, string.Format(FormBrowser.ScreenshotSavedTo, path));
				}
				catch (Exception ex)
				{
					SendErrorReport(ex.ToString(), FormBrowser.FailedToSaveScreenshot);
				}
			}


			// to clipboard
			if ((savemode & 2) != 0)
			{
				try
				{
					Clipboard.SetImage(image.ToBitmapSource());

					if ((savemode & 3) != 3)
						AddLog(2, FormBrowser.ScreenshotCopiedToClipboard);
				}
				catch (Exception ex)
				{
					SendErrorReport(ex.ToString(), FormBrowser.FailedToCopyScreenshotToClipboard);
				}
			}
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.ScreenshotError);
		}
		finally
		{
			image?.Dispose();
		}
	}

	public void SetProxy(string proxy)
	{
		if (ushort.TryParse(proxy, out ushort port))
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
		Browser.ExecuteScriptAsync(FormBrowser.RegionCookie);
	}

	private async void SetIconResource()
	{
		byte[][] canvas = await BrowserHost.GetIconResource();

		Icons = new(canvas);

		SetVolumeState();
	}

	private void TryGetVolumeManager()
	{
		VolumeManager = VolumeManager.CreateInstanceByProcessName("CefSharp.BrowserSubprocess");
	}

	private void SetVolumeState()
	{
		bool mute;
		float volume;

		try
		{
			if (VolumeManager == null)
			{
				TryGetVolumeManager();
			}

			mute = VolumeManager?.IsMute ?? false;
			volume = (VolumeManager?.Volume ?? 1) * 100;
		}
		catch (Exception)
		{
			// 音量データ取得不能時
			VolumeManager = null;
			mute = false;
			volume = 100;
		}

		MuteStateImage = mute switch
		{
			true => Icons?.Mute,
			_ => Icons?.Unmute,
		};

		Volume = (int)volume;
		Configuration.Volume = volume;
		Configuration.IsMute = mute;
		ConfigurationUpdated();
	}

	private async void ToolMenu_Other_ScreenShot_Click()
	{
		await SaveScreenShot();
	}

	private void SetZoom(string? zoomParameter)
	{
		if (!double.TryParse(zoomParameter, out double zoom)) return;

		Configuration.ZoomRate = zoom;
		Configuration.ZoomFit = ZoomFit = false;
		ApplyZoom();
		ConfigurationUpdated();
	}

	private void ModifyZoom(string? zoomParameter)
	{
		if (!double.TryParse(zoomParameter, out double zoom)) return;

		Configuration.ZoomRate = Math.Clamp(Configuration.ZoomRate + zoom, 0.1, 10);
		Configuration.ZoomFit = ZoomFit = false;
		ApplyZoom();
		ConfigurationUpdated();
	}

	private void SetToolMenuAlignment(Dock dock)
	{
		ToolMenuDock = dock;

		Configuration.ToolMenuDockStyle = dock switch
		{
			Dock.Top => 1,
			Dock.Bottom => 2,
			Dock.Left => 3,
			Dock.Right => 4,
			_ => 1
		};

		ConfigurationUpdated();
	}

	private void SetToolMenuVisibility(Visibility visibility)
	{
		ToolMenuVisibility = visibility;

		Configuration.IsToolMenuVisible = visibility == Visibility.Visible;
		ConfigurationUpdated();
	}

	private void ToolMenu_Other_Mute_Click()
	{
		if (VolumeManager == null)
		{
			TryGetVolumeManager();
		}

		try
		{
			if (VolumeManager is null)
			{
				System.Media.SystemSounds.Beep.Play();
			}
			else
			{
				VolumeManager.ToggleMute();
			}
		}
		catch (Exception)
		{

		}

		SetVolumeState();
	}

	private void ToolMenu_Other_Volume_ValueChanged()
	{
		if (VolumeManager is null)
		{
			TryGetVolumeManager();
		}

		try
		{
			if (VolumeManager is not null)
			{
				VolumeManager.Volume = (float)Volume / 100;
				// control.BackColor = System.Drawing.SystemColors.Window;
			}
			else
			{
				// todo: add red color to indicate volume manager isn't active?
				// control.BackColor = System.Drawing.Color.MistyRose;
			}

		}
		catch (Exception)
		{
		}
	}

	private void ToolMenu_Other_Refresh_Click()
	{
		if (!Configuration.ConfirmAtRefresh ||
			MessageBox.Show(FormBrowser.ReloadDialog, FormBrowser.Confirmation,
				MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel)
			== MessageBoxResult.OK)
		{
			RefreshBrowser();
		}
	}

	private void ToolMenu_Other_RefreshIgnoreCache_Click()
	{
		if (!Configuration.ConfirmAtRefresh ||
			MessageBox.Show(FormBrowser.ReloadHardDialog, FormBrowser.Confirmation,
				MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel)
			== MessageBoxResult.OK)
		{
			RefreshBrowser(true);
		}
	}

	private void ToolMenu_Other_NavigateToLogInPage_Click()
	{
		if (MessageBox.Show(FormBrowser.LoginDialog, FormBrowser.Confirmation,
				MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel)
			== MessageBoxResult.OK)
		{
			Navigate(Configuration.LogInPageURL);
		}
	}

	private void ToolMenu_Other_Navigate_Click()
	{
		BrowserHost.RequestNavigation(Browser.GetMainFrame()?.Url ?? "");
	}

	private void ToolMenu_Other_AppliesStyleSheet_Click()
	{
		/*
		Configuration.AppliesStyleSheet = ToolMenu.Other_AppliesStyleSheet.Checked;
		if (!Configuration.AppliesStyleSheet)
			RestoreStyleSheet = true;
		ApplyStyleSheet();
		ApplyZoom();
		ConfigurationUpdated();
		*/
	}



	private void ContextMenuTool_ShowToolMenu_Click(object sender, EventArgs e)
	{
		/*
		ToolMenu.Visible = Configuration.IsToolMenuVisible = true;
		ConfigurationUpdated();
		*/
	}

	private void FormBrowser_Activated(object sender, EventArgs e)
	{
		Browser?.Focus();
	}

	void ToolMenu_Other_LastScreenShot_ImageHost_Click()
	{
		if (LastScreenShotPath is null || !File.Exists(LastScreenShotPath)) return;

		ProcessStartInfo psi = new()
		{
			FileName = LastScreenShotPath,
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	private void ToolMenu_Other_LastScreenShot_OpenScreenShotFolder_Click()
	{
		if (!Directory.Exists(Configuration.ScreenShotPath)) return;

		ProcessStartInfo psi = new()
		{
			FileName = Configuration.ScreenShotPath,
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	private void ToolMenu_Other_LastScreenShot_CopyToClipboard_Click()
	{
		if (LastScreenShotPath is null || !File.Exists(LastScreenShotPath)) return;

		try
		{
			using var img = new System.Drawing.Bitmap(LastScreenShotPath);
			Clipboard.SetImage(img.ToBitmapSource());
			AddLog(2, string.Format(FormBrowser.LastScreenshotCopiedToClipboard, LastScreenShotPath));
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.Message, FormBrowser.FailedToCopyScreenshotToClipboard);
		}
	}

	private void ToolMenu_Other_OpenDevTool_Click()
	{
		if (Browser is not { IsBrowserInitialized: true }) return;

		Browser.GetBrowser().ShowDevTools();
	}

	private void ToolMenu_Other_ClearCache_Click()
	{
		if (MessageBox.Show(FormBrowser.ClearCacheMessage, FormBrowser.ClearCacheTitle,
			MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			BrowserHost.ClearCache();
		}
	}

	public void OpenExtraBrowser()
	{
		new ExtraBrowserWindow().Show();
	}

	/*
	protected override void WndProc(ref Message m)
	{
		if (m.Msg == WM_ERASEBKGND)
			// ignore this message
			return;

		base.WndProc(ref m);
	}

*/

	#region 呪文

	[DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
	public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

	[DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
	public static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

	public const int GWL_STYLE = (-16);
	public const uint WS_CHILD = 0x40000000;
	public const uint WS_VISIBLE = 0x10000000;
	public const int WM_ERASEBKGND = 0x14;

	#endregion
}
