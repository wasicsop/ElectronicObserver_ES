using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.Fleet;
using ElectronicObserverTypes;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.FleetOverview;

public class FleetOverviewItemViewModel : ObservableObject
{
	public string? Text { get; set; }
	public string? ToolTip { get; set; }
	public ImageSource? Icon { get; set; }
	public bool Visible { get; set; }
	public DateTime? Tag { get; set; }

	public Visibility Visibility => Visible.ToVisibility();
}

public class FleetOverviewViewModel : AnchorableViewModel
{
	public FormFleetOverviewTranslationViewModel FormFleetOverview { get; }

	public List<FleetViewModel> Fleets { get; }
	public FleetOverviewItemViewModel AnchorageRepairingTimer { get; }
	public FleetOverviewItemViewModel CombinedTag { get; }

	public FleetOverviewViewModel(List<FleetViewModel> fleets) : base("Fleets", "Fleets",
		ImageSourceIcons.GetIcon(IconContent.FormFleet))
	{
		FormFleetOverview = App.Current.Services.GetService<FormFleetOverviewTranslationViewModel>()!;

		Title = FormFleetOverview.Title;
		FormFleetOverview.PropertyChanged += (_, _) => Title = FormFleetOverview.Title;

		Fleets = fleets;

		//api register
		APIObserver o = APIObserver.Instance;

		o["api_req_nyukyo/start"].RequestReceived += Updated;
		o["api_req_nyukyo/speedchange"].RequestReceived += Updated;
		o["api_req_hensei/change"].RequestReceived += Updated;
		o["api_req_kousyou/destroyship"].RequestReceived += Updated;
		o["api_req_member/updatedeckname"].RequestReceived += Updated;
		o["api_req_map/start"].RequestReceived += Updated;
		o["api_req_hensei/combined"].RequestReceived += Updated;
		o["api_req_kaisou/open_exslot"].RequestReceived += Updated;

		o["api_port/port"].ResponseReceived += Updated;
		o["api_get_member/ship2"].ResponseReceived += Updated;
		o["api_get_member/ndock"].ResponseReceived += Updated;
		o["api_req_kousyou/getship"].ResponseReceived += Updated;
		o["api_req_hokyu/charge"].ResponseReceived += Updated;
		o["api_req_kousyou/destroyship"].ResponseReceived += Updated;
		o["api_get_member/ship3"].ResponseReceived += Updated;
		o["api_req_kaisou/powerup"].ResponseReceived += Updated; //requestのほうは面倒なのでこちらでまとめてやる
		o["api_get_member/deck"].ResponseReceived += Updated;
		o["api_req_map/start"].ResponseReceived += Updated;
		o["api_req_map/next"].ResponseReceived += Updated;
		o["api_get_member/ship_deck"].ResponseReceived += Updated;
		o["api_req_hensei/preset_select"].ResponseReceived += Updated;
		o["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;
		o["api_get_member/require_info"].ResponseReceived += Updated;
		o["api_req_kaisou/slot_deprive"].ResponseReceived += Updated;
		o["api_req_kaisou/marriage"].ResponseReceived += Updated;
		o["api_req_map/anchorage_repair"].ResponseReceived += Updated;


		AnchorageRepairingTimer = new()
		{
			// Anchor = AnchorStyles.Left,
			// Padding = new Padding(0, 1, 0, 1),
			// Margin = new Padding(2, 1, 2, 1),
			// ImageList = ResourceManager.Instance.Icons,
			// ImageIndex = (int)ResourceManager.IconContent.FleetAnchorageRepairing,
			Text = "-",
			Icon = ImageSourceIcons.GetIcon(IconContent.FleetAnchorageRepairing),
			// AutoSize = true
			Visible = false
		};

		// TableFleet.Controls.Add(AnchorageRepairingTimer, 1, 4);

		CombinedTag = new()
		{
			// Anchor = AnchorStyles.Left,
			// Padding = new Padding(0, 1, 0, 1),
			// Margin = new Padding(2, 1, 2, 1),
			// ImageList = ResourceManager.Instance.Icons,
			// ImageIndex = (int)ResourceManager.IconContent.FleetCombined,
			Text = "-",
			Icon = ImageSourceIcons.GetIcon(IconContent.FleetCombined),

			// AutoSize = true,
			Visible = false
		};

		// TableFleet.Controls.Add(CombinedTag, 1, 5);

		ConfigurationChanged();

		Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;
		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	void ConfigurationChanged()
	{
		// TableFleet.SuspendLayout();

		// AutoScroll = Utility.Configuration.Config.FormFleet.IsScrollable;

		/*
		foreach (var c in ControlFleet)
			c.ConfigurationChanged(this);
		*/

		// CombinedTag.Font = Font;
		// AnchorageRepairingTimer.Font = Font;
		AnchorageRepairingTimer.Visible = Utility.Configuration.Config.FormFleet.ShowAnchorageRepairingTimer;

		// LayoutSubInformation();

		// ControlHelper.SetTableRowStyles(TableFleet, ControlHelper.GetDefaultRowStyle());

		// TableFleet.ResumeLayout();
	}


	private void Updated(string apiname, dynamic data)
	{

		// TableFleet.SuspendLayout();

		// TableFleet.RowCount = KCDatabase.Instance.Fleet.Fleets.Values.Count(f => f.IsAvailable);
		// for (int i = 0; i < ControlFleet.Count; i++)
		// {
		// 	ControlFleet[i].Update();
		// }

		if (KCDatabase.Instance.Fleet.CombinedFlag > 0)
		{
			CombinedTag.Text = Constants.GetCombinedFleet(KCDatabase.Instance.Fleet.CombinedFlag);

			var fleet1 = KCDatabase.Instance.Fleet[1];
			var fleet2 = KCDatabase.Instance.Fleet[2];

			int tp = Calculator.GetTPDamage(fleet1) + Calculator.GetTPDamage(fleet2);

			var members = fleet1.MembersWithoutEscaped.Concat(fleet2.MembersWithoutEscaped).Where(s => s != null);

			// 各艦ごとの ドラム缶 or 大発系 を搭載している個数
			var transport = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.TransportContainer));
			var landing = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.LandingCraft || eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank));
			var radar = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.IsSurfaceRadar == true));

			CombinedTag.ToolTip = string.Format(FormFleetOverview.CombinedFleetToolTip,
				transport.Sum(),
				landing.Sum(),
				tp,
				(int)Math.Floor(tp * 0.7),
				Calculator.GetAirSuperiority(fleet1) + Calculator.GetAirSuperiority(fleet2),
				Math.Floor(fleet1.GetSearchingAbility() * 100) / 100 + Math.Floor(fleet2.GetSearchingAbility() * 100) / 100,
				Math.Floor(Calculator.GetSearchingAbility_New33(fleet1, 1) * 100) / 100 + Math.Floor(Calculator.GetSearchingAbility_New33(fleet2, 1) * 100) / 100,
				Math.Floor(Calculator.GetSearchingAbility_New33(fleet1, 2) * 100) / 100 + Math.Floor(Calculator.GetSearchingAbility_New33(fleet2, 2) * 100) / 100,
				Math.Floor(Calculator.GetSearchingAbility_New33(fleet1, 3) * 100) / 100 + Math.Floor(Calculator.GetSearchingAbility_New33(fleet2, 3) * 100) / 100,
				Math.Floor(Calculator.GetSearchingAbility_New33(fleet1, 4) * 100) / 100 + Math.Floor(Calculator.GetSearchingAbility_New33(fleet2, 4) * 100) / 100,
				radar.Sum(),
				radar.Count(i => i > 0),
				transport.Count(i => i> 0),
				landing.Count(i => i > 0)

			);


			CombinedTag.Visible = true;
		}
		else
		{
			CombinedTag.Visible = false;
		}

		if (KCDatabase.Instance.Fleet.AnchorageRepairingTimer > DateTime.MinValue)
		{
			AnchorageRepairingTimer.Text = DateTimeHelper.ToTimeElapsedString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer);
			AnchorageRepairingTimer.Tag = KCDatabase.Instance.Fleet.AnchorageRepairingTimer;
			AnchorageRepairingTimer.ToolTip =
				FormFleetOverview.AnchorageRepairToolTip +
				DateTimeHelper.TimeToCSVString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer) +
				$"\r\n{FormFleetOverview.Recovery}: " +
				DateTimeHelper.TimeToCSVString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer.AddMinutes(20));
		}

	}


	void UpdateTimerTick()
	{
		// for (int i = 0; i < ControlFleet.Count; i++)
		// {
		// 	ControlFleet[i].Refresh();
		// }

		if (AnchorageRepairingTimer.Visible && AnchorageRepairingTimer.Tag != null)
			AnchorageRepairingTimer.Text = DateTimeHelper.ToTimeElapsedString((DateTime)AnchorageRepairingTimer.Tag);
	}
}
