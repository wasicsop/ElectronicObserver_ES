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

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterShip
{
    /// <summary>
    /// Interaction logic for ImageTextControl.xaml
    /// </summary>
    public partial class ImageTextControl : UserControl
    {
	    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
		    nameof(Image), typeof(ImageSource), typeof(ImageTextControl), new PropertyMetadata(default(ImageSource)));

	    public ImageSource Image
	    {
		    get => (ImageSource)GetValue(ImageProperty);
		    set => SetValue(ImageProperty, value);
	    }

	    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
		    nameof(Text), typeof(string), typeof(ImageTextControl), new PropertyMetadata(default(string)));

	    public string Text
	    {
		    get => (string)GetValue(TextProperty);
		    set => SetValue(TextProperty, value);
	    }

        public ImageTextControl()
        {
            InitializeComponent();
        }
    }
}
