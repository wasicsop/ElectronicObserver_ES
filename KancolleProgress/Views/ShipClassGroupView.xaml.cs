using System.Windows;
using System.Windows.Controls;
using KancolleProgress.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace KancolleProgress.Views
{
	public sealed partial class ShipClassGroupView : UserControl
	{
		public static readonly DependencyProperty ShipClassGroupProperty = DependencyProperty.Register(
			nameof(ShipClassGroup), 
			typeof(ShipClassGroup), 
			typeof(ShipClassGroupView), 
			new PropertyMetadata(default(ShipClassGroup)));

		public ShipClassGroup ShipClassGroup

		{
			get => (ShipClassGroup) GetValue(ShipClassGroupProperty);
			set => SetValue(ShipClassGroupProperty, value);
		}

		public ShipClassGroupView()
		{
			this.InitializeComponent();
		}
	}
}
