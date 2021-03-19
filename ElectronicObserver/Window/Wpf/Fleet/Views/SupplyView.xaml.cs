using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver.Window.Wpf.Fleet.Views
{
    /// <summary>
    /// Interaction logic for SupplyView.xaml
    /// </summary>
    public partial class SupplyView : UserControl
    {
	    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		    "ViewModel", typeof(ShipViewModel), typeof(SupplyView), new PropertyMetadata(default(ShipViewModel)));

	    public ShipViewModel ViewModel
	    {
		    get { return (ShipViewModel) GetValue(ViewModelProperty); }
		    set { SetValue(ViewModelProperty, value); }
	    }

        public SupplyView()
        {
            InitializeComponent();
        }
    }
}
