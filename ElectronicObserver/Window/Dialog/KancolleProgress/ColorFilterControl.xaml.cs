using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ElectronicObserver.Window.Dialog.KancolleProgress
{
    /// <summary>
    /// Interaction logic for ColorFilterControl.xaml
    /// </summary>
    public partial class ColorFilterControl : UserControl
    {
        private ColorFilter _colorFilter;

        public ColorFilter ColorFilter
        {
            get => _colorFilter;
            set
            {
                _colorFilter = value;

                ColorFilterContainer.Children.Clear();

                DockPanel dockPanel = new DockPanel();
                SolidColorBrush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(ColorFilter.Color));

                dockPanel.Children.Add(new Label
                {
                    Content = $"{ColorFilter.Name}",
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Foreground = brush
                });

                dockPanel.Children.Add(new Label
                {
                    Content = $"{ColorFilter.MatchCount}",
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    Foreground = brush
                });

                ColorFilterContainer.Children.Add(dockPanel);
            }
        }

        public ColorFilterControl()
        {
            InitializeComponent();
        }
    }
}
