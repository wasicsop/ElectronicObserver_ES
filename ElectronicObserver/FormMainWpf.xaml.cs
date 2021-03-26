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
		public FormMainViewModel ViewModel { get; } = new();

		public FormMainWpf()
		{
			InitializeComponent();
		}
	}
}
