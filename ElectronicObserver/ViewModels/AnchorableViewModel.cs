using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ElectronicObserver.ViewModels;

public partial class AnchorableViewModel : ObservableObject
{
	public string Title { get; set; }
	public virtual string ContentId { get; }
	public Visibility Visibility { get; set; } = Visibility.Collapsed;
	public bool IsSelected { get; set; }
	public bool IsActive { get; set; }
	public ImageSource? IconSource { get; set; }

	public bool CanFloat { get; set; }
	public bool CanClose { get; set; }

	protected AnchorableViewModel(string title, string contentId, ImageSource? icon = null)
	{
		Title = title;
		ContentId = contentId;
		IconSource = icon;
	}

	[ICommand(CanExecute = nameof(CanClose))]
	protected virtual void Close()
	{
		Visibility = Visibility.Collapsed;
	}
}
