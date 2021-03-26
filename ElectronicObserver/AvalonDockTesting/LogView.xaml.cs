using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.AvalonDockTesting
{
	/// <summary>
	/// Interaction logic for LogView.xaml
	/// </summary>
	public partial class LogView : UserControl
	{
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
			"ViewModel", typeof(LogViewModel), typeof(LogView), new PropertyMetadata(default(LogViewModel)));

		public LogViewModel ViewModel
		{
			get => (LogViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		public LogView()
		{
			InitializeComponent();
		}
	}
}
