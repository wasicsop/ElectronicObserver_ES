using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using ElectronicObserver.AvalonDockTesting;
using ElectronicObserver.Data;
using ElectronicObserver.Notifier;
using ElectronicObserver.Observer;
using ElectronicObserver.Properties;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserver.Window.Wpf.WinformsWrappers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.ViewModels
{
	public class FormMainViewModel : ObservableObject
	{
		private Control View { get; }
		private DockingManager DockingManager { get; }
		private System.Windows.Forms.Timer UIUpdateTimer { get; }
		private string LayoutPath { get; } = @"Settings\DefaultLayout.xml";
		public bool NotificationsSilenced { get; set; }
		private DateTime PrevPlayTimeRecorded { get; set; } = DateTime.MinValue;

		public ObservableCollection<AnchorableViewModel> Views { get; } = new();

		public List<FleetViewModel> Fleets { get; }

		public List<FormFleetViewModel> FormFleets { get; }
		public FormFleetOverviewViewModel FormFleetOverview { get; }
		public FormShipGroupViewModel FormShipGroup { get; }
		public FormFleetPresetViewModel FormFleetPreset { get; }
		public FormDockViewModel FormDock { get; }
		public FormArsenalViewModel FormArsenal { get; }
		public FormBaseAirCorpsViewModel FormBaseAirCorps { get; }
		public FormHeadquartersViewModel FormHeadquarters { get; }
		public FormQuestViewModel FormQuest { get; }
		public FormInformationViewModel FormInformation { get; }
		public FormCompassViewModel FormCompass { get; }
		public FormBattleViewModel FormBattle { get; }
		public FormBrowserHostViewModel FormBrowserHost { get; }
		public FormLogViewModel FormLog { get; }

		public LogViewModel LogViewModel { get; }

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

		public ICommand OpenViewCommand { get; }
		public ICommand SaveLayoutCommand { get; }
		public ICommand LoadLayoutCommand { get; }
		public ICommand ClosingCommand { get; }

		public FormMainViewModel(DockingManager dockingManager, Control view)
		{
			View = view;
			DockingManager = dockingManager;

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

			OpenViewCommand = new RelayCommand<AnchorableViewModel>(OpenView);
			SaveLayoutCommand = new RelayCommand(SaveLayout);
			LoadLayoutCommand = new RelayCommand(LoadLayout);
			ClosingCommand = new RelayCommand<CancelEventArgs>(Close);

			if (!Directory.Exists("Settings"))
				Directory.CreateDirectory("Settings");

			
			// todo the parameter is never used, remove it later
			Configuration.Instance.Load(null!);

			/*
			this.MainDockPanel.Styles = Configuration.Config.UI.DockPanelSuiteStyles;
			this.MainDockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2012Theme();
			this.BackColor = this.StripMenu.BackColor = Utility.Configuration.Config.UI.BackColor;
			this.ForeColor = this.StripMenu.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			this.StripStatus.BackColor = Utility.Configuration.Config.UI.StatusBarBackColor;
			this.StripStatus.ForeColor = Utility.Configuration.Config.UI.StatusBarForeColor;
			*/

			/*
			Utility.Logger.Instance.LogAdded += new Utility.LogAddedEventHandler((Utility.Logger.LogData data) =>
			{
				if (InvokeRequired)
				{
					// Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
					// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
					// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
					Invoke(new Utility.LogAddedEventHandler(Logger_LogAdded), data);
				}
				else
				{
					Logger_LogAdded(data);
				}
			});
			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
			*/

			Logger.Add(2, SoftwareInformation.SoftwareNameEnglish + " is starting...");


			ResourceManager.Instance.Load();
			RecordManager.Instance.Load();
			KCDatabase.Instance.Load();
			NotifierManager.Instance.Initialize(View);
			SyncBGMPlayer.Instance.ConfigurationChanged();

			#region Icon settings
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
			StripMenu_Tool_KancolleProgress.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList];


			StripMenu_Help_Help.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation];
			StripMenu_Help_Version.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.AppIcon];
			*/
			#endregion


			APIObserver.Instance.Start(Utility.Configuration.Config.Connection.Port, View);


			// MainDockPanel.Extender.FloatWindowFactory = new CustomFloatWindowFactory();


			// SubForms = new List<DockContent>();
			/*
			//form init
			//注：一度全てshowしないとイベントを受け取れないので注意
			fFleet = new FormFleet[4];
			for (int i = 0; i < fFleet.Length; i++)
			{
				SubForms.Add(fFleet[i] = new FormFleet(this, i + 1));
			}

			SubForms.Add(fDock = new FormDock(this));
			SubForms.Add(fArsenal = new FormArsenal(this));
			SubForms.Add(fHeadquarters = new FormHeadquarters(this));
			SubForms.Add(fInformation = new FormInformation(this));
			SubForms.Add(fCompass = new FormCompass(this));
			SubForms.Add(fLog = new FormLog(this));
			SubForms.Add(fQuest = new FormQuest(this));
			SubForms.Add(fBattle = new FormBattle(this));
			SubForms.Add(fFleetOverview = new FormFleetOverview(this));
			SubForms.Add(fShipGroup = new FormShipGroup(this));
			SubForms.Add(fBrowser = new FormBrowserHost(this));
			SubForms.Add(fWindowCapture = new FormWindowCapture(this));
			SubForms.Add(fXPCalculator = new FormXPCalculator(this));
			SubForms.Add(fBaseAirCorps = new FormBaseAirCorps(this));
			SubForms.Add(fJson = new FormJson(this));
			SubForms.Add(fFleetPreset = new FormFleetPreset(this));
			*/

			// ConfigurationChanged();     //設定から初期化

			// LoadLayout(Configuration.Config.Life.LayoutFilePath);


