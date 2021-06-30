using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ElectronicObserver.ViewModels;

namespace ElectronicObserver
{
	/// <summary>
	/// Interaction logic for FormMainWpf.xaml
	/// </summary>
	public partial class FormMainWpf : System.Windows.Window
	{
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
			"ViewModel", typeof(FormMainViewModel), typeof(FormMainWpf), new PropertyMetadata(default(FormMainViewModel)));

		public FormMainViewModel ViewModel
		{
			get => (FormMainViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		public FormMainWpf()
		{
			InitializeComponent();

			ViewModel = new(DockingManager, this);

			Loaded += (_, _) => ViewModel.LoadLayout();
			Closed += (_, _) => ViewModel.SaveLayout();
		}
	}
}
