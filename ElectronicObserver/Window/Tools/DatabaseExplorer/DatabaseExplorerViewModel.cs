using ElectronicObserver.Common;
using ElectronicObserver.Window.Tools.DatabaseExplorer.ApiFile;
using ElectronicObserver.Services.ApiFileService;

namespace ElectronicObserver.Window.Tools.DatabaseExplorer;

public partial class DatabaseExplorerViewModel : WindowViewModelBase
{
	public ApiFileExplorerViewModel ApiFileExplorer { get; } = new();
}
