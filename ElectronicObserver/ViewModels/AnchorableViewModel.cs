using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.ViewModels
{
	public class AnchorableViewModel : ObservableObject
	{
		public string Title { get; }
		public string ContentId { get; }
		public bool IsVisible { get; set; } = true;
		public bool IsSelected { get; set; }
		public bool IsActive { get; set; }
		public ImageSource? IconSource { get; }

		public ICommand CloseCommand { get; }

		protected AnchorableViewModel(string title, ImageSource? icon = null)
		{
			Title = title;
			ContentId = title;
			IconSource = icon;
			CloseCommand = new RelayCommand(() => IsVisible = false);
		}
	}
}