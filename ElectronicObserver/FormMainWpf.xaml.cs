using System.Windows;
using ElectronicObserver.ViewModels;

namespace ElectronicObserver;

/// <summary>
/// Interaction logic for FormMainWpf.xaml
/// </summary>
public partial class FormMainWpf : System.Windows.Window
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(FormMainViewModel), typeof(FormMainWpf), new PropertyMetadata(default(FormMainViewModel)));

	public FormMainViewModel ViewModel
	{
		get => (FormMainViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public FormMainWpf()
	{
		InitializeComponent();

		ViewModel = new(DockingManager, this);

		Loaded += (sender, _) => ViewModel.LoadLayout(sender);
		Closed += (sender, _) => ViewModel.SaveLayout(sender);
	}
}
