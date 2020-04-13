using System.Collections.Generic;
using System.Windows.Controls;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Dialog.KancolleProgress
{
    /// <summary>
    /// Interaction logic for ColorFilterContainerControl.xaml
    /// </summary>
    public partial class ColorFilterContainerControl : UserControl
    {
        private List<IShipData> _ships;

        List<ColorFilter> ColorFilters => new List<ColorFilter>
        {
            new ColorFilter{Level = 175, Name = "Max", Ships = Ships},
            new ColorFilter{Level = 99, Name = "99+", Ships = Ships},
            new ColorFilter{Level = 90, Name = "90+", Ships = Ships},
            new ColorFilter{Level = 1, Name = "Collection", Ships = Ships},
            new ColorFilter{Level = 0, Name = "Missing", Ships = Ships},
        };

        public List<IShipData> Ships
        {
            get => _ships;
            set
            {
                _ships = value;

                ColorFilterContainer.Children.Clear();

                foreach (ColorFilter colorFilter in ColorFilters)
                {
                    ColorFilterContainer.Children.Add(new ColorFilterControl { ColorFilter = colorFilter });
                }
            } 

        }

        public ColorFilterContainerControl()
        {
            InitializeComponent();
        }
    }
}
