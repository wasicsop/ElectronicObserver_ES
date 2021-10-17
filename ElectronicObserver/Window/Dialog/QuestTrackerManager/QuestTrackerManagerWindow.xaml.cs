using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

/// <summary>
/// Interaction logic for QuestTrackerManagerWindow.xaml
/// </summary>
public partial class QuestTrackerManagerWindow : System.Windows.Window
{
	public QuestTrackerManagerViewModel ViewModel { get; }

	public QuestTrackerManagerWindow()
	{
		InitializeComponent();

		ViewModel = KCDatabase.Instance.QuestTrackerManagers;
		DataContext = ViewModel;
	}
}