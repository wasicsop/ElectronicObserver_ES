using System.Linq;
using System.Windows.Controls;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.KancolleProgress
{
    /// <summary>
    /// Interaction logic for ShipTypeGroupControl.xaml
    /// </summary>
    public partial class ShipTypeGroupControl : UserControl
    {
        private IGrouping<ShipTypeGroup, IShipData>? _group;

        public IGrouping<ShipTypeGroup, IShipData> Group
        {
            get => _group!;
            set
            {
                _group = value;


                ShipTypeGroupLabel.Content = Group.Key.Display();

                ShipClassContainer.Children.Clear();

                var shipClassGroups = Group
					// .GroupBy(s => s.ShipClass);
					.GroupBy(s => s.MasterShip.ShipClass);

				foreach (var shipClassGroup in shipClassGroups)
                {
                    /*StackPanel s = new StackPanel();

                    foreach (ShipDataCustom ship in shipClassGroup)
                    {
                        s.Children.Add(new Label
                        {
                            Content = $"{ship.Name} {ship.Level}"
                        });
                    }*/

                    ShipClassContainer.Children.Add(new ShipClassGroupControl{ClassGroup = shipClassGroup });
                }
            }
        }

        public ShipTypeGroupControl()
        {
            InitializeComponent();
        }
    }
}
