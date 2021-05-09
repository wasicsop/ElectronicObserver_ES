using System;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels
{
	public class FleetStatusViewModel : ObservableObject
	{
		public FleetItemControlViewModel Name { get; }
		public FleetStateViewModel State { get; }
		public FleetItemControlViewModel AirSuperiority { get; }
		public FleetItemControlViewModel SearchingAbility { get; }
		public FleetItemControlViewModel AntiAirPower { get; }

		private int FleetId { get; }
		public int BranchWeight { get; private set; } = 1;

		public ICommand IncreaseBranchWeightCommand { get; }

		public FleetStatusViewModel(int fleetId)
		{
			FleetId = fleetId;

			IncreaseBranchWeightCommand = new RelayCommand(SearchingAbility_Click);

			Name = new()
			{
				// Text = "[" + parent.FleetID.ToString() + "]",
				// Anchor = AnchorStyles.Left,
				// ForeColor = parent.MainFontColor,
				// UseMnemonic = false,
				// Padding = new Padding(0, 1, 0, 1),
				// Margin = new Padding(2, 0, 2, 0),
				// AutoSize = true,
				// //Name.Visible = false;
				// Cursor = Cursors.Help
			};

			State = new()
			{
				// Anchor = AnchorStyles.Left,
				// ForeColor = parent.MainFontColor,
				// Padding = new Padding(),
				// Margin = new Padding(),
				// AutoSize = true
			};

			AirSuperiority = new()
			{
				// Anchor = AnchorStyles.Left,
				// ForeColor = parent.MainFontColor,
				// ImageList = ResourceManager.Instance.Equipments,
				ImageIndex = ResourceManager.EquipmentContent.CarrierBasedFighter,
				// Padding = new Padding(2, 2, 2, 2),
				// Margin = new Padding(2, 0, 2, 0),
				// AutoSize = true
			};

			SearchingAbility = new()
			{
				// Anchor = AnchorStyles.Left,
				// ForeColor = parent.MainFontColor,
				// ImageList = ResourceManager.Instance.Equipments,
				ImageIndex = ResourceManager.EquipmentContent.CarrierBasedRecon,
				// Padding = new Padding(2, 2, 2, 2),
				// Margin = new Padding(2, 0, 2, 0),
				// AutoSize = true
			};
			// SearchingAbility.Click += (sender, e) => SearchingAbility_Click(sender, e, parent.FleetID);

			AntiAirPower = new()
			{
				// Anchor = AnchorStyles.Left,
				// ForeColor = parent.MainFontColor,
				// ImageList = ResourceManager.Instance.Equipments,
				ImageIndex = ResourceManager.EquipmentContent.HighAngleGun,
				// Padding = new Padding(2, 2, 2, 2),
				// Margin = new Padding(2, 0, 2, 0),
				// AutoSize = true
			};

			ConfigurationChanged();
		}

		private void SearchingAbility_Click()
		{
			BranchWeight--;
			if (BranchWeight <= 0)
				BranchWeight = 4;

			Update(KCDatabase.Instance.Fleet[FleetId]);
		}

		public void Update(FleetData fleet)
		{
			KCDatabase db = KCDatabase.Instance;

			if (fleet == null) return;

			Name.Text = fleet.Name;
			{
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


				Name.ToolTip = string.Format(
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

			State.UpdateFleetState(fleet);


			//制空戦力計算
			{
				int airSuperiority = fleet.GetAirSuperiority();
				bool includeLevel = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1;
				AirSuperiority.Text = fleet.GetAirSuperiorityString();
				AirSuperiority.ToolTip = string.Format(GeneralRes.ASTooltip,
					(int)(airSuperiority / 3.0),
					(int)(airSuperiority / 1.5),
					Math.Max((int)(airSuperiority * 1.5 - 1), 0),
					Math.Max((int)(airSuperiority * 3.0 - 1), 0),
					includeLevel ? "w/o Proficiency" : "w/ Proficiency",
					includeLevel ? Calculator.GetAirSuperiorityIgnoreLevel(fleet) : Calculator.GetAirSuperiority(fleet));
			}


			//索敵能力計算
			SearchingAbility.Text = fleet.GetSearchingAbilityString(BranchWeight);
			{
				StringBuilder sb = new StringBuilder();
				double probStart = fleet.GetContactProbability();
				var probSelect = fleet.GetContactSelectionProbability();

				sb.AppendFormat("Formula 33 (n={0})\r\n　(Click to switch between weighting)\r\n\r\nContact:\r\n　AS+ {1:p1} / AS {2:p1}\r\n",
					BranchWeight,
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

				SearchingAbility.ToolTip = sb.ToString();
			}

			// 対空能力計算
			{
				var sb = new StringBuilder();
				double lineahead = Calculator.GetAdjustedFleetAAValue(fleet, 1);

				AntiAirPower.Text = lineahead.ToString("0.0");

				sb.AppendFormat(GeneralRes.AntiAirPower,
					lineahead,
					Calculator.GetAdjustedFleetAAValue(fleet, 2),
					Calculator.GetAdjustedFleetAAValue(fleet, 3));

				AntiAirPower.ToolTip = sb.ToString();
			}
		}


		public void Refresh()
		{
			State.RefreshFleetState();
		}

		public void ConfigurationChanged()
		{
			// Name.Font = parent.MainFont;
			// State.Font = parent.MainFont;
			//State.BackColor = Color.Transparent;
			State.RefreshFleetState();
			// AirSuperiority.Font = parent.MainFont;
			// SearchingAbility.Font = parent.MainFont;
			// AntiAirPower.Font = parent.MainFont;

			// ControlHelper.SetTableRowStyles(parent.TableFleet, ControlHelper.GetDefaultRowStyle());
		}
	}
}