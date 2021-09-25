using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using AvalonDock.Themes;
using DynaJson;
using ElectronicObserver.Data;
using ElectronicObserver.Notifier;
using ElectronicObserver.Observer;
using ElectronicObserver.Properties;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Wpf;
using ElectronicObserver.Window.Wpf.Arsenal;
using ElectronicObserver.Window.Wpf.BaseAirCorps;
using ElectronicObserver.Window.Wpf.Battle;
using ElectronicObserver.Window.Wpf.Compass;
using ElectronicObserver.Window.Wpf.Dock;
using ElectronicObserver.Window.Wpf.Fleet;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserver.Window.Wpf.FleetOverview;
using ElectronicObserver.Window.Wpf.FleetPreset;
using ElectronicObserver.Window.Wpf.Headquarters;
using ElectronicObserver.Window.Wpf.ShipGroup.ViewModels;
using ElectronicObserver.Window.Wpf.WinformsWrappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ModernWpf;
using Control = System.Windows.Controls.Control;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Windows.Forms.Timer;

namespace ElectronicObserver.ViewModels
{
	public class FormMainViewModel : ObservableObject
	{
		private Control View { get; }
		private DockingManager DockingManager { get; }
		private Configuration.ConfigurationData Config { get; }
		public FormMainTranslationViewModel FormMain { get; }
		private System.Windows.Forms.Timer UIUpdateTimer { get; }

		private string DefaultLayoutPath => @"Settings\Layout\Default.xml";

		// todo: add multi layout support after full wpf release
		private string LayoutPath => DefaultLayoutPath; // Config.Life.LayoutFilePath;
		private string PositionPath => Path.ChangeExtension(LayoutPath, ".Position.json");
		public bool NotificationsSilenced { get; set; }
		private DateTime PrevPlayTimeRecorded { get; set; } = DateTime.MinValue;
		public FontFamily Font { get; set; }
		public double FontSize { get; set; }
		public SolidColorBrush FontBrush { get; set; }
		public FontFamily SubFont { get; set; }
		public double SubFontSize { get; set; }
		public SolidColorBrush SubFontBrush { get; set; }

		public List<Theme> Themes { get; } = new()
		{
			new Vs2013LightTheme(),
			new Vs2013BlueTheme(),
			new Vs2013DarkTheme(),
		};
		public Theme CurrentTheme { get; set; }
		public Color BackgroundColor { get; set; }

		private WindowPosition Position { get; set; } = new();

		#region Icons

		public ImageSource? ConfigurationImageSource { get; }

		public ImageSource? FleetsImageSource { get; }
		public ImageSource? FleetOverviewImageSource { get; }
		public ImageSource? ShipGroupImageSource { get; }
		public ImageSource? FleetPresetImageSource { get; }
		public ImageSource? DockImageSource { get; }
		public ImageSource? ArsenalImageSource { get; }
		public ImageSource? BaseAirCorpsImageSource { get; }
		public ImageSource? HeadquartersImageSource { get; }
		public ImageSource? QuestImageSource { get; }
		public ImageSource? InformationImageSource { get; }
		public ImageSource? CompassImageSource { get; }
		public ImageSource? BattleImageSource { get; }
		public ImageSource? BrowserHostImageSource { get; }
		public ImageSource? LogImageSource { get; }
		public ImageSource? JsonImageSource { get; }

		public ImageSource? EquipmentListImageSource { get; }
		public ImageSource? DropRecordImageSource { get; }
		public ImageSource? DevelopmentRecordImageSource { get; }
		public ImageSource? ConstructionRecordImageSource { get; }
		public ImageSource? ResourceChartImageSource { get; }
		public ImageSource? AlbumMasterShipImageSource { get; }
		public ImageSource? AlbumMasterEquipmentImageSource { get; }
		public ImageSource? AntiAirDefenseImageSource { get; }
		public ImageSource? FleetImageGeneratorImageSource { get; }
		public ImageSource? BaseAirCorpsSimulationImageSource { get; }
		public ImageSource? ExpCheckerImageSource { get; }
		public ImageSource? ExpeditionCheckImageSource { get; }
		public ImageSource? KancolleProgressImageSource { get; }
		public ImageSource? ExtraBrowserImageSource { get; }

		public ImageSource? ViewHelpImageSource { get; }
		public ImageSource? ViewVersionImageSource { get; }

		#endregion

		public ObservableCollection<AnchorableViewModel> Views { get; } = new();

		public List<FleetViewModel> Fleets { get; }
		public FleetOverviewViewModel FleetOverview { get; }
		public FormShipGroupViewModel FormShipGroup { get; }
		// public ShipGroupViewModel ShipGroup { get; }
		public FleetPresetViewModel FleetPreset { get; }

		public DockViewModel Dock { get; }
		public ArsenalViewModel Arsenal { get; }
		public BaseAirCorpsViewModel BaseAirCorps { get; }

		public HeadquartersViewModel Headquarters { get; }
		public FormQuestViewModel FormQuest { get; }
		public FormInformationViewModel FormInformation { get; }

		public CompassViewModel Compass { get; }
		public BattleViewModel Battle { get; }

		public FormBrowserHostViewModel FormBrowserHost { get; }
		public FormLogViewModel FormLog { get; }
		public FormJsonViewModel FormJson { get; }

		public StripStatusViewModel StripStatus { get; } = new();
		public int ClockFormat { get; set; }

		private bool DebugEnabled { get; set; }
		public Visibility DebugVisibility => DebugEnabled.ToVisibility();

		#region Commands

		public ICommand SaveDataCommand { get; }
		public ICommand LoadDataCommand { get; }
		public ICommand SilenceNotificationsCommand { get; }
		public ICommand OpenConfigurationCommand { get; }

