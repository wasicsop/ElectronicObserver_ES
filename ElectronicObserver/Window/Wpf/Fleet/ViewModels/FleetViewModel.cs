using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PropertyChanged;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels
{
	public class FleetViewModel : AnchorableViewModel
	{
		private IFleetData? _fleet;

		[DoNotNotify]
		public IFleetData? Fleet
		{
			get => _fleet;
			set
			{
				_fleet = value;
				OnPropertyChanged(nameof(FleetVisibility));
				if (Fleet == null) return;

				Name = Fleet.Name;
				Air = Fleet.GetAirSuperiority();
				FleetAA = Calculator.GetAdjustedFleetAAValue(Fleet, 1);

				UpdateFleetState(Fleet);
				for (int i = 0; i < ShipViewModels.Count; i++)
				{
					ShipViewModels[i].Fleet = Fleet;
					ShipViewModels[i].Ship = Fleet.MembersInstance[i];
				}

				OnPropertyChanged(nameof(Fleet));
				OnPropertyChanged(nameof(HasTaihaShip));
				OnPropertyChanged(nameof(LosString));

				OnPropertyChanged(nameof(FleetNameToolTip));
				OnPropertyChanged(nameof(FleetAirToolTip));
				OnPropertyChanged(nameof(FleetLosToolTip));
				OnPropertyChanged(nameof(FleetAaToolTip));
			}
		}

		private int FleetId { get; set; }
		private int BranchWeight { get; set; } = 1;

		public Visibility FleetVisibility => Fleet switch
		{
			null => Visibility.Collapsed,
			_ => Visibility.Visible
		};

		public string Name { get; set; } = "";
		private int Air { get; set; }
		private double FleetAA { get; set; }

		public string AirString => Air.ToString();
		public string LosString => Fleet?.GetSearchingAbilityString(BranchWeight) ?? "0";
		public string FleetAaString => FleetAA.ToString("N1");

		public ImageSource? AirImage => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.CarrierBasedFighter);
		public ImageSource? LosImage => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.CarrierBasedRecon);
		public ImageSource? FleetAaImage => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.HighAngleGun);

		public string FleetNameToolTip => GetNameString(Fleet);
		public string FleetAirToolTip => GetFleetAirString(Fleet);
		public string FleetLosToolTip => GetFleetLosString(Fleet, BranchWeight);
		public string FleetAaToolTip => GetFleetAaString(Fleet);

		public bool HasTaihaShip => Ships
			.Where(s => s.Ship != null)
			.Any(s => s.HPRate < .25);

		private List<ShipViewModel> ShipViewModels { get; set; } = new()
		{
			new(),
			new(),
			new(),
			new(),
			new(),
			new(),
		};
		public IEnumerable<ShipViewModel> Ships => ShipViewModels;

		public IRelayCommand CopyCommand { get; }
		public IRelayCommand CopyDeckBuilderCommand { get; }
		public IRelayCommand CopyKanmusuListCommand { get; }
		public IRelayCommand CopyFleetAnalysisCommand { get; }
		public IRelayCommand CopyFleetAnalysisEquipCommand { get; }
		public IRelayCommand CopyFleetAnalysisAllEquipCommand { get; }

		public IRelayCommand AntiAirDetailsCommand { get; }
		public IRelayCommand OutputFleetImageCommand { get; }

		public IRelayCommand IncreaseBranchWeightCommand { get; }

		private DispatcherTimer Timer { get; }

		private Action<ResourceManager.IconContent> SetIcon { get; }

		public FleetViewModel(int fleetId, Action<ResourceManager.IconContent>? setIcon = null)
			: base($"#{fleetId}", $"Fleet{fleetId}", ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormFleet))
		{
			FleetId = fleetId;
			SetIcon = setIcon ?? (i => { });

			CopyCommand = new RelayCommand(ContextMenuFleet_CopyFleet_Click);
			CopyDeckBuilderCommand = new RelayCommand(ContextMenuFleet_CopyFleetDeckBuilder_Click);
			CopyKanmusuListCommand = new RelayCommand(ContextMenuFleet_CopyKanmusuList_Click);
			CopyFleetAnalysisCommand = new RelayCommand(ContextMenuFleet_CopyFleetAnalysis_Click);
			CopyFleetAnalysisEquipCommand = new RelayCommand(ContextMenuFleet_CopyFleetAnalysisLockedEquip_Click);
			CopyFleetAnalysisAllEquipCommand = new RelayCommand(ContextMenuFleet_CopyFleetAnalysisAllEquip_Click);

			AntiAirDetailsCommand = new RelayCommand(ContextMenuFleet_AntiAirDetails_Click);
			OutputFleetImageCommand = new RelayCommand(ContextMenuFleet_OutputFleetImage_Click);

			IncreaseBranchWeightCommand = new RelayCommand(() =>
			{
				BranchWeight++;

				if(BranchWeight > 4)
				{
					BranchWeight = 1;
				}
			});

			Timer = new(DispatcherPriority.Send)
			{
				Interval = new TimeSpan(0, 0, 0, 1),
				IsEnabled = true,
			};

			Timer.Tick += (sender, args) =>
			{
				switch (State)
				{
					/*
					case FleetStates.Damaged:
						if (Utility.Configuration.Config.FormFleet.BlinkAtDamaged)
							state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? Color.LightCoral : Color.Transparent;
						break;

					case FleetStates.SortieDamaged:
						state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? Color.LightCoral : Color.Transparent;
						break;
					*/
					case FleetStates.Docking:
						FleetStatusShortText = DateTimeHelper.ToTimeRemainString(StateTime);
						FleetStatusText = "On dock " + FleetStatusShortText;
						// UpdateText();
						// if (Utility.Configuration.Config.FormFleet.BlinkAtCompletion && (state.Timer - DateTime.Now).TotalMilliseconds <= Utility.Configuration.Config.NotifierRepair.AccelInterval)
							// state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? Color.LightGreen : Color.Transparent;

						break;

					case FleetStates.Expedition:
						FleetStatusShortText = DateTimeHelper.ToTimeRemainString(StateTime);
						FleetStatusText = FleetStatusText.Substring(0, 5) + DateTimeHelper.ToTimeRemainString(StateTime);
						// state.UpdateText();
						// if (Utility.Configuration.Config.FormFleet.BlinkAtCompletion && (state.Timer - DateTime.Now).TotalMilliseconds <= Utility.Configuration.Config.NotifierExpedition.AccelInterval)
							// state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? Color.LightGreen : Color.Transparent;
						break;

					case FleetStates.Tired:
						FleetStatusShortText = DateTimeHelper.ToTimeRemainString(StateTime);
						FleetStatusText = "Fatigued " + FleetStatusShortText;
						// state.UpdateText();
						// if (Utility.Configuration.Config.FormFleet.BlinkAtCompletion && (state.Timer - DateTime.Now).TotalMilliseconds <= 0)
							// state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? Color.LightGreen : Color.Transparent;
						break;

					case FleetStates.AnchorageRepairing:
						FleetStatusShortText = DateTimeHelper.ToTimeElapsedString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer);
						FleetStatusText = "Repairing " + FleetStatusShortText;
						// state.UpdateText();
						break;

				}

			};

			Timer.Start();

			SubscribeToApis();
		}

		private void SubscribeToApis()
		{
			APIObserver o = APIObserver.Instance;

			o["api_req_nyukyo/start"].RequestReceived += Updated;
			o["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o["api_req_hensei/change"].RequestReceived += Updated;
			o["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o["api_req_member/updatedeckname"].RequestReceived += Updated;
			o["api_req_kaisou/remodeling"].RequestReceived += Updated;
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
			o["api_req_kaisou/powerup"].ResponseReceived += Updated;        //requestのほうは面倒なのでこちらでまとめてやる
			o["api_get_member/deck"].ResponseReceived += Updated;
			o["api_get_member/slot_item"].ResponseReceived += Updated;
			o["api_req_map/start"].ResponseReceived += Updated;
			o["api_req_map/next"].ResponseReceived += Updated;
			o["api_get_member/ship_deck"].ResponseReceived += Updated;
			o["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;
			o["api_get_member/require_info"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_deprive"].ResponseReceived += Updated;
			o["api_req_kaisou/marriage"].ResponseReceived += Updated;
			o["api_req_map/anchorage_repair"].ResponseReceived += Updated;
		}

		private void Updated(string apiname, dynamic data)
		{
			KCDatabase db = KCDatabase.Instance;

			if (db.Ships.Count == 0) return;

			FleetData fleet = db.Fleet.Fleets[FleetId];
			if (fleet == null) return;

			Fleet = fleet;
			OnPropertyChanged(nameof(Fleet));
		}

		private FleetStates State { get; set; }
		private DateTime StateTime { get; set; }
		public string FleetStatusText { get; set; } = "";
		private string FleetStatusShortText { get; set; } = "";
		private ResourceManager.IconContent ImageIndex { get; set; } = ResourceManager.IconContent.Nothing;
		private Color BackColor { get; set; }
		public string? FleetStatusToolTip { get; set; }
		public ImageSource? FleetStatusImage => ImageSourceIcons.GetIcon(ImageIndex);

		private void SetInformation(FleetStates state, string text, string shortText, ResourceManager.IconContent imageIndex, 
			Color? backColor = null)
		{
			State = state;
			FleetStatusText = text;
			FleetStatusShortText = shortText;
			ImageIndex = imageIndex;
			SetIcon(imageIndex);
			BackColor = backColor ?? Colors.Transparent;
			IconSource = ImageSourceIcons.GetIcon(ImageIndex);
		}

		private void UpdateFleetState(IFleetData? fleet)
		{
			KCDatabase db = KCDatabase.Instance;

			int index = 0;


			bool emphasizesSubFleetInPort = Utility.Configuration.Config.FormFleet.EmphasizesSubFleetInPort &&
				(db.Fleet.CombinedFlag > 0 ? fleet.FleetID >= 3 : fleet.FleetID >= 2);
			FleetStateDisplayModes displayMode = (FleetStateDisplayModes) Utility.Configuration.Config.FormFleet.FleetStateDisplayMode;

			Color colorDanger = Colors.LightCoral;
			Color colorInPort = Colors.Transparent;


			//所属艦なし
			if (fleet == null || fleet.Members.All(id => id == -1))
			{
				// var state = GetStateLabel(index);

				SetInformation(FleetStates.NoShip, "No Ships Assigned", "", ResourceManager.IconContent.FleetNoShip);
				FleetStatusToolTip = null;

				emphasizesSubFleetInPort = false;
				return;

			}
			else
			{

				if (fleet.IsInSortie)
				{

					//大破出撃中
					if (fleet.MembersWithoutEscaped.Any(s => s != null && s.HPRate <= 0.25))
					{
						// var state = GetStateLabel(index);

						SetInformation(FleetStates.SortieDamaged, "！！Advancing at critical damage！！", "！！Advancing at critical damage！！", ResourceManager.IconContent.FleetSortieDamaged, colorDanger);
						FleetStatusToolTip = null;

						return;

					}
					else
					{   //出撃中
						// var state = GetStateLabel(index);

						SetInformation(FleetStates.Sortie, "On sortie", "", ResourceManager.IconContent.FleetSortie);
						FleetStatusToolTip = null;

						return;
					}

					emphasizesSubFleetInPort = false;
				}

				//遠征中
				if (fleet.ExpeditionState != 0)
				{
					// var state = GetStateLabel(index);
					var dest = db.Mission[fleet.ExpeditionDestination];
					string expeditionId = dest.DisplayID;
					string expeditionName = dest.NameEN;

					DateTime expeditionTime = fleet.ExpeditionTime;
					StateTime = expeditionTime;

					// state.Timer = fleet.ExpeditionTime;
					SetInformation(FleetStates.Expedition,
						$"[{expeditionId}] {DateTimeHelper.ToTimeRemainString(expeditionTime)}",
						DateTimeHelper.ToTimeRemainString(expeditionTime),
						ResourceManager.IconContent.FleetExpedition);

					FleetStatusToolTip = $"{expeditionId}: {expeditionName}\r\n" +
					                    $"ETA: {DateTimeHelper.TimeToCSVString(expeditionTime)}";

					emphasizesSubFleetInPort = false;
					return;
				}

				//大破艦あり
				if (!fleet.IsInSortie && fleet.MembersWithoutEscaped.Any(s => s != null && s.HPRate <= 0.25 && s.RepairingDockID == -1))
				{
					// var state = GetStateLabel(index);

					SetInformation(FleetStates.Damaged, "Critically damaged ship!", "Critically damaged ship!", ResourceManager.IconContent.FleetDamaged, colorDanger);
					FleetStatusToolTip = null;

					emphasizesSubFleetInPort = false;
					return;
				}

				//泊地修理中
				if (fleet.CanAnchorageRepair)
				{
					// var state = GetStateLabel(index);
					DateTime repairTimer = db.Fleet.AnchorageRepairingTimer;
					StateTime = repairTimer;

					SetInformation(FleetStates.AnchorageRepairing,
						"Repairing " + DateTimeHelper.ToTimeElapsedString(repairTimer),
						DateTimeHelper.ToTimeElapsedString(repairTimer),
						ResourceManager.IconContent.FleetAnchorageRepairing);


					StringBuilder sb = new StringBuilder();
					sb.AppendFormat("Start: {0}\r\nRepair time:\r\n",
						DateTimeHelper.TimeToCSVString(repairTimer));

					for (int i = 0; i < fleet.Members.Count; i++)
					{
						var ship = fleet.MembersInstance[i];
						if (ship != null && ship.HPRate < 1.0)
						{
							var totaltime = DateTimeHelper.FromAPITimeSpan(ship.RepairTime);
							var unittime = Calculator.CalculateDockingUnitTime(ship);
							sb.AppendFormat("#{0} : {1} @ {2} x -{3} HP\r\n",
								i + 1,
								DateTimeHelper.ToTimeRemainString(totaltime),
								DateTimeHelper.ToTimeRemainString(unittime),
								ship.HPMax - ship.HPCurrent
								);
						}
						else
						{
							sb.Append("#").Append(i + 1).Append(" : ----\r\n");
						}
					}

					FleetStatusToolTip = sb.ToString();

					emphasizesSubFleetInPort = false;
					return;
				}

				//入渠中
				{
					long ntime = db.Docks.Values.Where(d => d.State == 1 && fleet.Members.Contains(d.ShipID)).Select(d => d.CompletionTime.Ticks).DefaultIfEmpty().Max();

					if (ntime > 0)
					{   //入渠中
						// var state = GetStateLabel(index);

						DateTime dockTimer = new DateTime(ntime);
						StateTime = dockTimer;

						SetInformation(FleetStates.Docking,
							 "On dock " + DateTimeHelper.ToTimeRemainString(dockTimer),
							 DateTimeHelper.ToTimeRemainString(dockTimer),
							 ResourceManager.IconContent.FleetDocking);

						FleetStatusToolTip = "ETA : " + DateTimeHelper.TimeToCSVString(dockTimer);

						emphasizesSubFleetInPort = false;
						return;
					}

				}

				//未補給
				{
					var members = fleet.MembersInstance.Where(s => s != null);

					int fuel = members.Sum(ship => ship.SupplyFuel);
					int ammo = members.Sum(ship => ship.SupplyAmmo);
					int aircraft = members.SelectMany(s => s.MasterShip.Aircraft.Zip(s.Aircraft, (max, now) => max - now)).Sum();
					int bauxite = aircraft * 5;

					if (fuel > 0 || ammo > 0 || bauxite > 0)
					{
						// var state = GetStateLabel(index);

						SetInformation(FleetStates.NotReplenished, "Supply needed", "", ResourceManager.IconContent.FleetNotReplenished, colorInPort);
						FleetStatusToolTip = string.Format("Fuel: {0}\r\nAmmo: {1}\r\nBaux: {2} ({3} planes)", fuel, ammo, bauxite, aircraft);

						return;
					}
				}

				//疲労
				{
					int cond = fleet.MembersInstance.Min(s => s == null ? 100 : s.Condition);
					int conditionBorder = Utility.Configuration.Config.Control.ConditionBorder;
					double conditionBorderAccuracy = db.Fleet.ConditionBorderAccuracy;

					if (cond < conditionBorder && fleet.ConditionTime != null && fleet.ExpeditionState == 0)
					{
						// var state = GetStateLabel(index);

						ResourceManager.IconContent iconIndex;
						if (cond < 20)
							iconIndex = ResourceManager.IconContent.ConditionVeryTired;
						else if (cond < 30)
							iconIndex = ResourceManager.IconContent.ConditionTired;
						else
							iconIndex = ResourceManager.IconContent.ConditionLittleTired;

						DateTime conditionTimer = (DateTime)fleet.ConditionTime;
						StateTime = conditionTimer;

						SetInformation(FleetStates.Tired,
							"Fatigued " + DateTimeHelper.ToTimeRemainString(conditionTimer),
							DateTimeHelper.ToTimeRemainString(conditionTimer),
							iconIndex,
							colorInPort);

						FleetStatusToolTip = string.Format("Recovery time: {0}\r\n(prediction error: {1})",
							DateTimeHelper.TimeToCSVString(conditionTimer), DateTimeHelper.ToTimeRemainString(TimeSpan.FromSeconds(conditionBorderAccuracy)));

						return;

					}
					else if (cond >= 50)
					{       //戦意高揚
						// var state = GetStateLabel(index);

						SetInformation(FleetStates.Sparkled, "Sparkled fleet!", "", ResourceManager.IconContent.ConditionSparkle, colorInPort);
						FleetStatusToolTip = string.Format("Lowest morale: {0}\r\nEffective for {1} expeditions.", cond, Math.Ceiling((cond - 49) / 3.0));

						return;
					}

				}

				//出撃可能！
				if (index == 0)
				{
					// var state = GetStateLabel(index);

					SetInformation(FleetStates.Ready, "Ready to sortie!", "", ResourceManager.IconContent.FleetReady, colorInPort);
					FleetStatusToolTip = null;

					return;
				}

			}

			/*
			if (emphasizesSubFleetInPort)
			{
				for (int i = 0; i < index; i++)
				{
					if (StateLabels[i].Label.BackColor == Color.Transparent)
						StateLabels[i].Label.BackColor = Color.LightGreen;
				}
			}
			*/
			/*
			for (int i = displayMode == FleetStateDisplayModes.Single ? 1 : index; i < StateLabels.Count; i++)
				StateLabels[i].Enabled = false;
			*/
			/*
			switch (displayMode)
			{

				case FleetStateDisplayModes.AllCollapsed:
					for (int i = 0; i < index; i++)
						StateLabels[i].AutoShorten = true;
					break;

				case FleetStateDisplayModes.MultiCollapsed:
					if (index == 1)
					{
						StateLabels[0].AutoShorten = false;
					}
					else
					{
						for (int i = 0; i < index; i++)
							StateLabels[i].AutoShorten = true;
					}
					break;

				case FleetStateDisplayModes.Single:
				case FleetStateDisplayModes.AllExpanded:
					for (int i = 0; i < index; i++)
						StateLabels[i].AutoShorten = false;
					break;
			}
			*/
		}


		private string GetNameString(IFleetData? fleet)
		{
			if (fleet == null) return "";

			var members = fleet.MembersInstance.Where(s => s != null);

			int levelSum = members.Sum(s => s.Level);

			int fueltotal = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * (s.IsMarried ? 0.85 : 1.00)), 1));
			int ammototal = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * (s.IsMarried ? 0.85 : 1.00)), 1));

			int fuelunit = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));
			int ammounit = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));

			int speed = members.Select(s => s.Speed).DefaultIfEmpty(20).Min();

			string supporttype;
			switch (fleet.SupportType)
			{
				case 0:
				default:
					supporttype = "n/a"; break;
				case 1:
					supporttype = "Aerial Support"; break;
				case 2:
					supporttype = "Support Shelling"; break;
				case 3:
					supporttype = "Long-range Torpedo Attack"; break;
			}

			double expeditionBonus = Calculator.GetExpeditionBonus(fleet);
			int tp = Calculator.GetTPDamage(fleet);

			// 各艦ごとの ドラム缶 or 大発系 を搭載している個数
			var transport = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.TransportContainer));
			var landing = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.LandingCraft || eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank));


			return string.Format(
				"Lv sum: {0} / avg: {1:0.00}\r\n" +
				"{2} fleet\r\n" +
				"Support Expedition: {3}\r\n" +
				"Total FP {4} / Torp {5} / AA {6} / ASW {7} / LOS {8}\r\n" +
				"Drum: {9} ({10} ships)\r\n" +
				"Daihatsu: {11} ({12} ships, +{13:p1})\r\n" +
				"TP: S {14} / A {15}\r\n" +
				"Consumption: {16} fuel / {17} ammo\r\n" +
				"({18} fuel / {19} ammo per battle)",
				levelSum,
				(double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1),
				Constants.GetSpeed(speed),
				supporttype,
				members.Sum(s => s.FirepowerTotal),
				members.Sum(s => s.TorpedoTotal),
				members.Sum(s => s.AATotal),
				members.Sum(s => s.ASWTotal),
				members.Sum(s => s.LOSTotal),
				transport.Sum(),
				transport.Count(i => i > 0),
				landing.Sum(),
				landing.Count(i => i > 0),
				expeditionBonus,
				tp,
				(int)(tp * 0.7),
				fueltotal,
				ammototal,
				fuelunit,
				ammounit
			);
		}

		private string GetFleetAirString(IFleetData? fleet)
		{
			if (fleet == null) return "";

			int airSuperiority = fleet.GetAirSuperiority();
			bool includeLevel = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1;

			return string.Format(GeneralRes.ASTooltip,
					(int)(airSuperiority / 3.0),
					(int)(airSuperiority / 1.5),
					Math.Max((int)(airSuperiority * 1.5 - 1), 0),
					Math.Max((int)(airSuperiority * 3.0 - 1), 0),
					includeLevel ? "w/o Proficiency" : "w/ Proficiency",
					includeLevel ? Calculator.GetAirSuperiorityIgnoreLevel(fleet) : Calculator.GetAirSuperiority(fleet));
		}

		private string GetFleetLosString(IFleetData? fleet, int branchWeight)
		{
			if (fleet == null) return "";

			StringBuilder sb = new StringBuilder();
			double probStart = fleet.GetContactProbability();
			var probSelect = fleet.GetContactSelectionProbability();

			sb.AppendFormat("Formula 33 (n={0})\r\n　(Click to switch between weighting)\r\n\r\nContact:\r\n　AS+ {1:p1} / AS {2:p1}\r\n",
				branchWeight,
				probStart,
				probStart * 0.6);

			if (probSelect.Count > 0)
			{
				sb.AppendLine("Selection:");

				foreach (var p in probSelect.OrderBy(p => p.Key))
				{
					sb.AppendFormat("　Acc+{0}: {1:p1}\r\n", p.Key, p.Value);
				}
			}

			return sb.ToString();
		}

		private string GetFleetAaString(IFleetData? fleet)
		{
			if (fleet == null) return "";

			var sb = new StringBuilder();
			double lineahead = Calculator.GetAdjustedFleetAAValue(fleet, 1);

			sb.AppendFormat(GeneralRes.AntiAirPower,
				lineahead,
				Calculator.GetAdjustedFleetAAValue(fleet, 2),
				Calculator.GetAdjustedFleetAAValue(fleet, 3));

			return sb.ToString();
		}

		#region Commands
		
		private void ContextMenuFleet_CopyFleet_Click()
		{

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[FleetId];
			if (fleet == null) return;

			sb.AppendFormat("{0}\tAS: {1} / LOS: {2} / TP: {3}\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString(BranchWeight), Calculator.GetTPDamage(fleet));
			for (int i = 0; i < fleet.Members.Count; i++)
			{
				if (fleet[i] == -1)
					continue;

				ShipData ship = db.Ships[fleet[i]];

				sb.AppendFormat("{0}/Lv{1}\t", ship.MasterShip.NameEN, ship.Level);

				var eq = ship.AllSlotInstance;


				if (eq != null)
				{
					for (int j = 0; j < eq.Count; j++)
					{

						if (eq[j] == null) continue;

						int count = 1;
						for (int k = j + 1; k < eq.Count; k++)
						{
							if (eq[k] != null && eq[k].EquipmentID == eq[j].EquipmentID && eq[k].Level == eq[j].Level && eq[k].AircraftLevel == eq[j].AircraftLevel)
							{
								count++;
							}
							else
							{
								break;
							}
						}

						if (count == 1)
						{
							sb.AppendFormat("{0}{1}", j == 0 ? "" : ", ", eq[j].NameWithLevel);
						}
						else
						{
							sb.AppendFormat("{0}{1}x{2}", j == 0 ? "" : ", ", eq[j].NameWithLevel, count);
						}

						j += count - 1;
					}
				}

				sb.AppendLine();
			}


			Clipboard.SetText(sb.ToString());
		}

		/*
		private void ContextMenuFleet_Opening(object sender, CancelEventArgs e)
		{

			ContextMenuFleet_Capture.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;

		}
		*/

		/// <summary>
		/// 「艦隊デッキビルダー」用編成コピー
		/// <see cref="http://www.kancolle-calc.net/deckbuilder.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyFleetDeckBuilder_Click()
		{

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// 手書き json の悲しみ

			sb.Append(@"{""version"":4,");

			foreach (var fleet in db.Fleet.Fleets.Values)
			{
				if (fleet == null || fleet.MembersInstance.All(m => m == null)) continue;

				sb.AppendFormat(@"""f{0}"":{{", fleet.FleetID);

				int shipcount = 1;
				foreach (var ship in fleet.MembersInstance)
				{
					if (ship == null) break;

					sb.AppendFormat(@"""s{0}"":{{""id"":{1},""lv"":{2},""luck"":{3},""items"":{{",
						shipcount,
						ship.ShipID,
						ship.Level,
						ship.LuckBase);

					int eqcount = 1;
					foreach (var eq in ship.AllSlotInstance.Where(eq => eq != null))
					{
						if (eq == null) break;

						sb.AppendFormat(@"""i{0}"":{{""id"":{1},""rf"":{2},""mas"":{3}}},", eqcount >= 6 ? "x" : eqcount.ToString(), eq.EquipmentID, eq.Level, eq.AircraftLevel);

						eqcount++;
					}

					if (eqcount > 1)
						sb.Remove(sb.Length - 1, 1);        // remove ","
					sb.Append(@"}},");

					shipcount++;
				}

				if (shipcount > 0)
					sb.Remove(sb.Length - 1, 1);        // remove ","
				sb.Append(@"},");

			}

			sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append(@"}");

			Clipboard.SetText(sb.ToString());
		}

		/// <summary>
		/// 「艦隊晒しページ」用編成コピー
		/// <see cref="http://kancolle-calc.net/kanmusu_list.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyKanmusuList_Click()
		{

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// version
			sb.Append(".2");

			// <たね艦娘(完全未改造時)のID, 艦娘リスト>　に分類
			Dictionary<int, List<ShipData>> shiplist = new Dictionary<int, List<ShipData>>();

			foreach (var ship in db.Ships.Values.Where(s => s.IsLocked))
			{
				var master = ship.MasterShip;
				while (master.RemodelBeforeShip != null)
					master = master.RemodelBeforeShip;

				if (!shiplist.ContainsKey(master.ShipID))
				{
					shiplist.Add(master.ShipID, new List<ShipData>() { ship });
				}
				else
				{
					shiplist[master.ShipID].Add(ship);
				}
			}

			// 上で作った分類の各項を文字列化
			foreach (var sl in shiplist)
			{
				sb.Append("|").Append(sl.Key).Append(":");

				foreach (var ship in sl.Value.OrderByDescending(s => s.Level))
				{
					sb.Append(ship.Level);

					// 改造レベルに達しているのに未改造の艦は ".<たね=1, 改=2, 改二=3, ...>" を付加
					if (ship.MasterShip.RemodelAfterShipID != 0 && ship.ExpNextRemodel == 0)
					{
						sb.Append(".");
						int count = 1;
						var master = ship.MasterShip;
						while (master.RemodelBeforeShip != null)
						{
							master = master.RemodelBeforeShip;
							count++;
						}
						sb.Append(count);
					}
					sb.Append(",");
				}

				// 余った "," を削除
				sb.Remove(sb.Length - 1, 1);
			}

			Clipboard.SetText(sb.ToString());
		}

		/// <summary>
		/// 
		/// <see cref="https://kancolle-fleetanalysis.firebaseapp.com"/>
		/// </summary>
		private void ContextMenuFleet_CopyFleetAnalysis_Click()
		{
			KCDatabase db = KCDatabase.Instance;
			List<string> ships = new List<string>();

			foreach (ShipData ship in db.Ships.Values.Where(s => s.IsLocked))
			{
				int[] apiKyouka =
				{
					ship.FirepowerModernized,
					ship.TorpedoModernized,
					ship.AAModernized,
					ship.ArmorModernized,
					ship.LuckModernized,
					ship.HPMaxModernized,
					ship.ASWModernized
				};

				int expProgress = 0;
				if (ExpTable.ShipExp.ContainsKey(ship.Level + 1) && ship.Level != 99)
				{
					expProgress = (ExpTable.ShipExp[ship.Level].Next - ship.ExpNext)
						/ ExpTable.ShipExp[ship.Level].Next;
				}

				int[] apiExp = { ship.ExpTotal, ship.ExpNext, expProgress };

				string shipId = $"\"api_ship_id\":{ship.ShipID}";
				string level = $"\"api_lv\":{ship.Level}";
				string kyouka = $"\"api_kyouka\":[{string.Join(",", apiKyouka)}]";
				string exp = $"\"api_exp\":[{string.Join(",", apiExp)}]";
				// ship.SallyArea defaults to -1 if it doesn't exist on api 
				// which breaks the app, changing the default to 0 would be 
				// easier but I'd prefer not to mess with that
				string sallyArea = $"\"api_sally_area\":{(ship.SallyArea >= 0 ? ship.SallyArea : 0)}";

				string[] analysisData = { shipId, level, kyouka, exp, sallyArea };

				ships.Add($"{{{string.Join(",", analysisData)}}}");
			}

			string json = $"[{string.Join(",", ships)}]";

			Clipboard.SetText(json);
		}
		
		/// <summary>
		/// <see cref="https://kancolle-fleetanalysis.firebaseapp.com"/>
		/// </summary>
		private void ContextMenuFleet_CopyFleetAnalysisLockedEquip_Click()
		{
			GenerateEquipList(false);
		}

		private void ContextMenuFleet_CopyFleetAnalysisAllEquip_Click()
		{
			GenerateEquipList(true);
		}

		private void GenerateEquipList(bool allEquipment)
		{
			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// 手書き json の悲しみ
			// pain and suffering

			sb.Append("[");

			foreach (EquipmentData equip in db.Equipments.Values.Where(eq => allEquipment || eq.IsLocked))
			{
				sb.Append($"{{\"api_slotitem_id\":{equip.EquipmentID},\"api_level\":{equip.Level}}},");
			}

			sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append("]");

			Clipboard.SetText(sb.ToString());
		}

		private void ContextMenuFleet_AntiAirDetails_Click()
		{

			var dialog = new DialogAntiAirDefense();

			if (KCDatabase.Instance.Fleet.CombinedFlag != 0 && (FleetId == 1 || FleetId == 2))
				dialog.SetFleetID(5);
			else
				dialog.SetFleetID(FleetId);

			dialog.Show();
		}

		/*
		private void ContextMenuFleet_Capture_Click(object sender, EventArgs e)
		{

			using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
			{
				this.DrawToBitmap(bitmap, this.ClientRectangle);

				Clipboard.SetData(DataFormats.Bitmap, bitmap);
			}
		}
		*/


		private void ContextMenuFleet_OutputFleetImage_Click()
		{
			using (var dialog = new DialogFleetImageGenerator(FleetId))
			{
				dialog.ShowDialog();
			}
		}
		#endregion
	}
}