#if !DEBUG
			SoftwareInformation.CheckUpdate();
			await SoftwareUpdater.CheckUpdateAsync();
			CancellationTokenSource cts = new CancellationTokenSource();
			Task.Run(async () => await SoftwareUpdater.PeriodicUpdateCheckAsync(cts.Token));
#endif
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

			APIObserver.Instance.ResponseReceived += (a, b) => UpdatePlayTime();


			// 🎃
			if (DateTime.Now.Month == 10 && DateTime.Now.Day == 31)
			{
				APIObserver.Instance.APIList["api_port/port"].ResponseReceived += CallPumpkinHead;
			}

			// 完了通知（ログインページを開く）
			fBrowser.InitializeApiCompleted();

			*/
			UIUpdateTimer = new() {Interval = 1000};
			UIUpdateTimer.Tick += UIUpdateTimer_Tick;
			UIUpdateTimer.Start();

			Utility.Logger.Add(3, Resources.StartupComplete);

			Fleets = new()
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

			FormFleets = new()
			{
				new(1),
				new(2),
				new(3),
				new(4),
			};
			foreach (FormFleetViewModel fleet in FormFleets)
			{
				Views.Add(fleet);
			}
			Views.Add(FormFleetOverview = new());
			Views.Add(FormShipGroup = new());
			Views.Add(FormFleetPreset = new());
			Views.Add(FormDock = new());
			Views.Add(FormArsenal = new());
			Views.Add(FormBaseAirCorps = new());
			Views.Add(FormHeadquarters = new());
			Views.Add(FormQuest = new());
			Views.Add(FormInformation = new());
			Views.Add(FormCompass = new());
			Views.Add(FormBattle = new());
			Views.Add(FormBrowserHost = new());
			Views.Add(FormLog = new());

			Views.Add(LogViewModel = new());

			NotificationsSilenced = NotifierManager.Instance.GetNotifiers().All(n => n.IsSilenced);

			// LoadLayout();
		}

		#region File

		private void StripMenu_File_SaveData_Save_Click()
		{
			RecordManager.Instance.SaveAll();
		}

		private void StripMenu_File_SaveData_Load_Click()
		{
			if (MessageBox.Show(Resources.AskLoad, "Confirmation",
				    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
			    == MessageBoxResult.Yes)
			{
				RecordManager.Instance.Load();
			}
		}

		public void SaveLayout()
		{
			XmlLayoutSerializer serializer = new(DockingManager);
			serializer.Serialize(LayoutPath);
		}

		public void LoadLayout()
		{
			if (!File.Exists(LayoutPath)) return;

			DockingManager.Layout = new LayoutRoot();

			XmlLayoutSerializer serializer = new(DockingManager);
			serializer.Deserialize(LayoutPath);
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
				MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (RecordManager.Instance.ShipDrop.Record.Count == 0)
			{
				MessageBox.Show(GeneralRes.NoDevData, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new DialogDropRecordViewer().Show();
		}

		private void StripMenu_Tool_DevelopmentRecord_Click()
		{
			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (RecordManager.Instance.Development.Record.Count == 0)
			{
				MessageBox.Show(GeneralRes.NoDevData, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new DialogDevelopmentRecordViewer().Show();
		}

		private void StripMenu_Tool_ConstructionRecord_Click()
		{
			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (RecordManager.Instance.Construction.Record.Count == 0)
			{
				MessageBox.Show(GeneralRes.NoBuildData, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
				MessageBox.Show("Ship data is not loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new DialogAlbumMasterShip().Show();
		}

		private void StripMenu_Tool_AlbumMasterEquipment_Click()
		{

			if (KCDatabase.Instance.MasterEquipments.Count == 0)
			{
				MessageBox.Show("Equipment data is not loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

		private void UIUpdateTimer_Tick(object sender, EventArgs e)
		{

			SystemEvents.OnUpdateTimerTick();

			// 東京標準時
			DateTime now = Utility.Mathematics.DateTimeHelper.GetJapanStandardTimeNow();
			/*
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

					TimeSpan maintTimer = new TimeSpan(0);
					MaintenanceState eventState = SoftwareUpdater.LatestVersion.EventState;
					DateTime maintDate = SoftwareUpdater.LatestVersion.MaintenanceDate;

					if (eventState != MaintenanceState.None)
					{
						if (maintDate < now)
							maintDate = now;
						maintTimer = maintDate - now;
					}

					string message = eventState switch
					{
						MaintenanceState.EventStart => maintDate > now ? "Event starts in" : "Event has started!",
						MaintenanceState.EventEnd => maintDate > now ? "Event ends in" : "Event period has ended.",
						MaintenanceState.Regular => maintDate > now ? "Maintenance starts in" : "Maintenance has started.",
						_ => string.Empty,
					};

					string maintState;
					if (maintDate > now)
					{
						var hours = $"{maintTimer.Days}d {maintTimer.Hours}h";
						if ((int)maintTimer.TotalHours < 24)
							hours = $"{maintTimer.Hours}h";
						maintState = $"{message} {hours} {maintTimer.Minutes}m {maintTimer.Seconds}s";
					}
					else
						maintState = message;

					var resetMsg =
						$"Next PVP reset: {(int)pvpTimer.TotalHours:D2}:{pvpTimer.Minutes:D2}:{pvpTimer.Seconds:D2}\r\n" +
						$"Next Quest reset: {(int)questTimer.TotalHours:D2}:{questTimer.Minutes:D2}:{questTimer.Seconds:D2}\r\n" +
						$"{maintState}";

					StripStatus_Clock.Text = now.ToString("HH\\:mm\\:ss");
					StripStatus_Clock.ToolTipText = now.ToString("yyyy\\/MM\\/dd (ddd)\r\n") + resetMsg;

					break;

				case 1: //演習更新まで
					{
						var border = now.Date.AddHours(3);
						while (border < now)
							border = border.AddHours(12);

						var ts = border - now;
						StripStatus_Clock.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
						StripStatus_Clock.ToolTipText = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

					}
					break;

				case 2: //任務更新まで
					{
						var border = now.Date.AddHours(5);
						if (border < now)
							border = border.AddHours(24);

						var ts = border - now;
						StripStatus_Clock.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
						StripStatus_Clock.ToolTipText = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

					}
					break;
			}


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
			if (Configuration.Config.Life.ConfirmOnClosing)
			{
				if (MessageBox.Show(
					    "Are you sure you want to exit?",
					    "Electronic Observer",
					    MessageBoxButton.YesNo,
					    MessageBoxImage.Question,
					    MessageBoxResult.No)
				    == MessageBoxResult.No)
				{
					e.Cancel = true;
					return;
				}
			}

			Logger.Add(2, SoftwareInformation.SoftwareNameEnglish + Resources.IsClosing);


		}
	}
}