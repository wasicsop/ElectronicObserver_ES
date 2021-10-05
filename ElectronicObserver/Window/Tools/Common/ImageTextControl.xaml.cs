using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ElectronicObserver.Window.Tools.Common
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
