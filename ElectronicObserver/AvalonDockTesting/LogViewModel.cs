using System.Collections.ObjectModel;
using ElectronicObserver.ViewModels;

namespace ElectronicObserver.AvalonDockTesting
{
	public class LogViewModel : AnchorableViewModel
	{
		public ObservableCollection<string> Logs { get; } = new(new()
		{
			"Log 1",
			"Log 2",
			"Log 3",
			"Log 4",
			"Log 5",
			"Log 6",
		});

		public LogViewModel() : base("Log")
		{
			
		}
	}
}