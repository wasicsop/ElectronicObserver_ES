using System.Collections.Generic;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Settings.SubWindow.Arsenal;
using ElectronicObserver.Window.Settings.SubWindow.Dock;
using ElectronicObserver.Window.Settings.SubWindow.Fleet;

namespace ElectronicObserver.Window.Settings.SubWindow;

public class ConfigurationSubWindowViewModel : ConfigurationViewModelBase
{
	public ConfigurationSubWindowTranslationViewModel Translation { get; }

	private Configuration.ConfigurationData Config { get; }

	public ConfigurationFleetViewModel Fleet { get; }
	public ConfigurationArsenalViewModel Arsenal { get; }
	public ConfigurationDockViewModel Dock { get; }

	private IEnumerable<ConfigurationViewModelBase> Configurations()
	{
		yield return Fleet;
		yield return Arsenal;
		yield return Dock;
	}

	public ConfigurationSubWindowViewModel(Configuration.ConfigurationData config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationSubWindowTranslationViewModel>();

		Config = config;

		Fleet = new(Config.FormFleet);
		Arsenal = new(Config.FormArsenal);
		Dock = new(Config.FormDock);
	}

	public override void Save()
	{
		foreach (ConfigurationViewModelBase configuration in Configurations())
		{
			configuration.Save();
		}
	}
}
