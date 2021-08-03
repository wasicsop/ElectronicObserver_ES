using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.ViewModels
{
	public class StripStatusViewModel : ObservableObject
	{
		public string Information { get; set; } = "";
		public string Clock { get; set; } = "";
		public string? ClockToolTip { get; set; }
		public bool Visible { get; set; }

		public Visibility Visibility => Visible switch
		{
			true => Visibility.Visible,
			_ => Visibility.Collapsed
		};

	}
}