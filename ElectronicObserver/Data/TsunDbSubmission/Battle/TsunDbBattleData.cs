using System;
using Newtonsoft.Json;

namespace ElectronicObserver.Data;

public class TsunDbBattleData : TsunDbEntity
{
	protected override string Url => "battle";

	protected override bool IsBetaAPI => true;

	private static KCDatabase Database => KCDatabase.Instance;

	#region Json properties
	/// <summary>
	/// Map name, e.g. 53-5
	/// </summary>
	[JsonProperty("map")]
	public string Map { get; private set; }

	/// <summary>
	/// Node edge number
	/// </summary>
	[JsonProperty("node")]
	public int Node { get; private set; }

	/// <summary>
	/// Chosen map difficulty
	/// </summary>
	[JsonProperty("difficulty")]
	public int Difficulty { get; private set; }

	/// <summary>
	/// Checks if the node is debuffed
	/// </summary>
	[JsonProperty("debuffed")]
	public bool Debuffed { get; private set; }

	/// <summary>
	/// Checks if the map is cleared
	/// </summary>
	[JsonProperty("cleared")]
	public bool MapCleared { get; private set; }

	/// <summary>
	/// Contains information about the sortied fleet(s), LBAS and support
	/// </summary>
	[JsonProperty("fleet")]
	public TsunDbFleetsAndAirBaseData Fleet { get; private set; }

	/// <summary>
	/// Checks if resupply has been used in the battle, obtained from request.params.api_supply_flag
	/// </summary>
	[JsonProperty("resupplyused")]
	public bool ResupplyUsed { get; private set; }

	/// <summary>
	/// Formation ID chosen by the player, obtained from request.params.api_formation
	/// </summary>
	[JsonProperty("playerformation")]
	public int Formation { get; private set; }

	/// <summary>
	/// Raw battle data
	/// </summary>
	[JsonProperty("rawapi")]
	public string RawApi { get; private set; }

	/// <summary>
	/// Amount of edges in the map
	/// </summary>
	[JsonProperty("amountofnodes")]
	public int AmountOfNodes { get; private set; }

	/// <summary>
	/// API endpoint for the battle, e.g. api_req_sortie/battle
	/// </summary>
	[JsonProperty("apiname")]
	public string ApiName { get; private set; }
	#endregion

	#region Constructor
	public TsunDbBattleData(string apiName, string rawApi)
	{
		Map = string.Format("{0}-{1}", Database.Battle.Compass.MapAreaID, Database.Battle.Compass.MapInfoID);
		Node = Database.Battle.Compass.Destination;
		Difficulty = Database.Battle.Compass.MapInfo.EventDifficulty;

		// Start day or night, its always the first battle that should carry debuff info ?
		Debuffed = Database.Battle.FirstBattle.Initial.IsBossDamaged;
		MapCleared = Database.Battle.Compass.MapInfo.IsCleared;

		Fleet = new TsunDbFleetsAndAirBaseData();

		ResupplyUsed = Database.Battle.ResupplyUsed;
		Formation = Database.Battle.FirstBattle.Searching.FormationFriend;
		AmountOfNodes = TsunDbSubmissionManager.CurrentMapAmountOfNodes;

		RawApi = rawApi;
		ApiName = apiName;
	}
	#endregion
}
