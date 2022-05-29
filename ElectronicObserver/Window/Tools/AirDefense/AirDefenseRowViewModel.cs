using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Tools.AirDefense;

public class AirDefenseRowViewModel : ObservableObject
{
	private AirDefenseViewModel ViewModel { get; }
	private int EnemyAircraftCount => ViewModel.EnemyAircraftCount;

	public string Name { get; }
	public int AaBase { get; }
	public double AdjustedAa { get; }
	public double ProportionalAa { get; }

	public string ProportionalAaDisplay => $"{ProportionalAa:p2}";
	public int FixedAa { get; }

	public int ShotDownBoth { get; }
	public string ShotDownBothDisplay => FormattedValue(ShotDownBoth, EnemyAircraftCount);
	public SolidColorBrush ShotDownBothBackground => Background(ShotDownBoth, EnemyAircraftCount);

	public int ShotDownProportional { get; }
	public string ShotDownProportionalDisplay => FormattedValue(ShotDownProportional, EnemyAircraftCount);
	public SolidColorBrush ShotDownProportionalBackground => Background(ShotDownProportional, EnemyAircraftCount);

	public int ShotDownFixed { get; }
	public string ShotDownFixedDisplay => FormattedValue(ShotDownFixed, EnemyAircraftCount);
	public SolidColorBrush ShotDownFixedBackground => Background(ShotDownFixed, EnemyAircraftCount);

	public int ShotDownFailed { get; }
	public string ShotDownFailedDisplay => FormattedValue(ShotDownFailed, EnemyAircraftCount);
	public SolidColorBrush ShotDownFailedBackground => Background(ShotDownFailed, EnemyAircraftCount);

	public double AarbProbability { get; }
	public string AarbProbabilityDisplay => $"{AarbProbability:p1}";

	private static string FormattedValue(int value, int enemySlot) =>
		$"{value} ({(double)value / enemySlot:p0})";

	private static SolidColorBrush Background(int value, int enemySlot) => (value >= enemySlot) switch
	{
		true => new SolidColorBrush(WipeColor),
		_ => new SolidColorBrush(Colors.Transparent)
	};

	private static Color WipeColor => Configuration.Config.UI.ThemeMode switch
	{
		0 => Colors.MistyRose,
		_ => Color.FromArgb(255, 255, 63, 63)
	};

	public AirDefenseRowViewModel(AirDefenseViewModel viewModel, string name, int aaBase, double adjustedAA, double proportionalAA, int fixedAA, int shotDownBoth, int shotDownProportional, int shotDownFixed, int shotDownFailed, double aarbProbability)
	{
		ViewModel = viewModel;

		Name = name;
		AaBase = aaBase;
		AdjustedAa = adjustedAA;
		ProportionalAa = proportionalAA;
		FixedAa = fixedAA;
		ShotDownBoth = shotDownBoth;
		ShotDownProportional = shotDownProportional;
		ShotDownFixed = shotDownFixed;
		ShotDownFailed = shotDownFailed;
		AarbProbability = aarbProbability;

		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.EnemyAircraftCount)) return;

			OnPropertyChanged(nameof(EnemyAircraftCount));
		};
	}
}
