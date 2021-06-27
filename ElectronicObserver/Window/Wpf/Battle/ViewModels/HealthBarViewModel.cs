using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.Battle.ViewModels
{
	public class HealthBarViewModel : ObservableObject
	{
		public string? Text { get; set; }
		public string? ToolTip { get; set; }
		public int Value { get; set; }
		public int PrevValue { get; set; }
		public int MaximumValue { get; set; }
		public System.Drawing.Color BackColor { get; set; }
		private System.Drawing.Color MainFontColor { get; set; }
		private System.Drawing.Color SubFontColor { get; set; }
		public bool Visible { get; set; }

		public int Health => Math.Max(0, Value);
		public string DamageTaken => (Value - PrevValue).ToString("+0;-0;-0");
		public SolidColorBrush MainFontBrush => MainFontColor.ToBrush();
		public SolidColorBrush SubFontBrush => SubFontColor.ToBrush();
		public SolidColorBrush HealthBarBrush => ProgressBarColor((double) Value / MaximumValue);

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

		public SolidColorBrush? BackgroundColor => BackColor.ToBrush();
		public Visibility Visibility => Visible switch
		{
			true => Visibility.Visible,
			_ => Visibility.Collapsed
		};

		public HealthBarViewModel()
		{
			PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName is not nameof(BackColor)) return;

				if (BackColor == System.Drawing.Color.Transparent)
				{
					MainFontColor = Utility.Configuration.Config.UI.ForeColor;
					SubFontColor = Utility.Configuration.Config.UI.SubForeColor;
				}
				else if (BackColor == Utility.Configuration.Config.UI.Battle_ColorHPBarsMVP)
				{
					MainFontColor = Utility.Configuration.Config.UI.Battle_ColorTextMVP;
					SubFontColor = Utility.Configuration.Config.UI.Battle_ColorTextMVP2;
				}
				else if (BackColor == Utility.Configuration.Config.UI.Battle_ColorHPBarsEscaped)
				{
					MainFontColor = Utility.Configuration.Config.UI.Battle_ColorTextEscaped;
					SubFontColor = Utility.Configuration.Config.UI.Battle_ColorTextEscaped2;
				}
				else if (BackColor == Utility.Configuration.Config.UI.Battle_ColorHPBarsBossDamaged)
				{
					MainFontColor = Utility.Configuration.Config.UI.Battle_ColorTextBossDamaged;
					SubFontColor = Utility.Configuration.Config.UI.Battle_ColorTextBossDamaged2;
				}
			};
		}
	}
}