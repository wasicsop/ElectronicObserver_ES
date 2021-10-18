using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ElectronicObserver.ReplayJSON;

public class Fleet1
{

	[JsonProperty("mst_id")]
	public int MstId { get; set; }

	[JsonProperty("level")]
	public int Level { get; set; }

	[JsonProperty("kyouka")]
	public List<int> Kyouka { get; set; }

	[JsonProperty("morale")]
	public int Morale { get; set; }

	[JsonProperty("equip")]
	public List<int> Equip { get; set; }

}

public class Fleet2
{

	[JsonProperty("mst_id")]
	public int MstId { get; set; }

	[JsonProperty("level")]
	public int Level { get; set; }

	[JsonProperty("kyouka")]
	public List<int> Kyouka { get; set; }

	[JsonProperty("morale")]
	public int Morale { get; set; }

	[JsonProperty("equip")]
	public List<int> Equip { get; set; }


}

public class Fleet3
{

	[JsonProperty("mst_id")]
	public int MstId { get; set; }

	[JsonProperty("level")]
	public int Level { get; set; }

	[JsonProperty("kyouka")]
	public List<int> Kyouka { get; set; }

	[JsonProperty("morale")]
	public int Morale { get; set; }

	[JsonProperty("equip")]
	public List<int> Equip { get; set; }


}

public class Fleet4
{

	[JsonProperty("mst_id")]
	public int MstId { get; set; }

	[JsonProperty("level")]
	public int Level { get; set; }

	[JsonProperty("kyouka")]
	public List<int> Kyouka { get; set; }

	[JsonProperty("morale")]
	public int Morale { get; set; }

	[JsonProperty("equip")]
	public List<int> Equip { get; set; }


}

public class Range
{

	[JsonProperty("api_base")]
	public int ApiBase { get; set; }

	[JsonProperty("api_bonus")]
	public int ApiBonus { get; set; }
}

public class Plane
{

	[JsonProperty("mst_id")]
	public int MstId { get; set; }

	[JsonProperty("count")]
	public int Count { get; set; }

	[JsonProperty("stars")]
	public int Stars { get; set; }

	[JsonProperty("ace")]
	public int Ace { get; set; }

	[JsonProperty("state")]
	public int State { get; set; }

	[JsonProperty("morale")]
	public int Morale { get; set; }
}

public class Lba
{

	[JsonProperty("rid")]
	public int Rid { get; set; }

	[JsonProperty("range")]
	public Range Range { get; set; }

	[JsonProperty("action")]
	public int Action { get; set; }

	[JsonProperty("planes")]
	public List<Plane> Planes { get; set; }
}

public class Battle
{

	[JsonProperty("sortie_id")]
	public int SortieId { get; set; }

	[JsonProperty("node")]
	public int Node { get; set; }

	[JsonProperty("data")]
	public object Data { get; set; }

	[JsonProperty("yasen")]
	public object Yasen { get; set; }

	[JsonProperty("rating")]
	public string Rating { get; set; }

	[JsonProperty("drop")]
	public int Drop { get; set; }

	[JsonProperty("time")]
	public double Time { get; set; }

	[JsonProperty("baseEXP")]
	public int BaseEXP { get; set; }

	[JsonProperty("hqEXP")]
	public int HqEXP { get; set; }

	[JsonProperty("mvp")]
	public List<int> Mvp { get; set; }

	[JsonProperty("id")]
	public int Id { get; set; }
}

public class ReplayRoot
{

	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("now_maphp")]
	public int Now_maphp { get; set; }

	[JsonProperty("max_maphp")]
	public int Max_maphp { get; set; }

	[JsonProperty("defeat_count")]
	public int Defeat_count { get; set; }

	[JsonProperty("world")]
	public int World { get; set; }

	[JsonProperty("mapnum")]
	public int Mapnum { get; set; }

	[JsonProperty("fleetnum")]
	public int Fleetnum { get; set; }

	[JsonProperty("combined")]
	public int Combined { get; set; }

	[JsonProperty("fleet1")]
	public List<Fleet1> Fleet1 { get; set; }

	[JsonProperty("fleet2")]
	public List<Fleet2> Fleet2 { get; set; }

	[JsonProperty("fleet3")]
	public List<Fleet3> Fleet3 { get; set; }

	[JsonProperty("fleet4")]
	public List<Fleet4> Fleet4 { get; set; }

	[JsonProperty("support1")]
	public int Support1 { get; set; }

	[JsonProperty("support2")]
	public int Support2 { get; set; }

	[JsonProperty("lbas")]
	public List<Lba> Lbas { get; set; }

	[JsonProperty("time")]
	public long Time { get; set; }

	[JsonProperty("battles")]
	public List<Battle> Battles { get; set; }

}
