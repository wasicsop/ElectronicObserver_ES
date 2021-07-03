using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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

				// hack: needed for running the winforms version
				// remove this and the Shutdown call when moving to wpf only
				ShutdownMode = ShutdownMode.OnExplicitShutdown;

				bool forceWpf = false;
#if DEBUG
				forceWpf = true;
#endif

				AppCenter.Start("7fdbafa0-058a-4691-b317-a700be513b95", typeof(Analytics), typeof(Crashes));

				if (forceWpf || e.Args.Contains("-wpf"))
				{
					ToolTipService.ShowDurationProperty.OverrideMetadata(
						typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));
					new FormMainWpf().ShowDialog();
				}
				else
				{
					try
					{
						// todo why does this exception happen?
						// observed first after I added the wpf version of KC progress
						System.Windows.Forms.Application.Run(new FormMain());
					}
					catch (System.Runtime.InteropServices.SEHException ex)
					{

					}
				}

				Shutdown();
			}
		}
	}
}
