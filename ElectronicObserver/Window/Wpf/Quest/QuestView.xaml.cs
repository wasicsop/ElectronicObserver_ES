using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.Quest;
/// <summary>
/// Interaction logic for QuestView.xaml
/// </summary>
public partial class QuestView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		nameof(ViewModel), typeof(QuestViewModel), typeof(QuestView), new PropertyMetadata(default(QuestViewModel)));

	public QuestViewModel ViewModel
	{
		get => (QuestViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public QuestView()
	{
		InitializeComponent();

		DataGrid.Sorting += (sender, args) =>
		{
			// dispatcher is needed to get the SortDescriptions value after sorting
			// without the dispatcher you get the old value before the sorting
			Dispatcher.BeginInvoke(() => ViewModel.SortDescriptions = DataGrid.Items.SortDescriptions.ToList());
		};
	}


}
