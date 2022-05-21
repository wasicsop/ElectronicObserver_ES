using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
