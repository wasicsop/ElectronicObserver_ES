namespace ElectronicObserver.Window.Control;

/// <summary>
/// 装備改修レベル・艦載機熟練度の表示フラグ
/// </summary>
public enum LevelVisibilityFlag
{

	/// <summary> 非表示 </summary>
	Invisible,

	/// <summary> 改修レベルのみ </summary>
	LevelOnly,

	/// <summary> 艦載機熟練度のみ </summary>
	AircraftLevelOnly,

	/// <summary> 改修レベル優先 </summary>
	LevelPriority,

	/// <summary> 艦載機熟練度優先 </summary>
	AircraftLevelPriority,

	/// <summary> 両方表示 </summary>
	Both,

	/// <summary> 両方表示(艦載機熟練度はアイコンにオーバーレイする) </summary>
	AircraftLevelOverlay,
}
