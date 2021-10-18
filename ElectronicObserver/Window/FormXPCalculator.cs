using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronicObserver;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window;

public partial class FormXPCalculator : WeifenLuo.WinFormsUI.Docking.DockContent
{
	public static int[] ExpTable = new int[] { 0, 0, 100, 300, 600, 1000, 1500, 2100, 2800, 3600, 4500, 5500, 6600, 7800, 9100, 10500, 12000, 13600, 15300, 17100, 19000,
		21000, 23100, 25300, 27600, 30000, 32500, 35100, 37800, 40600, 43500, 46500, 49600, 52800, 56100, 59500, 63000, 66600, 70300, 74100, 78000,
		82000, 86100, 90300, 94600, 99000, 103500, 108100, 112800, 117600, 122500, 127500, 132700, 138100, 143700, 149500, 155500, 161700, 168100, 174700, 181500,
		188500, 195800, 203400, 211300, 219500, 228000, 236800, 245900, 255300, 265000, 275000, 285400, 296200, 307400, 319000, 331000, 343400, 356200, 369400, 383000,
		397000, 411500, 426500, 442000, 458000, 474500, 491500, 509000, 527000, 545500, 564500, 584500, 606500, 631500, 661500, 701500, 761500, 851500, 1000000, 1000000,
		1010000, 1011000, 1013000, 1016000, 1020000, 1025000, 1031000, 1038000, 1046000, 1055000, 1065000, 1077000, 1091000, 1107000, 1125000, 1145000, 1168000, 1194000, 1223000, 1255000,
		1290000, 1329000, 1372000, 1419000, 1470000, 1525000, 1584000, 1647000, 1714000, 1785000, 1860000, 1940000, 2025000, 2115000, 2210000, 2310000, 2415000, 2525000, 2640000, 2760000,
		2887000, 3021000, 3162000, 3310000, 3465000, 3628000, 3799000, 3978000, 4165000, 4360000, 4564000, 4777000, 4999000, 5230000, 5470000 };

	public static Dictionary<string, int> SortieExpTable = new Dictionary<string, int>
	{
		{"1-1", 30}, {"1-2", 50}, {"1-3", 80}, {"1-4", 100}, {"1-5", 150},
		{"2-1", 120}, {"2-2", 150}, {"2-3", 200},{"2-4", 300}, {"2-5", 250},
		{"3-1", 310}, {"3-2", 320}, {"3-3", 330}, {"3-4", 350}, {"3-5", 400},
		{"4-1", 310}, {"4-2", 320}, {"4-3", 330}, {"4-4", 340},
		{"5-1", 360}, {"5-2", 380}, {"5-3", 400}, {"5-4", 420}, {"5-5", 450},
		{"6-1", 400}, {"6-2", 420},
	};

	public Font MainFont { get; set; }
	public Font SubFont { get; set; }

	public FormXPCalculator(FormMain parent)
	{
		InitializeComponent();
		ConfigurationChanged();
		selectMap.SelectedIndex = 0;
		selectResult.SelectedIndex = 0;
		selectStartLevel.SelectedIndex = 0;

		this.Load += FormXPCalculator_Load;
		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	public void FormXPCalculator_Load(object sender, EventArgs e)
	{
		APIObserver o = APIObserver.Instance;
		o.APIList["api_port/port"].ResponseReceived += ShipsUpdated;
	}

	public void ShipsUpdated(string apiname, dynamic data)
	{
		selectShip.Items.Clear();
		var shipsByLevel = from element in KCDatabase.Instance.Ships.Values orderby element.Level descending select element;
		foreach (var ship in shipsByLevel)
		{
			selectShip.Items.Add(ship);
		}
	}

	private void selectShip_SelectedIndexChanged(object sender, EventArgs e)
	{
		ShipData ship = selectShip.SelectedItem as ShipData;
		selectStartLevel.SelectedIndex = ship.Level - 1;
		selectEndLevel.SelectedIndex = ship.Level >= 155 ? 154 : ship.Level;
		PropertyUpdated();
	}

	private void PropertyUpdated()
	{
		double sortieXP = SortieExpTable[(string)selectMap.SelectedItem];
		int start, end;
		double diff, sorties;

		switch (selectResult.Text)
		{
			case "S":
				sortieXP *= 1.2;
				break;
			case "C":
				sortieXP *= 0.8;
				break;
			case "D":
				sortieXP *= 0.7;
				break;
			case "E":
				sortieXP *= 0.5;
				break;
			default:
				break;
		}
		if (checkFlagship.Checked) sortieXP *= 1.5;
		if (checkMVP.Checked) sortieXP *= 2;

		battleXP.Text = sortieXP.ToString();
		ShipData ship = selectShip.SelectedItem as ShipData;
		if (ship != null)
		{
			if ((selectStartLevel.SelectedIndex + 1) == ship.Level)
			{
				startXP.Text = ship.ExpTotal.ToString();
				start = ship.ExpTotal;
			}
			else
			{
				startXP.Text = ExpTable[selectStartLevel.SelectedIndex + 1].ToString();
				start = ExpTable[selectStartLevel.SelectedIndex + 1];
			}
		}
		else
		{
			startXP.Text = ExpTable[selectStartLevel.SelectedIndex + 1].ToString();
			start = ExpTable[selectStartLevel.SelectedIndex + 1];
		}
		endXP.Text = ExpTable[selectEndLevel.SelectedIndex + 1].ToString();
		end = ExpTable[selectEndLevel.SelectedIndex + 1];
		diff = end - start;
		if (diff < 0) return;
		remainXP.Text = diff.ToString();
		sorties = diff / sortieXP;
		double roundedSorties = Math.Ceiling(sorties);
		battleCount.Text = roundedSorties.ToString();
	}

	private void selectStartLevel_SelectedIndexChanged(object sender, EventArgs e)
	{
		PropertyUpdated();
	}

	private void selectEndLevel_SelectedIndexChanged(object sender, EventArgs e)
	{
		PropertyUpdated();
	}

	private void selectMap_SelectedIndexChanged(object sender, EventArgs e)
	{
		PropertyUpdated();
	}

	private void selectResult_SelectedIndexChanged(object sender, EventArgs e)
	{
		PropertyUpdated();
	}

	private void checkFlagship_CheckedChanged(object sender, EventArgs e)
	{
		PropertyUpdated();
	}

	private void checkMVP_CheckedChanged(object sender, EventArgs e)
	{
		PropertyUpdated();
	}

	private void ConfigurationChanged()
	{
		var c = Utility.Configuration.Config;

		MainFont = Font = c.UI.MainFont;
		SubFont = c.UI.SubFont;
	}
}
