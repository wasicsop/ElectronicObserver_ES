using System.Collections.Generic;
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
        private IEnumerable<IShipData> _group = new List<IShipData>();

        private string? _groupLabel;

        public string GroupLabel
        {
			get => _groupLabel ?? "";
			set
			{
				_groupLabel = value;
				ShipTypeGroupLabel.Content = value;
			}
        }

        public IEnumerable<IShipData> Group
        {
            get => _group;
            set
            {
                _group = value;

                ShipClassContainer.Children.Clear();

                var shipClassGroups = Group
					.GroupBy(s => s.MasterShip.ShipClass);

				foreach (var shipClassGroup in shipClassGroups)
                {
                    ShipClassContainer.Children.Add(new ShipClassGroupControl
                    {
	                    ClassGroup = shipClassGroup
                    });
                }
            }
        }

        public ShipTypeGroupControl()
        {
            InitializeComponent();
        }
    }
}
