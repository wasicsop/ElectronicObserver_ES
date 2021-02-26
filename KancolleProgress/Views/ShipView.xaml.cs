using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KancolleProgress.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace KancolleProgress.Views
{
	public sealed partial class ShipView : UserControl
	{
		public static readonly DependencyProperty ShipProperty = DependencyProperty.Register(
			"Ship", 
			typeof(ShipViewModel), 
			typeof(ShipView), 
			new PropertyMetadata(default(ShipViewModel)));

		public ShipViewModel Ship
		{
			get => (ShipViewModel) GetValue(ShipProperty);
			set => SetValue(ShipProperty, value);
		}
		
		public ShipView()
		{
			this.InitializeComponent();
		}
	}
}
