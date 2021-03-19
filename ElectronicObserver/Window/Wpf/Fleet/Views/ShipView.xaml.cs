using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver.Window.Wpf.Fleet.Views
{
    /// <summary>
    /// Interaction logic for ShipView.xaml
    /// </summary>
    public partial class ShipView : UserControl
    {
	    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		    "ViewModel", typeof(ShipViewModel), typeof(ShipView), new PropertyMetadata(default(ShipViewModel)));

	    public ShipViewModel ViewModel
	    {
		    get { return (ShipViewModel) GetValue(ViewModelProperty); }
		    set { SetValue(ViewModelProperty, value); }
	    }

        public ShipView()
        {
            InitializeComponent();
        }
    }
}
