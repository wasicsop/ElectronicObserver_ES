using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.BrowserHost
{
	/// <summary>
	/// Interaction logic for BrowserHostView.xaml
	/// </summary>
	public partial class BrowserHostView : UserControl
	{
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
			"ViewModel", typeof(BrowserHostViewModel), typeof(BrowserHostView), new PropertyMetadata(default(BrowserHostViewModel)));

		public BrowserHostViewModel ViewModel
		{
			get => (BrowserHostViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		public BrowserHostView()
		{
			InitializeComponent();
		}
	}
}
