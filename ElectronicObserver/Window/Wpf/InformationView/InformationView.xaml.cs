using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.InformationView;
/// <summary>
/// Interaction logic for InformationView.xaml
/// </summary>
public partial class InformationView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
	"ViewModel", typeof(InformationViewModel), typeof(InformationView), new PropertyMetadata(default(InformationViewModel)));

	public InformationViewModel ViewModel
	{
		get => (InformationViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public InformationView()
	{
		InitializeComponent();
	}
}