		public ICommand OpenEquipmentListCommand { get; }
		public ICommand OpenDropRecordCommand { get; }
		public ICommand OpenDevelopmentRecordCommand { get; }
		public ICommand OpenConstructionRecordCommand { get; }
		public ICommand OpenResourceChartCommand { get; }
		public ICommand OpenAlbumMasterShipCommand { get; }
		public ICommand OpenAlbumMasterEquipmentCommand { get; }
		public ICommand OpenAntiAirDefenseCommand { get; }
		public ICommand OpenFleetImageGeneratorCommand { get; }
		public ICommand OpenBaseAirCorpsSimulationCommand { get; }
		public ICommand OpenExpCheckerCommand { get; }
		public ICommand OpenExpeditionCheckCommand { get; }
		public ICommand OpenKancolleProgressCommand { get; }
		public ICommand OpenExtraBrowserCommand { get; }

		public ICommand LoadAPIFromFileCommand { get; }
		public ICommand LoadInitialAPICommand { get; }
		public ICommand LoadRecordFromOldCommand { get; }
		public ICommand LoadDataFromOldCommand { get; }
		public ICommand DeleteOldAPICommand { get; }
		public ICommand RenameShipResourceCommand { get; }

		public ICommand ViewHelpCommand { get; }
		public ICommand ReportIssueCommand { get; }
		public ICommand JoinDiscordCommand { get; }
		public ICommand CheckForUpdateCommand { get; }
		public ICommand ViewVersionCommand { get; }

		public ICommand OpenViewCommand { get; }
		public ICommand SaveLayoutCommand { get; }
		public ICommand LoadLayoutCommand { get; }
		public ICommand ClosingCommand { get; }
		public ICommand ClosedCommand { get; }

		#endregion

		public FormMainViewModel(DockingManager dockingManager, Control view)
		{
			View = view;
			DockingManager = dockingManager;

			#region Commands

			SaveDataCommand = new RelayCommand(StripMenu_File_SaveData_Save_Click);
			LoadDataCommand = new RelayCommand(StripMenu_File_SaveData_Load_Click);
			SilenceNotificationsCommand = new RelayCommand(StripMenu_File_Notification_MuteAll_Click);
			OpenConfigurationCommand = new RelayCommand(StripMenu_File_Configuration_Click);

			OpenEquipmentListCommand = new RelayCommand(StripMenu_Tool_EquipmentList_Click);
			OpenDropRecordCommand = new RelayCommand(StripMenu_Tool_DropRecord_Click);
			OpenDevelopmentRecordCommand = new RelayCommand(StripMenu_Tool_DevelopmentRecord_Click);
			OpenConstructionRecordCommand = new RelayCommand(StripMenu_Tool_ConstructionRecord_Click);
			OpenResourceChartCommand = new RelayCommand(StripMenu_Tool_ResourceChart_Click);
			OpenAlbumMasterShipCommand = new RelayCommand(StripMenu_Tool_AlbumMasterShip_Click);
			OpenAlbumMasterEquipmentCommand = new RelayCommand(StripMenu_Tool_AlbumMasterEquipment_Click);
			OpenAntiAirDefenseCommand = new RelayCommand(StripMenu_Tool_AntiAirDefense_Click);
			OpenFleetImageGeneratorCommand = new RelayCommand(StripMenu_Tool_FleetImageGenerator_Click);
			OpenBaseAirCorpsSimulationCommand = new RelayCommand(StripMenu_Tool_BaseAirCorpsSimulation_Click);
			OpenExpCheckerCommand = new RelayCommand(StripMenu_Tool_ExpChecker_Click);
			OpenExpeditionCheckCommand = new RelayCommand(StripMenu_Tool_ExpeditionCheck_Click);
			OpenKancolleProgressCommand = new RelayCommand(StripMenu_Tool_KancolleProgress_Click);
			OpenExtraBrowserCommand = new RelayCommand(StripMenu_Tool_ExtraBrowser_Click);

			LoadAPIFromFileCommand = new RelayCommand(StripMenu_Debug_LoadAPIFromFile_Click);
			LoadInitialAPICommand = new RelayCommand(StripMenu_Debug_LoadInitialAPI_Click);
			LoadRecordFromOldCommand = new RelayCommand(StripMenu_Debug_LoadRecordFromOld_Click);
			LoadDataFromOldCommand = new RelayCommand(StripMenu_Debug_LoadDataFromOld_Click);
			DeleteOldAPICommand = new RelayCommand(StripMenu_Debug_DeleteOldAPI_Click);
			RenameShipResourceCommand = new RelayCommand(StripMenu_Debug_RenameShipResource_Click);

			ViewHelpCommand = new RelayCommand(StripMenu_Help_Help_Click);
			ReportIssueCommand = new RelayCommand(StripMenu_Help_Issue_Click);
			JoinDiscordCommand = new RelayCommand(StripMenu_Help_Discord_Click);
			CheckForUpdateCommand = new RelayCommand(StripMenu_Help_Update_Click);
			ViewVersionCommand = new RelayCommand(StripMenu_Help_Version_Click);

			OpenViewCommand = new RelayCommand<AnchorableViewModel>(OpenView);
			SaveLayoutCommand = new RelayCommand<object>(SaveLayout);
			LoadLayoutCommand = new RelayCommand<object>(LoadLayout);
			ClosingCommand = new RelayCommand<CancelEventArgs>(Close);
			ClosedCommand = new RelayCommand(Closed);

			#endregion

			Config = Configuration.Config;
			FormMain = App.Current.Services.GetService<FormMainTranslationViewModel>()!;

			CultureInfo cultureInfo = new(Configuration.Config.UI.Culture);

			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;

			SetTheme();

			Utility.Logger.Instance.LogAdded += data =>
			{
				if (View.CheckAccess())
				{
					// Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
					// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
					// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
					View.Dispatcher.Invoke(new Utility.LogAddedEventHandler(Logger_LogAdded), data);
				}
				else
				{
					Logger_LogAdded(data);
				}
			};

			Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			string softwareName = CultureInfo.CurrentCulture.Name switch
			{
				"en-US" => SoftwareInformation.SoftwareNameEnglish,
				_ => SoftwareInformation.SoftwareNameJapanese
			};

			Utility.Logger.Add(2, softwareName + Properties.Window.FormMain.Starting);

			ResourceManager.Instance.Load();
			RecordManager.Instance.Load();
			KCDatabase.Instance.Load();
			NotifierManager.Instance.Initialize(View);
			SyncBGMPlayer.Instance.ConfigurationChanged();

			#region Icon settings

			ConfigurationImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormConfiguration);

			FleetsImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormFleet);
			FleetOverviewImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormFleet);
			ShipGroupImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormShipGroup);
			FleetPresetImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormFleetPreset);
			DockImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormDock);
			ArsenalImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormArsenal);
			BaseAirCorpsImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormBaseAirCorps);
			HeadquartersImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormHeadQuarters);
			QuestImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormQuest);
			InformationImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormInformation);
			CompassImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormCompass);
			BattleImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormBattle);
			BrowserHostImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormBrowser);
			LogImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormLog);
			JsonImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormJson);

			EquipmentListImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormEquipmentList);
			DropRecordImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormDropRecord);
			DevelopmentRecordImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormDevelopmentRecord);
			ConstructionRecordImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormConstructionRecord);
			ResourceChartImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormResourceChart);
			AlbumMasterShipImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormAlbumShip);
			AlbumMasterEquipmentImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormAlbumEquipment);
			AntiAirDefenseImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormAntiAirDefense);
			FleetImageGeneratorImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormFleetImageGenerator);
			BaseAirCorpsSimulationImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormBaseAirCorps);
			ExpCheckerImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormExpChecker);
			ExpeditionCheckImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormExpeditionCheck);
			KancolleProgressImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormEquipmentList);
			ExtraBrowserImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormBrowser);

			ViewHelpImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormInformation);
			ViewVersionImageSource = ImageSourceIcons.GetIcon(ResourceManager.IconContent.AppIcon);

			/*
			Icon = ResourceManager.Instance.AppIcon;

			StripMenu_File_Configuration.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration];

			StripMenu_View_Fleet.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet];
			StripMenu_View_FleetOverview.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet];
			StripMenu_View_ShipGroup.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormShipGroup];
			StripMenu_View_Dock.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDock];
			StripMenu_View_Arsenal.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormArsenal];
			StripMenu_View_Headquarters.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormHeadQuarters];
			StripMenu_View_Quest.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormQuest];
			StripMenu_View_Information.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation];
			StripMenu_View_Compass.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormCompass];
			StripMenu_View_Battle.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBattle];
			StripMenu_View_Browser.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBrowser];
			StripMenu_View_Log.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormLog];
			StripMenu_WindowCapture.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormWindowCapture];
			StripMenu_View_BaseAirCorps.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps];
			StripMenu_View_Json.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormJson];
			StripMenu_View_FleetPreset.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleetPreset];

			StripMenu_Tool_EquipmentList.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList];
			StripMenu_Tool_DropRecord.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDropRecord];
			StripMenu_Tool_DevelopmentRecord.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDevelopmentRecord];
			StripMenu_Tool_ConstructionRecord.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConstructionRecord];
			StripMenu_Tool_ResourceChart.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormResourceChart];
			StripMenu_Tool_AlbumMasterShip.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumShip];
			StripMenu_Tool_AlbumMasterEquipment.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumEquipment];
			StripMenu_Tool_AntiAirDefense.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAntiAirDefense];
			StripMenu_Tool_FleetImageGenerator.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleetImageGenerator];
			StripMenu_Tool_BaseAirCorpsSimulation.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps];
			StripMenu_Tool_ExpChecker.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormExpChecker];
			StripMenu_Tool_ExpeditionCheck.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormExpeditionCheck];
			StripMenu_Tool_KancolleProgress.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList];


			StripMenu_Help_Help.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation];
			StripMenu_Help_Version.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.AppIcon];
			*/

			#endregion

			APIObserver.Instance.Start(Configuration.Config.Connection.Port, View);

			Fleets = new List<FleetViewModel>()
			{
				new(1),
				new(2),
				new(3),
				new(4),
			};
			foreach (FleetViewModel fleet in Fleets)
			{
				Views.Add(fleet);
			}
			Views.Add(FleetOverview = new FleetOverviewViewModel(Fleets));
			Views.Add(FormShipGroup = new FormShipGroupViewModel());
			// Views.Add(ShipGroup = new());
			Views.Add(FleetPreset = new FleetPresetViewModel());

			Views.Add(Dock = new DockViewModel());
			Views.Add(Arsenal = new ArsenalViewModel());
			Views.Add(BaseAirCorps = new BaseAirCorpsViewModel());

			Views.Add(Headquarters = new HeadquartersViewModel());
			Views.Add(FormQuest = new FormQuestViewModel());
			Views.Add(FormInformation = new FormInformationViewModel());

			Views.Add(Compass = new CompassViewModel());
			Views.Add(Battle = new BattleViewModel());

			Views.Add(FormBrowserHost = new FormBrowserHostViewModel() {Visibility = Visibility.Visible});
			Views.Add(FormLog = new FormLogViewModel());
			Views.Add(FormJson = new FormJsonViewModel());

			ConfigurationChanged(); //設定から初期化

			// LoadLayout();
			
			SoftwareInformation.CheckUpdate();
			Task.Run(async () => await SoftwareUpdater.CheckUpdateAsync());
			CancellationTokenSource cts = new();
			Task.Run(async () => await SoftwareUpdater.PeriodicUpdateCheckAsync(cts.Token));

			/*
			// デバッグ: 開始時にAPIリストを読み込む
			if (Configuration.Config.Debug.LoadAPIListOnLoad)
			{
				try
				{
					await Task.Factory.StartNew(() => LoadAPIList(Configuration.Config.Debug.APIListPath));

					Activate();     // 上記ロードに時間がかかるとウィンドウが表示されなくなることがあるので
				}
				catch (Exception ex)
				{

					Utility.Logger.Add(3, LoggerRes.FailedLoadAPI + ex.Message);
				}
			}
			*/

			APIObserver.Instance.ResponseReceived += (a, b) => UpdatePlayTime();


			// 🎃
			if (DateTime.Now.Month == 10 && DateTime.Now.Day == 31)
			{
				APIObserver.Instance.APIList["api_port/port"].ResponseReceived += CallPumpkinHead;
			}

			// 完了通知（ログインページを開く）
			// fBrowser.InitializeApiCompleted();
			if (FormBrowserHost.WinformsControl is FormBrowserHost host)
			{
				host.InitializeApiCompleted();
			}

			NotificationsSilenced = NotifierManager.Instance.GetNotifiers().All(n => n.IsSilenced);

			UIUpdateTimer = new Timer() {Interval = 1000};
			UIUpdateTimer.Tick += UIUpdateTimer_Tick;
			UIUpdateTimer.Start();

			Logger.Add(3, Resources.StartupComplete);
		}

		#region File

		private void StripMenu_File_SaveData_Save_Click()
		{
			RecordManager.Instance.SaveAll();
		}

		private void StripMenu_File_SaveData_Load_Click()
		{
			if (MessageBox.Show(Resources.AskLoad, Properties.Window.FormMain.ConfirmatonCaption,
				    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
			    == MessageBoxResult.Yes)
			{
				RecordManager.Instance.Load();
			}
		}

		public void SaveLayout(object? sender)
		{
			if (sender is not FormMainWpf window) return;

			XmlLayoutSerializer serializer = new(DockingManager);
			serializer.Serialize(LayoutPath);

			Position.Top = window.Top;
			Position.Left = window.Left;
			Position.Height = window.Height;
			Position.Width = window.Width;
			Position.WindowState = window.WindowState;

			File.WriteAllText(PositionPath, JsonSerializer.Serialize(Position, new JsonSerializerOptions()
			{
				WriteIndented = true
			}));
		}

		public void LoadLayout(object? sender)
		{
			if (sender is not FormMainWpf window) return;
			if (!File.Exists(LayoutPath)) return;

			DockingManager.Layout = new LayoutRoot();

			XmlLayoutSerializer serializer = new(DockingManager);
			serializer.Deserialize(LayoutPath);

			if (File.Exists(PositionPath))
			{
				try
				{
					Position = JsonSerializer.Deserialize<WindowPosition>(File.ReadAllText(PositionPath)) ??
					           new WindowPosition();
				}
				catch
				{
					// couldn't get position, keep the default
				}
			}

			window.Top = Position.Top;
			window.Left = Position.Left;
			window.Width = Position.Width;
			window.Height = Position.Height;
			window.WindowState = Position.WindowState;
		}

		private void StripMenu_File_Notification_MuteAll_Click()
		{
			foreach (var n in NotifierManager.Instance.GetNotifiers())
				n.IsSilenced = NotificationsSilenced;
		}

		private void StripMenu_File_Configuration_Click()
		{
			UpdatePlayTime();

			using DialogConfiguration dialog = new(Configuration.Config);
			if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

			dialog.ToConfiguration(Configuration.Config);
			Configuration.Instance.OnConfigurationChanged();
		}

		#endregion

		#region View

		private void OpenView(AnchorableViewModel view)
		{
			view.Visibility = Visibility.Visible;
			view.IsSelected = true;
			view.IsActive = true;
		}

		#endregion

		#region Tools

		private void StripMenu_Tool_EquipmentList_Click()
		{
			new DialogEquipmentList().Show();
		}

		private void StripMenu_Tool_DropRecord_Click()
		{
			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
					MessageBoxImage.Error);
				return;
			}

			if (RecordManager.Instance.ShipDrop.Record.Count == 0)
			{
				MessageBox.Show(GeneralRes.NoDevData, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new DialogDropRecordViewer().Show();
		}

		private void StripMenu_Tool_DevelopmentRecord_Click()
		{
			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
					MessageBoxImage.Error);
				return;
			}

			if (RecordManager.Instance.Development.Record.Count == 0)
			{
				MessageBox.Show(GeneralRes.NoDevData, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new DialogDevelopmentRecordViewer().Show();
		}

		private void StripMenu_Tool_ConstructionRecord_Click()
		{
			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
					MessageBoxImage.Error);
				return;
			}

			if (RecordManager.Instance.Construction.Record.Count == 0)
			{
				MessageBox.Show(GeneralRes.NoBuildData, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new DialogConstructionRecordViewer().Show();
		}

		private void StripMenu_Tool_ResourceChart_Click()
		{
			new DialogResourceChart().Show();
		}

		private void StripMenu_Tool_AlbumMasterShip_Click()
		{

			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show(Properties.Window.FormMain.ShipDataNotLoaded, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new DialogAlbumMasterShip().Show();
		}

		private void StripMenu_Tool_AlbumMasterEquipment_Click()
		{

			if (KCDatabase.Instance.MasterEquipments.Count == 0)
			{
				MessageBox.Show(Properties.Window.FormMain.EquipmentDataNotLoaded, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new DialogAlbumMasterEquipment().Show();
		}

		private void StripMenu_Tool_AntiAirDefense_Click()
		{
			new DialogAntiAirDefense().Show();
		}

		private void StripMenu_Tool_FleetImageGenerator_Click()
		{
			new DialogFleetImageGenerator(1).Show();
		}

		private void StripMenu_Tool_BaseAirCorpsSimulation_Click()
		{
			new DialogBaseAirCorpsSimulation().Show();
		}

		private void StripMenu_Tool_ExpChecker_Click()
		{
			new DialogExpChecker().Show();
		}

		private void StripMenu_Tool_ExpeditionCheck_Click()
		{
			new DialogExpeditionCheck().Show();
		}

		private void StripMenu_Tool_KancolleProgress_Click()
		{
			new DialogKancolleProgressWpf().Show();
		}

		private void StripMenu_Tool_ExtraBrowser_Click()
		{
			Window.FormBrowserHost.Instance.Browser.OpenExtraBrowser();
		}

		#endregion

		#region Debug

		private void StripMenu_Debug_LoadAPIFromFile_Click()
		{

			/*/
			using ( var dialog = new DialogLocalAPILoader() ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
					if ( APIObserver.Instance.APIList.ContainsKey( dialog.APIName ) ) {

						if ( dialog.IsResponse ) {
							APIObserver.Instance.LoadResponse( dialog.APIPath, dialog.FileData );
						}
						if ( dialog.IsRequest ) {
							APIObserver.Instance.LoadRequest( dialog.APIPath, dialog.FileData );
						}

					}
				}
			}
			/*/
			new DialogLocalAPILoader2().Show();
			//*/
		}

		private async void StripMenu_Debug_LoadInitialAPI_Click()
		{
			using OpenFileDialog ofd = new();

			ofd.Title = "Load API List";
			ofd.Filter = "API List|*.txt|File|*";
			ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;
			if (!string.IsNullOrWhiteSpace(Utility.Configuration.Config.Debug.APIListPath))
				ofd.FileName = Utility.Configuration.Config.Debug.APIListPath;

			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				try
				{

					await Task.Factory.StartNew(() => LoadAPIList(ofd.FileName));

				}
				catch (Exception ex)
				{

					MessageBox.Show("Failed to load API List.\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption,
						MessageBoxButton.OK, MessageBoxImage.Error);

				}

			}
		}

		private void LoadAPIList(string path)
		{

			string parent = Path.GetDirectoryName(path);

			using StreamReader sr = new(path);
			string line;
			while ((line = sr.ReadLine()) != null)
			{

				bool isRequest = false;
				{
					int slashindex = line.IndexOf('/');
					if (slashindex != -1)
					{

						switch (line.Substring(0, slashindex).ToLower())
						{
							case "q":
							case "request":
								isRequest = true;
								goto case "s";
							case "":
							case "s":
							case "response":
								line = line.Substring(Math.Min(slashindex + 1, line.Length));
								break;
						}

					}
				}

				if (APIObserver.Instance.APIList.ContainsKey(line))
				{
					APIBase api = APIObserver.Instance.APIList[line];

					if (isRequest ? api.IsRequestSupported : api.IsResponseSupported)
					{

						string[] files = Directory.GetFiles(parent,
							string.Format("*{0}@{1}.json", isRequest ? "Q" : "S", line.Replace('/', '@')),
							SearchOption.TopDirectoryOnly);

						if (files.Length == 0)
							continue;

						Array.Sort(files);

						using StreamReader sr2 = new(files[files.Length - 1]);
						if (isRequest)
						{
							View.Dispatcher.Invoke((Action) (() =>
							{
								APIObserver.Instance.LoadRequest("/kcsapi/" + line, sr2.ReadToEnd());
							}));
						}
						else
						{
							View.Dispatcher.Invoke((Action) (() =>
							{
								APIObserver.Instance.LoadResponse("/kcsapi/" + line, sr2.ReadToEnd());
							}));
						}

						//System.Diagnostics.Debug.WriteLine( "APIList Loader: API " + line + " File " + files[files.Length-1] + " Loaded." );
					}
				}
			}
		}

		private void StripMenu_Debug_LoadRecordFromOld_Click()
		{

			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show("Please load normal api_start2 first.", Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK,
					MessageBoxImage.Information);
				return;
			}


			using OpenFileDialog ofd = new();

			ofd.Title = "Build Record from Old api_start2";
			ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";

			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				try
				{
					using StreamReader sr = new(ofd.FileName);
					dynamic json = JsonObject.Parse(sr.ReadToEnd().Remove(0, 7));

					foreach (dynamic elem in json.api_data.api_mst_ship)
					{
						if (elem.api_name != "なし" && KCDatabase.Instance.MasterShips.ContainsKey((int) elem.api_id) &&
						    KCDatabase.Instance.MasterShips[(int) elem.api_id].Name == elem.api_name)
						{
							RecordManager.Instance.ShipParameter.UpdateParameter((int) elem.api_id, 1,
								(int) elem.api_tais[0], (int) elem.api_tais[1], (int) elem.api_kaih[0],
								(int) elem.api_kaih[1], (int) elem.api_saku[0], (int) elem.api_saku[1]);

							int[] defaultslot = Enumerable.Repeat(-1, 5).ToArray();
							((int[]) elem.api_defeq).CopyTo(defaultslot, 0);
							RecordManager.Instance.ShipParameter.UpdateDefaultSlot((int) elem.api_id, defaultslot);
						}
					}
				}
				catch (Exception ex)
				{

					MessageBox.Show("Failed to load API.\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption,
						MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void StripMenu_Debug_LoadDataFromOld_Click()
		{

			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show("Please load normal api_start2 first.", Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK,
					MessageBoxImage.Information);
				return;
			}


			using OpenFileDialog ofd = new();

			ofd.Title = "Restore Abyssal Data from Old api_start2";
			ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";
			ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;

			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				try
				{

					using (StreamReader sr = new(ofd.FileName))
					{

						dynamic json = JsonObject.Parse(sr.ReadToEnd().Remove(0, 7));

						foreach (dynamic elem in json.api_data.api_mst_ship)
						{

							var ship = KCDatabase.Instance.MasterShips[(int) elem.api_id];

							if (elem.api_name != "なし" && ship != null && ship.IsAbyssalShip)
							{

								KCDatabase.Instance.MasterShips[(int) elem.api_id].LoadFromResponse("api_start2", elem);
							}
						}
					}

					Utility.Logger.Add(1, "Restored data from old api_start2");

				}
				catch (Exception ex)
				{

					MessageBox.Show("Failed to load API.\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption,
						MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private async void StripMenu_Debug_DeleteOldAPI_Click()
		{

			if (MessageBox.Show("This will delete old API data.\r\nAre you sure?", "Confirmation",
				    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
			    == MessageBoxResult.Yes)
			{

				try
				{

					int count = await Task.Factory.StartNew(() => DeleteOldAPI());

					MessageBox.Show("Delete successful.\r\n" + count + " files deleted.", "Delete Successful",
						MessageBoxButton.OK, MessageBoxImage.Information);

				}
				catch (Exception ex)
				{

					MessageBox.Show("Failed to delete.\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK,
						MessageBoxImage.Error);
				}


			}

		}

		private int DeleteOldAPI()
		{


			//適当極まりない
			int count = 0;

			var apilist = new Dictionary<string, List<KeyValuePair<string, string>>>();

			foreach (string s in Directory.EnumerateFiles(Utility.Configuration.Config.Connection.SaveDataPath,
				"*.json", SearchOption.TopDirectoryOnly))
			{

				int start = s.IndexOf('@');
				int end = s.LastIndexOf('.');

				start--;
				string key = s.Substring(start, end - start + 1);
				string date = s.Substring(0, start);


				if (!apilist.ContainsKey(key))
				{
					apilist.Add(key, new List<KeyValuePair<string, string>>());
				}

				apilist[key].Add(new KeyValuePair<string, string>(date, s));
			}

			foreach (var l in apilist.Values)
			{
				var l2 = l.OrderBy(el => el.Key).ToList();
				for (int i = 0; i < l2.Count - 1; i++)
				{
					File.Delete(l2[i].Value);
					count++;
				}
			}

			return count;
		}

		private async void StripMenu_Debug_RenameShipResource_Click()
		{

			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show("Ship data is not loaded.", Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (MessageBox.Show("通信から保存した艦船リソース名を持つファイル及びフォルダを、艦船名に置換します。\r\n" +
			                    "対象は指定されたフォルダ以下のすべてのファイル及びフォルダです。\r\n" +
			                    "続行しますか？", "艦船リソースをリネーム", MessageBoxButton.YesNo, MessageBoxImage.Question,
				    MessageBoxResult.Yes)
			    == MessageBoxResult.Yes)
			{

				string path = null;

				using (FolderBrowserDialog dialog = new())
				{
					dialog.SelectedPath = Configuration.Config.Connection.SaveDataPath;
					if (dialog.ShowDialog() == DialogResult.OK)
					{
						path = dialog.SelectedPath;
					}
				}

				if (path == null) return;



				try
				{

					int count = await Task.Factory.StartNew(() => RenameShipResource(path));

					MessageBox.Show(string.Format("リネーム処理が完了しました。\r\n{0} 個のアイテムをリネームしました。", count), "処理完了",
						MessageBoxButton.OK, MessageBoxImage.Information);


				}
				catch (Exception ex)
				{

					Utility.ErrorReporter.SendErrorReport(ex, "艦船リソースのリネームに失敗しました。");
					MessageBox.Show("艦船リソースのリネームに失敗しました。\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK,
						MessageBoxImage.Error);

				}



			}

		}


		private int RenameShipResource(string path)
		{

			int count = 0;

			foreach (var p in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
			{

				string name = Path.GetFileName(p);

				foreach (var ship in KCDatabase.Instance.MasterShips.Values)
				{

					if (name.Contains(ship.ResourceName))
					{

						name = name.Replace(ship.ResourceName,
							string.Format("{0}({1})", ship.NameWithClass, ship.ShipID)).Replace(' ', '_');

						try
						{

							File.Move(p, Path.Combine(Path.GetDirectoryName(p), name));
							count++;
							break;

						}
						catch (IOException)
						{
							//ファイルが既に存在する：＊にぎりつぶす＊
						}

					}

				}

			}

			foreach (var p in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
			{

				string name = Path.GetFileName(p); //GetDirectoryName だと親フォルダへのパスになってしまうため

				foreach (var ship in KCDatabase.Instance.MasterShips.Values)
				{

					if (name.Contains(ship.ResourceName))
					{

						name = name.Replace(ship.ResourceName,
							string.Format("{0}({1})", ship.NameWithClass, ship.ShipID)).Replace(' ', '_');

						try
						{

							Directory.Move(p, Path.Combine(Path.GetDirectoryName(p), name));
							count++;
							break;

						}
						catch (IOException)
						{
							//フォルダが既に存在する：＊にぎりつぶす＊
						}
					}

				}

			}


			return count;
		}


		#endregion

		#region Help

		private void StripMenu_Help_Help_Click()
		{

			if (MessageBox.Show(Properties.Window.FormMain.OpenEOWiki, Properties.Window.FormMain.HelpCaption,
				    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes)
			    == MessageBoxResult.Yes)
			{
				ProcessStartInfo psi = new()
				{
					FileName = "https://github.com/silfumus/ElectronicObserver/wiki",
					UseShellExecute = true
				};
				Process.Start(psi);
			}

		}

		private void StripMenu_Help_Issue_Click()
		{

			if (MessageBox.Show(Properties.Window.FormMain.ReportIssue, Properties.Window.FormMain.ReportIssueCaption,
				    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes)
			    == MessageBoxResult.Yes)
			{
				ProcessStartInfo psi = new()
				{
					FileName = "https://github.com/gre4bee/ElectronicObserver/issues",
					UseShellExecute = true
				};
				Process.Start(psi);
			}

		}

		private void StripMenu_Help_Discord_Click()
		{
			try
			{
				ProcessStartInfo psi = new()
				{
					FileName = @"https://discord.gg/6ZvX8DG",
					UseShellExecute = true
				};
				Process.Start(psi);
			}
			catch (Exception ex)
			{
				ErrorReporter.SendErrorReport(ex, Properties.Window.FormMain.FailedToOpenBrowser);
			}
		}

		private async void StripMenu_Help_Update_Click()
		{
			// translations and maintenance state
			await SoftwareUpdater.CheckUpdateAsync();
			// EO
			SoftwareInformation.CheckUpdate();
		}

		private void StripMenu_Help_Version_Click()
		{
			using DialogVersion dialog = new();
			dialog.ShowDialog();
		}

		#endregion

		private void CallPumpkinHead(string apiname, dynamic data)
		{
			new DialogHalloween().Show();
			APIObserver.Instance.APIList["api_port/port"].ResponseReceived -= CallPumpkinHead;
		}


		void Logger_LogAdded(Utility.Logger.LogData data)
		{
			// bottom bar
			StripStatus.Information = data.Message.Replace("\r", " ").Replace("\n", " ");
		}

		private void ConfigurationChanged()
		{
			var c = Configuration.Config;
			
			DebugEnabled = c.Debug.EnableDebugMenu;

			StripStatus.Visible = c.Life.ShowStatusBar;
			/*
			// Load で TopMost を変更するとバグるため(前述)
			if (UIUpdateTimer.Enabled)
				TopMost = c.Life.TopMost;
			
			*/
			ClockFormat = c.Life.ClockFormat;
			SetTheme();

			/*
			//StripMenu.Font = Font;
			StripStatus.Font = Font;
			MainDockPanel.Skin.AutoHideStripSkin.TextFont = Font;
			MainDockPanel.Skin.DockPaneStripSkin.TextFont = Font;

			foreach (var f in SubForms)
			{
				f.BackColor = this.BackColor;
				f.ForeColor = this.ForeColor;
				if (f is FormShipGroup)
				{ // 暂时不对舰队编成窗口应用主题
					f.BackColor = SystemColors.Control;
					f.ForeColor = SystemColors.ControlText;
				}
			}*/

			if (FormShipGroup.WinformsControl is not null)
			{
				FormShipGroup.WinformsControl.BackColor = System.Drawing.SystemColors.Control;
				FormShipGroup.WinformsControl.ForeColor = System.Drawing.SystemColors.ControlText;
			}

			/*

			StripStatus_Information.BackColor = System.Drawing.Color.Transparent;
			StripStatus_Information.Margin = new Padding(-1, 1, -1, 0);


			if (c.Life.LockLayout)
			{
				MainDockPanel.AllowChangeLayout = false;
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			}
			else
			{
				MainDockPanel.AllowChangeLayout = true;
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			}

			StripMenu_File_Layout_LockLayout.Checked = c.Life.LockLayout;
			MainDockPanel.CanCloseFloatWindowInLock = c.Life.CanCloseFloatWindowInLock;

			StripMenu_File_Layout_TopMost.Checked = c.Life.TopMost;

			StripMenu_File_Notification_MuteAll.Checked = Notifier.NotifierManager.Instance.GetNotifiers().All(n => n.IsSilenced);

			if (!c.Control.UseSystemVolume)
				_volumeUpdateState = -1;
			*/
		}


		private void SetFont()
		{
			Font = new FontFamily(Config.UI.MainFont.FontData.FontFamily.Name);
			FontSize = Config.UI.MainFont.FontData.Size * Config.UI.MainFont.FontData.Unit switch
			{
				System.Drawing.GraphicsUnit.Point => 4 / 3.0,
				_ => 1
			};
			FontBrush = Config.UI.ForeColor.ToBrush();

			SubFont = new FontFamily(Config.UI.SubFont.FontData.FontFamily.Name);
			SubFontSize = Config.UI.SubFont.FontData.Size;
			SubFontBrush = Config.UI.SubForeColor.ToBrush();
		}

		private void SetTheme()
		{
			// todo switching themes doesn't update everything in runtime
			Utility.Configuration.Instance.ApplyTheme();

			BackgroundColor = Config.UI.BackColor.ToBrush().Color;

			CurrentTheme = Utility.Configuration.Config.UI.ThemeMode switch
			{
				0 => Themes[0], // light theme => light theme
				1 => Themes[2], // dark theme => dark theme
				_ => Themes[2], // custom theme => dark theme
			};

			ThemeManager.Current.ApplicationTheme = Utility.Configuration.Config.UI.ThemeMode switch
			{
				0 => ApplicationTheme.Light, // light theme => light theme
				1 => ApplicationTheme.Dark, // dark theme => dark theme
				_ => ApplicationTheme.Dark, // custom theme => dark theme
			};

			SetFont();
		}

		private void UIUpdateTimer_Tick(object sender, EventArgs e)
		{

			SystemEvents.OnUpdateTimerTick();

			// 東京標準時
			DateTime now = Utility.Mathematics.DateTimeHelper.GetJapanStandardTimeNow();

			switch (ClockFormat)
			{
				case 0: //時計表示
					var pvpReset = now.Date.AddHours(3);
					while (pvpReset < now)
						pvpReset = pvpReset.AddHours(12);
					var pvpTimer = pvpReset - now;

					var questReset = now.Date.AddHours(5);
					if (questReset < now)
						questReset = questReset.AddHours(24);
					var questTimer = questReset - now;

					TimeSpan maintTimer = new(0);
					MaintenanceState eventState = SoftwareUpdater.LatestVersion.EventState;
					DateTime maintDate = SoftwareUpdater.LatestVersion.MaintenanceDate;

					if (eventState != MaintenanceState.None)
					{
						if (maintDate < now)
							maintDate = now;
						maintTimer = maintDate - now;
					}

					bool eventOrMaintenanceStarted = maintDate <= now;

					string message = (eventState, eventOrMaintenanceStarted) switch
					{
						(MaintenanceState.EventStart, false) => FormMain.EventStartsIn,
						(MaintenanceState.EventStart, _) => FormMain.EventHasStarted,

						(MaintenanceState.EventEnd, false) => FormMain.EventEndsIn,
						(MaintenanceState.EventEnd, _) => FormMain.EventHasEnded,

						(MaintenanceState.Regular, false) => FormMain.MaintenanceStartsIn,
						(MaintenanceState.Regular, _) => FormMain.MaintenanceHasStarted,

						_ => string.Empty,
					};

					string maintState = eventOrMaintenanceStarted switch
					{
						false => string.Format(message, maintTimer.ToString("dd\\ hh\\:mm\\:ss")),
						_ => message
					};

					var resetMsg =
						$"{FormMain.NextExerciseReset} {pvpTimer:hh\\:mm\\:ss}\r\n" +
						$"{FormMain.NextQuestReset} {questTimer:hh\\:mm\\:ss}\r\n" +
						$"{maintState}";

					StripStatus.Clock = now.ToString("HH\\:mm\\:ss");
					StripStatus.ClockToolTip = now.ToString("yyyy\\/MM\\/dd (ddd)\r\n") + resetMsg;

					break;

				case 1: //演習更新まで
				{
					var border = now.Date.AddHours(3);
					while (border < now)
						border = border.AddHours(12);

					var ts = border - now;
					StripStatus.Clock = ts.ToString("hh\\:mm\\:ss");
					StripStatus.ClockToolTip = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

				}
					break;

				case 2: //任務更新まで
				{
					var border = now.Date.AddHours(5);
					if (border < now)
						border = border.AddHours(24);

					var ts = border - now;
					StripStatus.Clock = ts.ToString("hh\\:mm\\:ss");
					StripStatus.ClockToolTip = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

				}
					break;
			}

			/*
			// WMP コントロールによって音量が勝手に変えられてしまうため、前回終了時の音量の再設定を試みる。
			// 10回試行してダメなら諦める(例外によるラグを防ぐため)
			// 起動直後にやらないのはちょっと待たないと音量設定が有効にならないから
			if (_volumeUpdateState != -1 && _volumeUpdateState < 10 && Utility.Configuration.Config.Control.UseSystemVolume)
			{

				try
				{
					uint id = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
					float volume = Utility.Configuration.Config.Control.LastVolume;
					bool mute = Utility.Configuration.Config.Control.LastIsMute;

					BrowserLibCore.VolumeManager.SetApplicationVolume(id, volume);
					BrowserLibCore.VolumeManager.SetApplicationMute(id, mute);

					SyncBGMPlayer.Instance.SetInitialVolume((int)(volume * 100));
					foreach (var not in NotifierManager.Instance.GetNotifiers())
						not.SetInitialVolume((int)(volume * 100));

					_volumeUpdateState = -1;

				}
				catch (Exception)
				{

					_volumeUpdateState++;
				}
			}
			*/
		}

		private void UpdatePlayTime()
		{
			var c = Configuration.Config.Log;
			DateTime now = DateTime.Now;

			double span = (now - PrevPlayTimeRecorded).TotalSeconds;
			if (span < c.PlayTimeIgnoreInterval)
			{
				c.PlayTime += span;
			}

			PrevPlayTimeRecorded = now;
		}

		private void Close(CancelEventArgs e)
		{
			string name = CultureInfo.CurrentCulture.Name switch
			{
				"en-US" => SoftwareInformation.SoftwareNameEnglish,
				_ => SoftwareInformation.SoftwareNameJapanese
			};

			if (Configuration.Config.Life.ConfirmOnClosing)
			{
				if (MessageBox.Show(
					    string.Format(Properties.Window.FormMain.ExitConfirmation, name),
					    Properties.Window.FormMain.ConfirmatonCaption,
					    MessageBoxButton.YesNo,
					    MessageBoxImage.Question,
					    MessageBoxResult.No)
				    == MessageBoxResult.No)
				{
					e.Cancel = true;
					return;
				}
			}

			Logger.Add(2, name + Resources.IsClosing);

			UIUpdateTimer.Stop();

			if (FormBrowserHost.WinformsControl is FormBrowserHost host)
			{
				host.CloseBrowser();
			}

			UpdatePlayTime();
			SystemEvents.OnSystemShuttingDown();

			// SaveLayout(Configuration.Config.Life.LayoutFilePath);

			// 音量の保存
			{
				try
				{
					uint id = (uint) Process.GetCurrentProcess().Id;
					Configuration.Config.Control.LastVolume = BrowserLibCore.VolumeManager.GetApplicationVolume(id);
					Configuration.Config.Control.LastIsMute = BrowserLibCore.VolumeManager.GetApplicationMute(id);

				}
				catch (Exception)
				{
					/* ぷちっ */
				}

			}
		}

		private void Closed()
		{
			NotifierManager.Instance.ApplyToConfiguration();
			Configuration.Instance.Save();
			RecordManager.Instance.SavePartial();
			KCDatabase.Instance.Save();
			APIObserver.Instance.Stop();

			Logger.Add(2, Resources.ClosingComplete);

			if (Configuration.Config.Log.SaveLogFlag)
				Logger.Save();
		}
	}
}