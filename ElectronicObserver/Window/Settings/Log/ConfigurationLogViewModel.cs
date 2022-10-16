using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.Log;

public record EncodingOption(Encoding Encoding, int Value)
{
	public string Display => Value switch
	{
		0 => "UTF-8",
		1 => "UTF-8(BOM)",
		2 => "UTF-16",
		3 => "UTF-16(BOM)",
		4 => "Shift-JIS",
	};
}

public class ConfigurationLogViewModel : ConfigurationViewModelBase
{
	public ConfigurationLogTranslationViewModel Translation { get; }

	private Configuration.ConfigurationData.ConfigLog Config { get; }

	public List<EncodingOption> Encodings { get; }
	public EncodingOption SelectedEncoding { get; set; }

	public int LogLevel { get; set; }

	public bool SaveLogFlag { get; set; }

	public bool SaveErrorReport { get; set; }

	public bool ShowSpoiler { get; set; }

	public double PlayTime { get; set; }

	public double PlayTimeIgnoreInterval { get; set; }

	public bool SaveBattleLog { get; set; }

	public bool SaveLogImmediately { get; set; }

	public ConfigurationLogViewModel(Configuration.ConfigurationData.ConfigLog config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationLogTranslationViewModel>();

		Encodings = new List<EncodingOption>
		{
			new(new UTF8Encoding(false), 0),
			new(new UTF8Encoding(true), 1),
			new(new UnicodeEncoding(false, false), 2),
			new(new UnicodeEncoding(false, true), 3),
			new(Encoding.GetEncoding(932), 4),
		};
		SelectedEncoding = Encodings.First();

		Config = config;
		Load(config);
	}

	private void Load(Configuration.ConfigurationData.ConfigLog config)
	{
		LogLevel = config.LogLevel;
		SaveLogFlag = config.SaveLogFlag;
		SaveErrorReport = config.SaveErrorReport;
		ShowSpoiler = config.ShowSpoiler;
		PlayTime = config.PlayTime;
		PlayTimeIgnoreInterval = config.PlayTimeIgnoreInterval;
		SaveBattleLog = config.SaveBattleLog;
		SaveLogImmediately = config.SaveLogImmediately;
		SelectedEncoding = Encodings
			.FirstOrDefault(e => e.Value == config.FileEncodingID)
			?? Encodings.First();
	}

	public override void Save()
	{
		Config.LogLevel = LogLevel;
		Config.SaveLogFlag = SaveLogFlag;
		Config.SaveErrorReport = SaveErrorReport;
		Config.FileEncodingID = SelectedEncoding.Value;
		Config.ShowSpoiler = ShowSpoiler;
		Config.PlayTime = PlayTime;
		Config.PlayTimeIgnoreInterval = PlayTimeIgnoreInterval;
		Config.SaveBattleLog = SaveBattleLog;
		Config.SaveLogImmediately = SaveLogImmediately;
	}
}
