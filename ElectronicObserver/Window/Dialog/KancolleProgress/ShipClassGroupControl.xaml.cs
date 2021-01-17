using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.KancolleProgress
{
    /// <summary>
    /// Interaction logic for ShipClassGroupControl.xaml
    /// </summary>
    public partial class ShipClassGroupControl : UserControl
    {
        private IGrouping<int, IShipData>? _classGroup;

        public IGrouping<int, IShipData> ClassGroup
        {
            get => _classGroup!;
            set
            {
                _classGroup = value;

                ShipClassContainer.Children.Clear();

                foreach (IShipData ship in ClassGroup)
                {
                    DockPanel dockPanel = new DockPanel();

                    SolidColorBrush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(ColorFilter.LevelColor(ship.Level)));

                    dockPanel.Children.Add(new Label
                    {
                        Content = $"{ship.Name}",
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Foreground = brush
                    });

                    dockPanel.Children.Add(new Label
                    {
                        Content = $"{ship.Level}",
                        HorizontalContentAlignment = HorizontalAlignment.Right,
                        Foreground = brush
                    });

                    ShipClassContainer.Children.Add(dockPanel);
                }
            }
        }

        public ShipClassGroupControl()
        {
            InitializeComponent();
        }
    }
}
