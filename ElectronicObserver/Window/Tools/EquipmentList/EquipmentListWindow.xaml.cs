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
using System.Windows.Shapes;

namespace ElectronicObserver.Window.Tools.EquipmentList;
/// <summary>
/// Interaction logic for EquipmentListWindow.xaml
/// </summary>
public partial class EquipmentListWindow
{
	public EquipmentListWindow() : base(new EquipmentListViewModel())
	{
		InitializeComponent();
	}

	private void OpenEquipmentEncyclopedia(object sender, MouseButtonEventArgs e)
	{
		if (sender is not DataGridRow { DataContext: EquipmentListRow { Id: { } id } }) return;

		ViewModel.OpenEquipmentEncyclopedia(id);
	}
}
