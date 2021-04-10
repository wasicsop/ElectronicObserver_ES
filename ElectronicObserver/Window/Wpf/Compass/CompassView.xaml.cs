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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicObserver.Window.Wpf.Compass
{
    /// <summary>
    /// Interaction logic for CompassView.xaml
    /// </summary>
    public partial class CompassView : UserControl
    {
	    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		    "ViewModel", typeof(CompassViewModel), typeof(CompassView), new PropertyMetadata(default(CompassViewModel)));

	    public CompassViewModel ViewModel
	    {
		    get => (CompassViewModel) GetValue(ViewModelProperty);
		    set => SetValue(ViewModelProperty, value);
	    }

        public CompassView()
        {
            InitializeComponent();
        }
    }
}
