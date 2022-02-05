using System.Windows.Forms;
namespace ElectronicObserver.ViewModels;

public static class DialogExtensions
{
	public static void ShowDialogExt(this System.Windows.Window window, FormMainViewModel formMainViewModel)
	{
		window.Owner = formMainViewModel.Window;
		window.ShowDialog();
	}
	public static DialogResult ShowDialogExt(this System.Windows.Forms.Form form, FormMainViewModel formMainViewModel)
	{
		DialogResult result;
		bool topmost = formMainViewModel.Topmost;
		if (formMainViewModel.Topmost)
		{
			formMainViewModel.Topmost = false;
			result = form.ShowDialog();
			formMainViewModel.Topmost = topmost;
			return result;
		}
		else
		{
			result = form.ShowDialog();
			return result;
		}

	}
}
