using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AvalonDock;
using AvalonDock.Controls;
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
using ElectronicObserver.Window.Support;
using ElectronicObserver.Window.Wpf;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserver.Window.Wpf.WinformsWrappers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.ViewModels
{
	public class FormMainViewModel : ObservableObject
	{
		public ObservableCollection<AnchorableViewModel> Views { get; } = new();

		public List<FleetViewModel> Fleets { get; }
		public FormBrowserHostViewModel FormBrowserHost { get; }

		public LogViewModel LogViewModel { get; }

		public ICommand OpenViewCommand { get; }
		public ICommand SaveLayoutCommand { get; }
		public ICommand LoadLayoutCommand { get; }

		private DockingManager DockingManager { get; }
		
		public FormMainViewModel(DockingManager dockingManager)
		{
			DockingManager = dockingManager;

			OpenViewCommand = new RelayCommand<AnchorableViewModel>(OpenView);
			SaveLayoutCommand = new RelayCommand(SaveLayout);
			LoadLayoutCommand = new RelayCommand(LoadLayout);

			if (!Directory.Exists("Settings"))
				Directory.CreateDirectory("Settings");


			// Utility.Configuration.Instance.Load(this);
			// todo the parameter is never used, remove it later
			Utility.Configuration.Instance.Load(null!);

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

			Utility.Logger.Add(2, SoftwareInformation.SoftwareNameEnglish + " is starting...");


			ResourceManager.Instance.Load();
			RecordManager.Instance.Load();
			KCDatabase.Instance.Load();
			// NotifierManager.Instance.Initialize(this);
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


			APIObserver.Instance.Start(Utility.Configuration.Config.Connection.Port, this);


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

			UIUpdateTimer.Start();
			*/

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
			Views.Add(FormBrowserHost = new());
			Views.Add(LogViewModel = new());

			// LoadLayout();
		}

		private void OpenView(AnchorableViewModel view)
		{
			view.Visibility = Visibility.Visible;
			view.IsSelected = true;
			view.IsActive = true;
		}

		private string LayoutPath => @"Settings\DefaultLayout.xml";

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
	}
}