using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.Battle.ViewModels;

namespace ElectronicObserver.Window.Wpf.Battle.Views
{
    /// <summary>
    /// Interaction logic for HealthBarView.xaml
    /// </summary>
    public partial class HealthBarView : UserControl
    {
	    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		    "ViewModel", typeof(HealthBarViewModel), typeof(HealthBarView), new PropertyMetadata(default(HealthBarViewModel)));

	    public HealthBarViewModel ViewModel
	    {
		    get => (HealthBarViewModel) GetValue(ViewModelProperty);
		    set => SetValue(ViewModelProperty, value);
	    }

        public HealthBarView()
        {
            InitializeComponent();
        }
    }
}
