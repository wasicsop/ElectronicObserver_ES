using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Dialog;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PropertyChanged;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels
{
	public class ShipViewModel: ObservableObject
	{
		private IShipData? _ship;

		[DoNotNotify]
		public IShipData? Ship
		{
			get => _ship;
			set
			{
				_ship = value;
				OnPropertyChanged(nameof(ShipVisibility));
				if (Ship == null) return;

				Name = Ship.Name;
				NameWithLevel = Ship.NameWithLevel;
				Level = Ship.Level;
				ExpNext = Ship.ExpNext;
				HPCurrent = Ship.HPCurrent;
				HPMax = Ship.HPMax;
				HPRate = Ship.HPRate;
				Condition = Ship.Condition;
				Fuel = Ship.Fuel;
				FuelMax = Ship.FuelMax;
				FuelRate = Ship.FuelRate;
				Ammo = Ship.Ammo;
				AmmoMax = Ship.AmmoMax;
				AmmoRate = Ship.AmmoRate;

				ConditionIcon = Condition switch
				{
					< 20 => ResourceManager.IconContent.ConditionVeryTired,
					< 30 => ResourceManager.IconContent.ConditionTired,
					< 40 => ResourceManager.IconContent.ConditionLittleTired,
					< 50 => ResourceManager.IconContent.ConditionNormal,
					_ => ResourceManager.IconContent.ConditionSparkle
				};

				NameWidth = Utility.Configuration.Config.FormFleet.FixShipNameWidth switch
				{
					true => NameWidths[0],
					_ => NameWidths[1],
				};

				ConditionImageVisibility = Utility.Configuration.Config.FormFleet.ShowConditionIcon switch
				{
					true => Visibility.Visible,
					_ => Visibility.Collapsed
				};
				if (Utility.Configuration.Config.FormFleet.ShowConditionIcon)
				{
					ConditionBackgroundColor = ConditionBackgroundColors[3];
					ConditionForegroundColor = ConditionForegroundColors[0];
				}
				else
				{
					(ConditionBackgroundColor, ConditionForegroundColor) = Condition switch
					{
						< 20 => (ConditionBackgroundColors[0], ConditionForegroundColors[1]),
						< 30 => (ConditionBackgroundColors[1], ConditionForegroundColors[1]),
						< 40 => (ConditionBackgroundColors[2], ConditionForegroundColors[1]),
						< 50 => (ConditionBackgroundColors[3], ConditionForegroundColors[0]),
						_ => (ConditionBackgroundColors[4], ConditionForegroundColors[1])
					};
				}

				var slots = Ship.AllSlotInstance
					.Zip(Ship.Aircraft, (eq, s) => (Equipment: eq, CurrentAircraft: s))
					.Zip(Ship.MasterShip.Aircraft, (slot, t) => (slot.Equipment, slot.CurrentAircraft, Size: t))
					.ToList();

				for (int i = 0; i < Slots.Count; i++)
				{
					if (i < Ship.SlotSize)
					{
						Slots[i].Equipment = slots[i].Equipment;
						Slots[i].CurrentAircraft = slots[i].CurrentAircraft;
						Slots[i].Size = slots[i].Size;
						Slots[i].SlotVisibility = Visibility.Visible;
					}
					else
					{
						Slots[i].SlotVisibility = Visibility.Collapsed;
					}
				}

				if (Ship.IsExpansionSlotAvailable)
				{
					Slots[^1].Equipment = Ship.ExpansionSlotInstance;
					Slots[^1].SlotVisibility = Visibility.Visible;
				}

				OnPropertyChanged(nameof(Ship));
				OnPropertyChanged(nameof(NameToolTip));
				OnPropertyChanged(nameof(LevelToolTip));
				OnPropertyChanged(nameof(HpToolTip));
				OnPropertyChanged(nameof(ConditionToolTip));
				OnPropertyChanged(nameof(SupplyToolTip));
				OnPropertyChanged(nameof(EquipmentToolTip));
			}
		}

		public Visibility ShipVisibility => Ship switch
		{
			null => Visibility.Collapsed,
			_ => Visibility.Visible
		};

		public IFleetData? Fleet { get; set; }

		public string Name { get; set; } = "";
		public string NameWithLevel { get; set; } = "";
		public int Level { get; set; }
		public int ExpNext { get; set; }
		public int HPCurrent { get; set; }
		public int HPMax { get; set; }
		public double HPRate { get; set; }
		public int Condition { get; set; }
		public int Fuel { get; set; }
		public int FuelMax { get; set; }
		public double FuelRate { get; set; }
		public int Ammo { get; set; }
		public int AmmoMax { get; set; }
		public double AmmoRate { get; set; }

		public string HPString => $"{HPCurrent} / {HPMax}";
		public SolidColorBrush HealthBarBrush => ProgressBarColor(HPRate);
		public SolidColorBrush FuelBarBrush => ProgressBarColor(FuelRate);
		public SolidColorBrush AmmoBarBrush => ProgressBarColor(AmmoRate);

		public List<ShipSlotViewModel> Slots { get; set; } = new()
		{
			new(),
			new(),
			new(),
			new(),
			new(),
			new(),
		};

		public string NameToolTip => GetNameString(Ship);
		public string LevelToolTip => GetLevelString(Ship);
		public string HpToolTip => GetHpString(Ship);
		public string ConditionToolTip => GetConditionString(Ship);
		public string SupplyToolTip => GetSupplyString(Ship);
		public string EquipmentToolTip => GetEquipmentString(Ship, Fleet);

		private List<SolidColorBrush> ConditionBackgroundColors { get; } = new()
		{
			new(Colors.LightCoral),
			new(Colors.LightSalmon),
			new(Colors.Moccasin),
			new(Colors.Transparent),
			new(Colors.LightGreen),
		};

		private static Color WinformsColorToWpfColor(System.Drawing.Color color) =>
			Color.FromRgb(color.R, color.G, color.B);

		private List<SolidColorBrush> ConditionForegroundColors { get; } = new()
		{
			new(WinformsColorToWpfColor(Utility.Configuration.Config.UI.ForeColor)),
			new(Colors.Black),
		};

		public SolidColorBrush ConditionForegroundColor { get; set; }
		public SolidColorBrush ConditionBackgroundColor { get; set; }

		private List<SolidColorBrush> ProgressBarColors { get; } = new()
		{
			new(Color.FromRgb(38, 139, 210)),
			new(Color.FromRgb(70, 144, 70)),
			new(Color.FromRgb(214, 141, 0)),
			new(Color.FromRgb(201, 72, 0)),
			new(Color.FromRgb(199, 16, 21)),
			new(Color.FromRgb(55, 59, 65)),
		};

		private SolidColorBrush ProgressBarColor(double rate) => rate switch
		{
			1 => ProgressBarColors[0],
			> .75 => ProgressBarColors[1],
			> .5 => ProgressBarColors[2],
			> .25 => ProgressBarColors[3],
			> 0 => ProgressBarColors[4],
			_ => ProgressBarColors[5],
		};

		private List<GridLength> NameWidths = new()
		{
			new(Utility.Configuration.Config.FormFleet.FixedShipNameWidth),
			GridLength.Auto
		};

		public GridLength NameWidth { get; set; }

		public Visibility ConditionImageVisibility { get; set; }

		public ImageSource? ConditionImage => ImageSourceIcons.GetIcon(ConditionIcon);

		private ResourceManager.IconContent ConditionIcon { get; set; } = ResourceManager.IconContent.ConditionNormal;

		/*
		public static void SetConditionDesign(ImageLabel label, int cond)
		{

			if (label.ImageAlign == ContentAlignment.MiddleCenter)
			{
				// icon invisible
				label.ImageIndex = -1;

				(label.BackColor, label.ForeColor) = cond switch
				{
					{ } when cond < 20 => (Color.LightCoral, Color.Black),
					{ } when cond < 30 => (Color.LightSalmon, Color.Black),
					{ } when cond < 40 => (Color.Moccasin, Color.Black),
					{ } when cond < 50 => (Color.Transparent, Utility.Configuration.Config.UI.ForeColor),
					_ =>                  (Color.LightGreen, Color.Black)
				};
			}
			else
			{
				label.BackColor = Color.Transparent;
				label.ForeColor = Utility.Configuration.Config.UI.ForeColor;

				label.ImageIndex =
					cond < 20 ? (int)ResourceManager.IconContent.ConditionVeryTired :
					cond < 30 ? (int)ResourceManager.IconContent.ConditionTired :
					cond < 40 ? (int)ResourceManager.IconContent.ConditionLittleTired :
					cond < 50 ? (int)ResourceManager.IconContent.ConditionNormal :
					(int)ResourceManager.IconContent.ConditionSparkle;

			}
		}
		*/
		public IEnumerable<string> DayAttacks => ShipDayAttacks.GetDayAttacks(Ship).Select(e => e.Display());

		public IRelayCommand ShipNameRightClick { get; }
		public IRelayCommand ShipLevelRightClick { get; }
		
		public ShipViewModel()
		{
			ShipNameRightClick = new RelayCommand(() => new DialogAlbumMasterShip(Ship.MasterShip.ShipID).Show());
			ShipLevelRightClick = new RelayCommand(() => new DialogExpChecker(Ship.MasterID).Show());
		}

		private string GetNameString(IShipData? ship)
		{
			if (ship == null) return "";

			var equipments = ship.AllSlotInstance.Where(eq => eq != null);

			return string.Format(
				"{0}{1} {2}\r\nFP: {3}/{4}\r\nTorp: {5}/{6}\r\nAA: {7}/{8}\r\nArmor: {9}/{10}\r\nASW: {11}/{12}\r\nEvasion: {13}/{14}\r\nLOS: {15}/{16}\r\nLuck: {17}\r\nAccuracy: {18:+#;-#;+0}\r\nBombing: {19:+#;-#;+0}\r\nRange: {20} / Speed: {21}\r\n(right click to open encyclopedia)\n",
				ship.SallyArea > 0 ? $"[{ship.SallyArea}] " : "",
				ship.MasterShip.ShipTypeName, ship.NameWithLevel,
				ship.FirepowerBase, ship.FirepowerTotal,
				ship.TorpedoBase, ship.TorpedoTotal,
				ship.AABase, ship.AATotal,
				ship.ArmorBase, ship.ArmorTotal,
				ship.ASWBase, ship.ASWTotal,
				ship.EvasionBase, ship.EvasionTotal,
				ship.LOSBase, ship.LOSTotal,
				ship.LuckTotal,
				equipments.Sum(eq => eq.MasterEquipment.Accuracy),
				equipments.Sum(eq => eq.MasterEquipment.Bomber),
				Constants.GetRange(ship.Range),
				Constants.GetSpeed(ship.Speed)
			);
		}

		private string GetLevelString(IShipData? ship)
		{
			if (ship == null) return "";

			StringBuilder tip = new StringBuilder();
			tip.AppendFormat("Total: {0:N0} exp.\r\n", ship.ExpTotal);

			bool showNextExp = !Utility.Configuration.Config.FormFleet.ShowNextExp;

			if (showNextExp)
				tip.AppendFormat(GeneralRes.ToNextLevel + " exp.\r\n", ship.ExpNext.ToString("N0"));

			var remodels = KCDatabase.Instance.MasterShips.Values
				.Where(s => s.BaseShip() == ship.MasterShip.BaseShip())
				.Where(s => s.RemodelTier > ship.MasterShip.RemodelTier)
				.OrderBy(s => s.RemodelBeforeShip?.RemodelAfterLevel ?? 0)
				.Select(s => (s.NameEN, s.RemodelBeforeShip?.RemodelAfterLevel ?? 0));

			foreach ((var name, int remodelLevel) in remodels)
			{
				int neededExp = Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, remodelLevel), 0);
				tip.Append($"{name}({remodelLevel}): {neededExp:N0} exp.\r\n");
			}

			if (ship.Level < 99)
			{
				string lv99Exp = Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, 99), 0).ToString("N0");
				tip.AppendFormat(GeneralRes.To99 + " exp.\r\n", lv99Exp);
			}

			if (ship.Level < ExpTable.ShipMaximumLevel)
			{
				string lv175Exp = Math
					.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, ExpTable.ShipMaximumLevel), 0)
					.ToString("N0");
				tip.AppendFormat(GeneralRes.ToX + " exp.\r\n", ExpTable.ShipMaximumLevel, lv175Exp);
			}


			tip.AppendLine("(right click to calculate exp)");

			return tip.ToString();
		}

		private bool IsEscaped => Fleet?.EscapedShipList.Contains(Ship.MasterID) ?? false;

		private string GetHpString(IShipData? ship)
		{
			if (ship == null) return "";

			StringBuilder sb = new StringBuilder();
			double hprate = (double)ship.HPCurrent / ship.HPMax;

			sb.AppendFormat("HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState(hprate));
			if (IsEscaped)
			{
				sb.AppendLine(GeneralRes.Retreating);
			}
			else if (hprate > 0.50)
			{
				sb.AppendFormat(GeneralRes.ToMidAndHeavy + "\n", ship.HPCurrent - ship.HPMax / 2, ship.HPCurrent - ship.HPMax / 4);
			}
			else if (hprate > 0.25)
			{
				sb.AppendFormat(GeneralRes.ToHeavy + "\n", ship.HPCurrent - ship.HPMax / 4);
			}
			else
			{
				sb.AppendLine(GeneralRes.IsTaiha);
			}

			if (ship.RepairTime > 0)
			{
				var span = DateTimeHelper.FromAPITimeSpan(ship.RepairTime);
				sb.AppendFormat(GeneralRes.DockTime + ": {0} @ {1}",
					DateTimeHelper.ToTimeRemainString(span),
					DateTimeHelper.ToTimeRemainString(Calculator.CalculateDockingUnitTime(ship)));
			}

			return sb.ToString();
		}

		private string GetConditionString(IShipData? ship)
		{
			if (ship == null) return "";

			if (ship.Condition < 49)
			{
				TimeSpan ts = new TimeSpan(0, (int)Math.Ceiling((49 - ship.Condition) / 3.0) * 3, 0);
				return string.Format(GeneralRes.FatigueRestoreTime, (int)ts.TotalMinutes, (int)ts.Seconds);
			}
			else
			{
				return  string.Format(GeneralRes.RemainingExpeds, (int)Math.Ceiling((ship.Condition - 49) / 3.0));
			}
		}

		private string GetSupplyString(IShipData? ship)
		{
			if (ship == null) return "";

			return string.Format("Fuel: {0}/{1} ({2}%)\r\nAmmo: {3}/{4} ({5}%)",
				Fuel, FuelMax, (int)Math.Ceiling(100.0 * Fuel / FuelMax),
				Ammo, AmmoMax, (int)Math.Ceiling(100.0 * Ammo / AmmoMax));
		}

		private string GetEquipmentString(IShipData? ship, IFleetData? fleet)
		{
			if (ship == null) return "";
			if (fleet == null) return "";

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat(ship.NameWithLevel + "\r\n");

			for (int i = 0; i < ship.Slot.Count; i++)
			{
				var eq = ship.SlotInstance[i];
				if (eq != null)
					sb.AppendFormat("[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], eq.NameWithLevel);
			}

			{
				var exslot = ship.ExpansionSlotInstance;
				if (exslot != null)
					sb.AppendFormat(GeneralRes.Expansion + ": {0}\r\n", exslot.NameWithLevel);
			}

			int[] slotmaster = ship.AllSlotMaster.ToArray();


			EngagementType engagement = (EngagementType)Utility.Configuration.Config.Control.PowerEngagementForm;

			List<Enum> dayAttacks = ShipDayAttacks.GetDayAttacks(ship).ToList();

			if (dayAttacks.Any())
			{
				sb.AppendFormat($"\r\n{GeneralRes.DayBattle}:");
				List<double> asRates = DayAttackRate.TotalRates(dayAttacks.Select(a => ship.GetDayAttackRate(a, fleet, AirState.Superiority))
						.ToList());
				List<double> asPlusRates =
					DayAttackRate.TotalRates(dayAttacks.Select(a => ship.GetDayAttackRate(a, fleet)).ToList());

				foreach ((Enum attack, double asRate, double asPlusRate) in dayAttacks
					.Zip(asRates, (attack, rate) => (attack, rate))
					.Zip(asPlusRates, (ar, asPlus) => (ar.attack, ar.rate, asPlus)))
				{
					double power = DayAttackPower.GetDayAttackPower(ship, attack, fleet, engagement);
					double accuracy = DayAttackAccuracy.GetDayAttackAccuracy(ship, attack, fleet);
					string attackDisplay = attack switch
					{
						DayAttackKind dayAttack => Constants.GetDayAttackKind(dayAttack),
						DayAirAttackCutinKind cvci => cvci switch
						{
							DayAirAttackCutinKind.FighterBomberAttacker => "CI (FBA)",
							DayAirAttackCutinKind.BomberBomberAttacker => "CI (BBA)",
							DayAirAttackCutinKind.BomberAttacker => "CI (BA)",
							_ => "?"
						},
						_ => $"{attack}"
					};
					sb.AppendFormat($"\r\n・[{asRate:P1} | {asPlusRate:P1}] - {attackDisplay} - Power: {power} - Accuracy: {accuracy:0.##}");
				}
			}

			List<Enum> nightAttacks = ShipNightAttacks.GetNightAttacks(ship).ToList();
			List<double> nightAttackRates = DayAttackRate.TotalRates(nightAttacks.Select(a => ship.GetNightAttackRate(a, fleet))
					.ToList());

			if (nightAttacks.Any())
			{
				sb.AppendFormat($"\r\n{GeneralRes.NightBattle}:");

				foreach ((Enum attack, double rate) in nightAttacks.Zip(nightAttackRates, (attack, rate) => (attack, rate)))
				{
					double power = NightAttackPower.GetNightAttackPower(ship, attack, fleet);
					double accuracy = NightAttackAccuracy.GetNightAttackAccuracy(ship, attack, fleet);
					string attackDisplay = attack switch
					{
						NightAttackKind nightAttack => Constants.GetNightAttackKind(nightAttack),
						CvnciKind cvnci => cvnci switch
						{
							CvnciKind.FighterFighterAttacker => "CI (FFA)",
							CvnciKind.FighterAttacker => "CI (FA)",
							CvnciKind.Phototube => "CI (Photo)",
							CvnciKind.FighterOtherOther => "CI (FOO)",
							_ => "?"
						},
						NightTorpedoCutinKind torpedoCutin => torpedoCutin switch
						{
							NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment => "CI (Submarine Radar)",
							NightTorpedoCutinKind.LateModelTorpedo2 => "CI (Late Model)",
							_ => "?"
						},
						_ => $"{attack}"
					};

					sb.AppendFormat($"\r\n・[{rate:P1}] - {attackDisplay} - Power: {power} - Accuracy: {accuracy:0.##}");
				}
			}

			sb.AppendLine();
			
			{
				int torpedo = ship.TorpedoPower;
				int asw = ship.AntiSubmarinePower;

				DayAttackKind aswAttack = Calculator.GetDayAttackKind(ship.AllSlotMaster.ToArray(), ship.ShipID, 126, false);
				int asw2 = AswAttackPower.GetAswAttackPower(ship, aswAttack, fleet);

				if (torpedo > 0)
				{
					sb.AppendFormat(ConstantsRes.TorpedoAttack + ": Power: {0}", torpedo);
					sb.Append($" - Accuracy: {TorpedoAttackAccuracy.GetTorpedoAttackAccuracy(ship, fleet):0.##}");
				}
				if (asw > 0)
				{
					if (torpedo > 0)
						sb.AppendLine();

					sb.AppendFormat("ASW: Power: {0}", asw2);

					if (ship.CanOpeningASW)
						sb.Append(" (OASW)");

					sb.Append($" - Accuracy: {AswAttackAccuracy.GetAswAttackAccuracy(ship, fleet):0.##}");
				}
				if (torpedo > 0 || asw > 0)
					sb.AppendLine();
			}

			{
				int aacutin = Calculator.GetAACutinKind(ship.ShipID, slotmaster);
				if (aacutin != 0)
				{
					sb.AppendFormat("AA: {0}\r\n", Constants.GetAACutinKind(aacutin));
				}
				double adjustedaa = Calculator.GetAdjustedAAValue(ship);
				sb.AppendFormat(GeneralRes.ShipAADefense + "\r\n",
					adjustedaa,
					Calculator.GetProportionalAirDefense(adjustedaa, -1)
					);

				double aarbRate = Calculator.GetAarbRate(ship, adjustedaa);
				if (aarbRate > 0)
				{
					sb.Append($"AARB: {aarbRate:p1}\r\n");
				}

			}

			{
				int airsup_min = 0;
				int airsup_max = 0;
				int airMethod = 1;
				airMethod = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod;
				if (airMethod == 1)
				{
					airsup_min = Calculator.GetAirSuperiority(ship, false);
					airsup_max = Calculator.GetAirSuperiority(ship, true);
				}
				else
				{
					airsup_min = airsup_max = Calculator.GetAirSuperiorityIgnoreLevel(ship);
				}

				int airbattle = ship.AirBattlePower;
				if (airsup_min > 0)
				{

					string airsup_str;
					bool showAirRange = Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange;
					if (showAirRange && airsup_min < airsup_max)
					{
						airsup_str = string.Format("{0} ～ {1}", airsup_min, airsup_max);
					}
					else
					{
						airsup_str = airsup_min.ToString();
					}

					if (airbattle > 0)
						sb.AppendFormat(GeneralRes.AirPower + ": {0} / Airstrike Power: {1}\r\n", airsup_str, airbattle);
					else
						sb.AppendFormat(GeneralRes.AirPower + ": {0}\r\n", airsup_str);
				}
				else if (airbattle > 0)
					sb.AppendFormat("Airstrike Power: {0}\r\n", airbattle);
			}

			return sb.ToString();
		}

	}
}