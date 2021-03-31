using System.Linq;
using System.Threading;
using System.Windows;
using ElectronicObserver.Window;

namespace ElectronicObserver
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			bool allowMultiInstance = e.Args.Contains("-m") || e.Args.Contains("--multi-instance");


			using (var mutex = new Mutex(false, Application.ResourceAssembly.Location.Replace('\\', '/'),
				out var created))
			{

				/*
				bool hasHandle = false;

				try
				{
					hasHandle = mutex.WaitOne(0, false);
				}
				catch (AbandonedMutexException)
				{
					hasHandle = true;
				}
				*/

				if (!created && !allowMultiInstance)
				{
					// 多重起動禁止
					MessageBox.Show(
						"Electronic Observer already started.\r\nIn case of false positive, start using option -m via commandline.",
						Utility.SoftwareInformation.SoftwareNameEnglish,
						MessageBoxButton.OK,
						MessageBoxImage.Exclamation
					);
					return;
				}

				System.Windows.Forms.Application.EnableVisualStyles();
				System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

#if false // change to false if you want to test in native wpf

				try
				{
					// todo why does this exception happen?
					// observed first after I added the wpf version of KC progress
					System.Windows.Forms.Application.Run(new FormMain());
				}
				catch (System.Runtime.InteropServices.SEHException ex)
				{

				}
#else
				new FormMainWpf().ShowDialog();
#endif
			}
		}
	}
}
