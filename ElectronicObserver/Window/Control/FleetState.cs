namespace ElectronicObserver.Window.Control;

/// <summary>
/// 艦隊の状態を表します。
/// </summary>
public enum FleetStates
{
	NoShip,
	SortieDamaged,
	Sortie,
	Expedition,
	Damaged,
	AnchorageRepairing,
	Docking,
	NotReplenished,
	Tired,
	Sparkled,
	Ready,
}

/// <summary>
/// 状態の表示モードを指定します。
/// </summary>
public enum FleetStateDisplayModes
{
	/// <summary> 1つだけ表示 </summary>
	Single,

	/// <summary> 複数表示(すべて短縮表示) </summary>
	AllCollapsed,

	/// <summary> 複数表示(1つの時は通常表示、複数の時は短縮表示) </summary>
	MultiCollapsed,

	/// <summary> 複数表示(すべて通常表示) </summary>
	AllExpanded,
}
