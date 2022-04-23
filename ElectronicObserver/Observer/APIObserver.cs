using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using DynaJson;
using ElectronicObserver.Data;
using ElectronicObserver.Observer.kcsapi;
using ElectronicObserver.Observer.kcsapi.api_get_member;
using ElectronicObserver.Observer.kcsapi.api_req_air_corps;
using ElectronicObserver.Observer.kcsapi.api_req_battle_midnight;
using ElectronicObserver.Observer.kcsapi.api_req_map;
using ElectronicObserver.Observer.kcsapi.api_req_member;
using ElectronicObserver.Observer.kcsapi.api_req_sortie;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;
using static ElectronicObserver.Data.Constants;
using battle = ElectronicObserver.Observer.kcsapi.api_req_sortie.battle;

namespace ElectronicObserver.Observer;

public sealed class APIObserver
{


	#region Singleton

	private static readonly APIObserver instance = new APIObserver();

	public static APIObserver Instance => instance;

	#endregion



	public APIDictionary APIList { get; private set; }

	#region API members

	/// <summary>
	/// Send ship to dock <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L838" />
	/// </summary>
	public APIBase ApiReqNyukyo_Start => this["api_req_nyukyo/start"];

	/// <summary>
	/// Use bucket on docked ship (this doesn't happen if you use a bucket while docking) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L846" />
	/// </summary>
	public APIBase ApiReqNyukyo_Speedchange => this["api_req_nyukyo/speedchange"];

	/// <summary>
	/// Fleet reorganization <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1198" />
	/// </summary>
	public APIBase ApiReqHensei_Change => this["api_req_hensei/change"];

	/// <summary>
	/// Ship scrap <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L767" />
	/// </summary>
	public APIBase ApiReqKousyou_Destroyship => this["api_req_kousyou/destroyship"];

	/// <summary>
	/// Change fleet name <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1239" />
	/// </summary>
	public APIBase ApiReqMember_Updatedeckname => this["api_req_member/updatedeckname"];

	/// <summary>
	/// Ship remodel <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1499" />
	/// </summary>
	public APIBase ApiReqKaisou_Remodeling => this["api_req_kaisou/remodeling"];

	/// <summary>
	/// Sortie start <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1579" />
	/// </summary>
	public APIBase ApiReqMap_Start => this["api_req_map/start"];

	/// <summary>
	/// Make a combined fleet <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3375" />
	/// </summary>
	public APIBase ApiReqHensei_Combined => this["api_req_hensei/combined"];

	/// <summary>
	/// Ship hole-punch <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1460" />
	/// </summary>
	public APIBase ApiReqKaisou_OpenExslot => this["api_req_kaisou/open_exslot"];

	/// <summary>
	/// Get to main screen (home port) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L603" />
	/// </summary>
	public APIBase ApiPort_Port => this["api_port/port"];

	/// <summary>
	/// 艦船情報 (?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2382" />
	/// </summary>
	public APIBase ApiGetMember_Ship2 => this["api_get_member/ship2"];

	/// <summary>
	/// Go to dock screen (also happens after docking a ship, doesn't happen after bucketing a ship that's already in docks) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L826" />
	/// </summary>
	public APIBase ApiGetMember_Ndock => this["api_get_member/ndock"];

	/// <summary>
	/// Get ship from construction <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L740" />
	/// </summary>
	public APIBase ApiReqKousyou_Getship => this["api_req_kousyou/getship"];

	/// <summary>
	/// Resupply <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1163" />
	/// </summary>
	public APIBase ApiReqHokyu_Charge => this["api_req_hokyu/charge"];

	/// <summary>
	/// Happens when adding or removing equipment on a ship (doesn't happen when switching equipment between slots) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1505" />
	/// </summary>
	public APIBase ApiGetMember_Ship3 => this["api_get_member/ship3"];

	/// <summary>
	/// Ship modernization <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1125" />
	/// </summary>
	public APIBase ApiReqKaisou_Powerup => this["api_req_kaisou/powerup"];

	/// <summary>
	/// After sending out an expedition <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1188" />
	/// </summary>
	public APIBase ApiGetMember_Deck => this["api_get_member/deck"];

	/// <summary>
	/// After finishing a sortie <br />
	/// After finishing a quest that rewards ships or equipment <br />
	/// After remodeling a ship <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L685" />
	/// </summary>
	public APIBase ApiGetMember_SlotItem => this["api_get_member/slot_item"];

	/// <summary>
	/// Sortie advance <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1627" />
	/// </summary>
	public APIBase ApiReqMap_Next => this["api_req_map/next"];

	/// <summary>
	/// Sortie advance (right before api_req_map/next) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2392" />
	/// </summary>
	public APIBase ApiGetMember_ShipDeck => this["api_get_member/ship_deck"];

	/// <summary>
	/// Load fleet preset <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1225" />
	/// </summary>
	public APIBase ApiReqHensei_PresetSelect => this["api_req_hensei/preset_select"];

	/// <summary>
	/// Drag and drop equipment between slots <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1473" />
	/// </summary>
	public APIBase ApiReqKaisou_SlotExchangeIndex => this["api_req_kaisou/slot_exchange_index"];

	/// <summary>
	/// Login <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L553" />
	/// </summary>
	public APIBase ApiGetMember_RequireInfo => this["api_get_member/require_info"];

	/// <summary>
	/// Equipment transfer between ships <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1482" />
	/// </summary>
	public APIBase ApiReqKaisou_SlotDeprive => this["api_req_kaisou/slot_deprive"];

	/// <summary>
	/// Marriage <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3390" />
	/// </summary>
	public APIBase ApiReqKaisou_Marriage => this["api_req_kaisou/marriage"];

	/// <summary>
	/// Anchorage repair <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3520" />
	/// </summary>
	public APIBase ApiReqMap_AnchorageRepair => this["api_req_map/anchorage_repair"];

	/// <summary>
	/// Go to quest screen <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L892" />
	/// </summary>
	public questlist ApiGetMember_QuestList => (questlist)this["api_get_member/questlist"];

	/// <summary>
	/// Normal fleet battle finish <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2326" />
	/// </summary>
	public battleresult ApiReqSortie_BattleResult => (battleresult)this["api_req_sortie/battleresult"];

	/// <summary>
	/// Combined fleet battle finish <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3324" />
	/// </summary>
	public kcsapi.api_req_combined_battle.battleresult ApiReqCombinedFleet_BattleResult =>
		(kcsapi.api_req_combined_battle.battleresult)this["api_req_combined_battle/battleresult"];

	/// <summary>
	/// Practice battle finish <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1427" />
	/// </summary>
	public kcsapi.api_req_practice.battle_result ApiReqPractice_BattleResult =>
		(kcsapi.api_req_practice.battle_result)this["api_req_practice/battle_result"];

	/// <summary>
	/// Expedition finish <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1138" />
	/// </summary>
	public kcsapi.api_req_mission.result ApiReqMission_Result =>
		(kcsapi.api_req_mission.result)this["api_req_mission/result"];

	/// <summary>
	/// Equipment development <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L749" />
	/// </summary>
	public kcsapi.api_req_kousyou.createitem ApiReqKousyou_CreateItem =>
		(kcsapi.api_req_kousyou.createitem)this["api_req_kousyou/createitem"];

	/// <summary>
	/// Ship construction <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L720" />
	/// </summary>
	public kcsapi.api_req_kousyou.createship ApiReqKousyou_CreateShip =>
		(kcsapi.api_req_kousyou.createship)this["api_req_kousyou/createship"];

	/// <summary>
	/// Equipment upgrade <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L811" />
	/// </summary>
	public kcsapi.api_req_kousyou.remodel_slot ApiReqKousyou_RemodelSlot =>
		(kcsapi.api_req_kousyou.remodel_slot)this["api_req_kousyou/remodel_slot"];

	/// <summary>
	/// Login <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L9" />
	/// </summary>
	public kcsapi.api_start2.getData ApiStart2_GetData => (kcsapi.api_start2.getData)this["api_start2/getData"];

	/// <summary>
	/// Use torch on a ship that's in construction <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L733" />
	/// </summary>
	public kcsapi.api_req_kousyou.createship_speedchange ApiReqKousyou_CreateShipSpeedChange =>
		(kcsapi.api_req_kousyou.createship_speedchange)this["api_req_kousyou/createship_speedchange"];

	/// <summary>
	/// Equipment scrap <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L775" />
	/// </summary>
	public kcsapi.api_req_kousyou.destroyitem2 ApiReqKousyou_DestroyItem2 =>
		(kcsapi.api_req_kousyou.destroyitem2)this["api_req_kousyou/destroyitem2"];

	/// <summary>
	/// Change admiral comment <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1245" />
	/// </summary>
	public kcsapi.api_req_member.updatecomment ApiReqMember_UpdateComment =>
		(kcsapi.api_req_member.updatecomment)this["api_req_member/updatecomment"];

	/// <summary>
	/// 艦隊司令部情報 (?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L568" />
	/// </summary>
	public basic ApiGetMember_Basic => (basic)this["api_get_member/basic"];

	/// <summary>
	/// After remodeling <br />
	/// After building a ship <br />
	/// After finishing a quest that rewards resources (fuel, ammo, steel, bauxite, bucket, torch, nail, screw) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L703" />
	/// </summary>
	public material ApiGetMember_Material => (material)this["api_get_member/material"];

	/// <summary>
	/// Add plane to AB <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2448" />
	/// </summary>
	public set_plane ApiReqAirCorps_SetPlane => (set_plane)this["api_req_air_corps/set_plane"];

	/// <summary>
	/// AB resupply <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2486" />
	/// </summary>
	public supply ApiReqAirCorps_Supply => (supply)this["api_req_air_corps/supply"];

	/// <summary>
	/// List of items <br />
	/// After finishing a sortie <br />
	/// After finishing an expedition <br />
	/// After finishing a quest that rewards an item <br />
	/// After marriage <br />
	/// After using Mamiya/Irako <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L698" />
	/// </summary>
	public useitem ApiGetMember_UseItem => (useitem)this["api_get_member/useitem"];

	/// <summary>
	/// Sortie screen
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2399" />
	/// </summary>
	public mapinfo ApiGetMember_MapInfo => (mapinfo)this["api_get_member/mapinfo"];

	/// <summary>
	/// Event sortie condition (sortie win rate needs to be above 75%) <br />
	/// Happens right before the sortie <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2426" />
	/// </summary>
	public sortie_conditions ApiGetMember_SortieConditions => (sortie_conditions)this["api_get_member/sortie_conditions"];

	/// <summary>
	/// Player details when clicking an opponent in practice <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1296" />
	/// </summary>
	public get_practice_enemyinfo ApiReqMember_GetPracticeEnemyInfo =>
		(get_practice_enemyinfo)this["api_req_member/get_practice_enemyinfo"];

	/// <summary>
	/// Heavy air raid <br />
	/// todo: documentation
	/// </summary>
	public air_raid ApiReqMap_AirRaid => (air_raid)this["api_req_map/air_raid"];

	/// <summary>
	/// Normal fleet day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1752" />
	/// </summary>
	public battle ApiReqSortie_Battle => (battle)this["api_req_sortie/battle"];

	/// <summary>
	/// Normal fleet night battle after day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1967" />
	/// </summary>
	public kcsapi.api_req_battle_midnight.battle ApiReqBattleMidnight_Battle =>
		(kcsapi.api_req_battle_midnight.battle)this["api_req_battle_midnight/battle"];

	/// <summary>
	/// Normal fleet night battle only <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2007" />
	/// </summary>
	public sp_midnight ApiReqBattleMidnight_SpMidnight =>
		(sp_midnight)this["api_req_battle_midnight/sp_midnight"];

	/// <summary>
	/// Normal fleet air battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2033" />
	/// </summary>
	public airbattle ApiReqSortie_AirBattle => (airbattle)this["api_req_sortie/airbattle"];

	/// <summary>
	/// Normal fleet long distance air battle (like the one in 1-6?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2055" />
	/// </summary>
	public ld_airbattle ApiReqSortie_LdAirBattle => (ld_airbattle)this["api_req_sortie/ld_airbattle"];

	/// <summary>
	/// Normal fleet (?) night to day <br />
	/// todo: documentation
	/// </summary>
	public night_to_day ApiReqSortie_NightToDay => (night_to_day)this["api_req_sortie/night_to_day"];

	/// <summary>
	/// Normal fleet radar ambush <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2075" />
	/// </summary>
	public ld_shooting ApiReqSortie_LdShooting => (ld_shooting)this["api_req_sortie/ld_shooting"];

	/// <summary>
	/// Combined fleet (carrier or transport) day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2557" />
	/// </summary>
	public kcsapi.api_req_combined_battle.battle ApiReqCombinedBattle_Battle =>
		(kcsapi.api_req_combined_battle.battle)this["api_req_combined_battle/battle"];

	/// <summary>
	/// Combined fleet vs normal fleet day to night battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2658" />
	/// </summary>
	public kcsapi.api_req_combined_battle.midnight_battle ApiReqCombinedBattle_MidnightBattle =>
		(kcsapi.api_req_combined_battle.midnight_battle)this["api_req_combined_battle/midnight_battle"];

	/// <summary>
	/// Combined fleet night battle only <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2689" />
	/// </summary>
	public kcsapi.api_req_combined_battle.sp_midnight ApiReqCombinedBattle_SpMidnight =>
		(kcsapi.api_req_combined_battle.sp_midnight)this["api_req_combined_battle/sp_midnight"];

	/// <summary>
	/// Combined fleet air battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2501" />
	/// </summary>
	public kcsapi.api_req_combined_battle.airbattle ApiReqCombinedBattle_AirBattle =>
		(kcsapi.api_req_combined_battle.airbattle)this["api_req_combined_battle/airbattle"];

	/// <summary>
	/// Combined fleet (surface) day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2694" />
	/// </summary>
	public kcsapi.api_req_combined_battle.battle_water ApiReqCombinedBattle_BattleWater =>
		(kcsapi.api_req_combined_battle.battle_water)this["api_req_combined_battle/battle_water"];

	/// <summary>
	/// Combined fleet long distance air battle (the ones from Midway event?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2802" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ld_airbattle ApiReqCombinedBattle_LdAirBattle =>
		(kcsapi.api_req_combined_battle.ld_airbattle)this["api_req_combined_battle/ld_airbattle"];

	/// <summary>
	/// Normal fleet vs enemy combined fleet day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2143" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ec_battle ApiReqCombinedBattle_EcBattle =>
		(kcsapi.api_req_combined_battle.ec_battle)this["api_req_combined_battle/ec_battle"];

	/// <summary>
	/// Normal/combined fleet vs enemy combined fleet day to night battle (?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2268" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ec_midnight_battle ApiReqCombinedBattle_EcMidnightBattle =>
		(kcsapi.api_req_combined_battle.ec_midnight_battle)this["api_req_combined_battle/ec_midnight_battle"];

	/// <summary>
	/// Enemy combined fleet night to day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3105" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ec_night_to_day ApiReqCombinedBattle_EcNightToDay =>
		(kcsapi.api_req_combined_battle.ec_night_to_day)this["api_req_combined_battle/ec_night_to_day"];

	/// <summary>
	/// Combined fleet (carrier) vs enemy combined fleet day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2859" />
	/// </summary>
	public kcsapi.api_req_combined_battle.each_battle ApiReqCombinedBattle_EachBattle =>
		(kcsapi.api_req_combined_battle.each_battle)this["api_req_combined_battle/each_battle"];

	/// <summary>
	/// Combined fleet (surface) vs enemy combined fleet day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2964" />
	/// </summary>
	public kcsapi.api_req_combined_battle.each_battle_water ApiReqCombinedBattle_EachBattleWater =>
		(kcsapi.api_req_combined_battle.each_battle_water)this["api_req_combined_battle/each_battle_water"];

	/// <summary>
	/// Combined fleet radar ambush <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3296" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ld_shooting ApiReqCombinedBattle_LdShooting =>
		(kcsapi.api_req_combined_battle.ld_shooting)this["api_req_combined_battle/ld_shooting"];

	/// <summary>
	/// Practice day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1326" />
	/// </summary>
	public kcsapi.api_req_practice.battle ApiReqPractice_Battle =>
		(kcsapi.api_req_practice.battle)this["api_req_practice/battle"];

	/// <summary>
	/// Practice night battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1402" />
	/// </summary>
	public kcsapi.api_req_practice.midnight_battle ApiReqPractice_MidnightBattle =>
		(kcsapi.api_req_practice.midnight_battle)this["api_req_practice/midnight_battle"];

	/// <summary>
	/// Go to album <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L963" />
	/// </summary>
	public picture_book ApiGetMember_PictureBook => (picture_book)this["api_get_member/picture_book"];

	/// <summary>
	/// Go to expedition screen <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L853" />
	/// </summary>
	public mission ApiGetMember_Mission => (mission)this["api_get_member/mission"];

	/// <summary>
	/// Send out expedition <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1178" />
	/// </summary>
	public kcsapi.api_req_mission.start ApiReqMission_Start =>
		(kcsapi.api_req_mission.start)this["api_req_mission/start"];

	/// <summary>
	/// Switching AB planes (?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2433" />
	/// </summary>
	public base_air_corps ApiGetMember_BaseAirCorps => (base_air_corps)this["api_get_member/base_air_corps"];

	/// <summary>
	/// Move AB planes between different bases <br />
	/// todo: documentation
	/// </summary>
	public kcsapi.api_req_air_corps.change_deployment_base ApiReqAirCorps_ChangeDeploymentBase =>
		(kcsapi.api_req_air_corps.change_deployment_base)this["api_req_air_corps/change_deployment_base"];

	/// <summary>
	/// Change AB name <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2461" />
	/// </summary>
	public kcsapi.api_req_air_corps.change_name ApiReqAirCorps_ChangeName =>
		(kcsapi.api_req_air_corps.change_name)this["api_req_air_corps/change_name"];

	/// <summary>
	/// Change AB action (sortie, air def, rest etc) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2469" />
	/// </summary>
	public kcsapi.api_req_air_corps.set_action ApiReqAirCorps_SetAction =>
		(kcsapi.api_req_air_corps.set_action)this["api_req_air_corps/set_action"];

	/// <summary>
	/// Open new AB <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2469" />
	/// </summary>
	public kcsapi.api_req_air_corps.expand_base ApiReqAirCorps_ExpandBase =>
		(kcsapi.api_req_air_corps.expand_base)this["api_req_air_corps/expand_base"];

	#endregion

	public string? ServerAddress { get; private set; }
	public int ProxyPort { get; private set; }

	public delegate void ProxyStartedEventHandler();
	public event ProxyStartedEventHandler ProxyStarted = delegate { };

	private object UIControl;


	public event APIReceivedEventHandler RequestReceived = delegate { };
	public event APIReceivedEventHandler ResponseReceived = delegate { };

	private ProxyServer Proxy { get; }
	private ExplicitProxyEndPoint Endpoint { get; set; }

	private APIObserver()
	{

		APIList = new APIDictionary
		{
			new kcsapi.api_start2.getData(),
			new kcsapi.api_get_member.basic(),
			new kcsapi.api_get_member.slot_item(),
			new kcsapi.api_get_member.useitem(),
			new kcsapi.api_get_member.kdock(),
			new kcsapi.api_port.port(),
			new kcsapi.api_get_member.ship2(),
			new kcsapi.api_get_member.questlist(),
			new kcsapi.api_get_member.ndock(),
			new kcsapi.api_req_kousyou.getship(),
			new kcsapi.api_req_hokyu.charge(),
			new kcsapi.api_req_kousyou.destroyship(),
			new kcsapi.api_req_kousyou.destroyitem2(),
			new kcsapi.api_req_member.get_practice_enemyinfo(),
			new kcsapi.api_get_member.picture_book(),
			new kcsapi.api_req_mission.start(),
			new kcsapi.api_get_member.ship3(),
			new kcsapi.api_req_kaisou.powerup(),
			new kcsapi.api_req_map.start(),
			new kcsapi.api_req_map.next(),
			new kcsapi.api_req_map.air_raid(),
			new kcsapi.api_req_kousyou.createitem(),
			new kcsapi.api_req_sortie.battle(),
			new kcsapi.api_req_sortie.battleresult(),
			new kcsapi.api_req_battle_midnight.battle(),
			new kcsapi.api_req_battle_midnight.sp_midnight(),
			new kcsapi.api_req_combined_battle.battle(),
			new kcsapi.api_req_combined_battle.midnight_battle(),
			new kcsapi.api_req_combined_battle.sp_midnight(),
			new kcsapi.api_req_combined_battle.airbattle(),
			new kcsapi.api_req_combined_battle.battleresult(),
			new kcsapi.api_req_practice.battle(),
			new kcsapi.api_req_practice.midnight_battle(),
			new kcsapi.api_req_practice.battle_result(),
			new kcsapi.api_get_member.deck(),
			new kcsapi.api_get_member.mapinfo(),
			new kcsapi.api_get_member.mission(),
			new kcsapi.api_req_combined_battle.battle_water(),
			new kcsapi.api_req_combined_battle.goback_port(),
			new kcsapi.api_req_kousyou.remodel_slot(),
			new kcsapi.api_get_member.material(),
			new kcsapi.api_req_mission.result(),
			new kcsapi.api_req_ranking.getlist(),
			new kcsapi.api_req_sortie.airbattle(),
			new kcsapi.api_get_member.ship_deck(),
			new kcsapi.api_req_kaisou.marriage(),
			new kcsapi.api_req_hensei.preset_select(),
			new kcsapi.api_req_kaisou.slot_exchange_index(),
			new kcsapi.api_get_member.record(),
			new kcsapi.api_get_member.payitem(),
			new kcsapi.api_req_kousyou.remodel_slotlist(),
			new kcsapi.api_req_sortie.ld_airbattle(),
			new kcsapi.api_req_combined_battle.ld_airbattle(),
			new kcsapi.api_get_member.require_info(),
			new kcsapi.api_get_member.base_air_corps(),
			new kcsapi.api_req_air_corps.change_deployment_base(),
			new kcsapi.api_req_air_corps.set_plane(),
			new kcsapi.api_req_air_corps.set_action(),
			new kcsapi.api_req_air_corps.supply(),
			new kcsapi.api_req_kaisou.slot_deprive(),
			new kcsapi.api_req_air_corps.expand_base(),
			new kcsapi.api_req_combined_battle.ec_battle(),
			new kcsapi.api_req_combined_battle.ec_midnight_battle(),
			new kcsapi.api_req_combined_battle.each_battle(),
			new kcsapi.api_req_combined_battle.each_battle_water(),
			new kcsapi.api_get_member.sortie_conditions(),
			new kcsapi.api_req_sortie.night_to_day(),
			new kcsapi.api_req_combined_battle.ec_night_to_day(),
			new kcsapi.api_req_sortie.goback_port(),
			new kcsapi.api_req_member.itemuse(),
			new kcsapi.api_req_sortie.ld_shooting(),
			new kcsapi.api_req_combined_battle.ld_shooting(),
			new kcsapi.api_req_map.anchorage_repair(),
			new kcsapi.api_req_map.start_air_base(),
			new kcsapi.api_get_member.preset_deck(),

			new kcsapi.api_req_quest.clearitemget(),
			new kcsapi.api_req_nyukyo.start(),
			new kcsapi.api_req_nyukyo.speedchange(),
			new kcsapi.api_req_kousyou.createship(),
			new kcsapi.api_req_kousyou.createship_speedchange(),
			new kcsapi.api_req_hensei.change(),
			new kcsapi.api_req_member.updatedeckname(),
			new kcsapi.api_req_kaisou.remodeling(),
			new kcsapi.api_req_kaisou.open_exslot(),
			new kcsapi.api_req_map.select_eventmap_rank(),
			new kcsapi.api_req_hensei.combined(),
			new kcsapi.api_req_member.updatecomment(),
			new kcsapi.api_req_air_corps.change_name(),
			new kcsapi.api_req_quest.stop(),
			new kcsapi.api_req_hensei.preset_register(),
			new kcsapi.api_req_hensei.preset_delete(),
		};

		Proxy = new ProxyServer
		{
			ExceptionFunc = async exception =>
			{
				// todo write to output
			},
			TcpTimeWaitSeconds = 10,
			ConnectionTimeOutSeconds = 15,
			ReuseSocket = false,
			EnableConnectionPool = false,
			ForwardToUpstreamGateway = true
		};
		Proxy.CertificateManager.RootCertificate = new X509Certificate2();
		Proxy.BeforeRequest += ProxyOnBeforeRequest;
		Proxy.BeforeResponse += ProxyOnBeforeResponse;
	}

	/// <summary>
	/// 通信の受信を開始します。
	/// </summary>
	/// <param name="portID">受信に使用するポート番号。</param>
	/// <param name="UIControl">GUI スレッドで実行するためのオブジェクト。中身は何でもいい</param>
	/// <returns>実際に使用されるポート番号。</returns>
	public int Start(int portID, object UIControl)
	{
		Utility.Configuration.ConfigurationData.ConfigConnection c = Utility.Configuration.Config.Connection;

		this.UIControl = UIControl;

		if (Proxy.ProxyRunning) Proxy.Stop();

		try
		{
			Endpoint = new ExplicitProxyEndPoint(IPAddress.Any, portID, false);
			Proxy.AddEndPoint(Endpoint);

			if (c.UseUpstreamProxy)
			{
				Proxy.UpStreamHttpProxy = new ExternalProxy(c.UpstreamProxyAddress, c.UpstreamProxyPort);
			}
			else if (c.UseSystemProxy)
			{
				// todo system proxy
				// HttpProxy.UpstreamProxyConfig = new ProxyConfig(ProxyConfigType.SystemProxy);
			}

			Proxy.Start();
			ProxyPort = portID;

			ProxyStarted();

			Utility.Logger.Add(1, string.Format(LoggerRes.APIObserverStarted, portID));

		}
		catch (Exception ex)
		{

			Utility.Logger.Add(3, "APIObserver: Failed to start observation. " + ex.Message);
			ProxyPort = 0;
		}


		return ProxyPort;
	}

	/// <summary>
	/// 通信の受信を停止します。
	/// </summary>
	public void Stop()
	{
		Proxy.Stop();

		Utility.Logger.Add(1, LoggerRes.APIObserverStopped);
	}

	public APIBase this[string key]
	{
		get
		{
			if (APIList.ContainsKey(key)) return APIList[key];
			else return null;
		}
	}

	private async Task ProxyOnBeforeRequest(object sender, SessionEventArgs e)
	{
		e.HttpClient.Request.KeepBody = true;
		// need to read the request body here so it's available in ProxyOnBeforeResponse
		await e.GetRequestBodyAsString();
	}

	private async Task ProxyOnBeforeResponse(object sender, SessionEventArgs e)
	{
		if (e.HttpClient.Response.StatusCode != 200) return;

		Configuration.ConfigurationData.ConfigConnection c = Configuration.Config.Connection;

		string baseurl = e.HttpClient.Request.RequestUri.AbsoluteUri;

		// request
		if (baseurl.Contains("/kcsapi/"))
		{

			string url = baseurl;
			string body = await e.GetRequestBodyAsString();

			//保存
			if (c.SaveReceivedData && c.SaveRequest)
			{
				Task.Run((Action)(() => { SaveRequest(url, body); }));
			}

			switch (UIControl)
			{
				case Control control:
					control.BeginInvoke((Action)(() => { LoadRequest(url, body); }));
					break;
				case System.Windows.Controls.Control control:
					control.Dispatcher.Invoke(() => LoadRequest(url, body));
					break;
			}
		}

		//debug
		//Utility.Logger.Add( 1, baseurl );

		if (baseurl == ("/gadgets/makeRequest"))
		{
			KCDatabase db = KCDatabase.Instance;
			if (db.Server is null)
			{
				string body = await e.GetResponseBodyAsString();
				string url = body.Split('/')[2];
				url = url.Split('\\')[0];

				db.Server = getKCServer(url);
			}
		}

		//response
		//保存
		if (c.SaveReceivedData)
		{

			try
			{

				if (!Directory.Exists(c.SaveDataPath))
					Directory.CreateDirectory(c.SaveDataPath);


				if (c.SaveResponse && baseurl.Contains("/kcsapi/"))
				{

					// 非同期で書き出し処理するので取っておく
					// stringはイミュータブルなのでOK
					string url = baseurl;
					string body = await e.GetResponseBodyAsString();

					Task.Run((Action)(() =>
					{
						SaveResponse(url, body);
					}));

				}
				else if (baseurl.Contains("/kcs") && c.SaveOtherFile)
				{

					string saveDataPath = c.SaveDataPath; // スレッド間の競合を避けるため取っておく

					string tpath = string.Format("{0}\\{1}", saveDataPath, baseurl.Substring(baseurl.IndexOf("/kcs") + 1).Replace("/", "\\"));
					//Logger.Add(1, $"{baseurl} $ {tpath}");
					{
						int index = tpath.IndexOf("?");
						if (index != -1)
						{
							if (Utility.Configuration.Config.Connection.ApplyVersion)
							{
								string over = tpath.Substring(index + 1);
								int vindex = over.LastIndexOf("VERSION=", StringComparison.CurrentCultureIgnoreCase);
								if (vindex != -1)
								{
									string version = over.Substring(vindex + 8).Replace('.', '_');
									tpath = tpath.Insert(tpath.LastIndexOf('.', index), "_v" + version);
									index += version.Length + 2;
								}

							}

							tpath = tpath.Remove(index);
						}
					}



					// 非同期で書き出し処理するので取っておく
					byte[] responseCopy = new byte[(await e.GetResponseBody()).Length];
					Array.Copy(await e.GetResponseBody(), responseCopy, (await e.GetResponseBody()).Length);

					Task.Run((Action)(() =>
					{
						try
						{
							lock (this)
							{
								// 同時に書き込みが走るとアレなのでロックしておく

								Directory.CreateDirectory(Path.GetDirectoryName(tpath));

								//System.Diagnostics.Debug.WriteLine( oSession.fullUrl + " => " + tpath );
								using (var sw = new System.IO.BinaryWriter(System.IO.File.OpenWrite(tpath)))
								{
									sw.Write(responseCopy);
								}
							}

							Utility.Logger.Add(1, string.Format(LoggerRes.SavedAPI, tpath.Remove(0, saveDataPath.Length + 1)));

						}
						catch (IOException ex)
						{   //ファイルがロックされている; 頻繁に出るのでエラーレポートを残さない

							Utility.Logger.Add(3, LoggerRes.FailedSaveAPI + ex.Message);
						}
					}));

				}

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, LoggerRes.FailedSaveAPI);
			}

		}

		if (baseurl.Contains("/kcsapi/") && e.HttpClient.Response.ContentType == "text/plain")
		{
			// 非同期でGUIスレッドに渡すので取っておく
			// stringはイミュータブルなのでOK
			string url = baseurl;
			string body = await e.GetResponseBodyAsString();

			switch (UIControl)
			{
				case Control control:
					control.BeginInvoke((Action)(() => { LoadResponse(url, body); }));
					break;
				case System.Windows.Controls.Control control:
					control.Dispatcher.Invoke(() => LoadResponse(url, body));
					break;
			}
		}


		if (ServerAddress == null && baseurl.Contains("/kcsapi/"))
		{
			ServerAddress = e.HttpClient.Request.Host;
		}
	}

	public void LoadRequest(string path, string data)
	{

		string shortpath = path.Substring(path.LastIndexOf("/kcsapi/") + 8);

		try
		{

			Utility.Logger.Add(1, LoggerRes.RecievedRequest + shortpath);

			SystemEvents.UpdateTimerEnabled = false;


			var parsedData = new Dictionary<string, string>();


			foreach (string unit in data.Split("&".ToCharArray()))
			{
				string[] pair = unit.Split("=".ToCharArray());
				parsedData.Add(HttpUtility.UrlDecode(pair[0]), HttpUtility.UrlDecode(pair[1]));
			}

			RequestReceived(shortpath, parsedData);
			APIList.OnRequestReceived(shortpath, parsedData);

		}
		catch (Exception ex)
		{

			ErrorReporter.SendErrorReport(ex, LoggerRes.RequestError, shortpath, data);

		}
		finally
		{

			SystemEvents.UpdateTimerEnabled = true;

		}

	}


	public void LoadResponse(string path, string data)
	{

		string shortpath = path.Substring(path.LastIndexOf("/kcsapi/") + 8);

		try
		{

			Utility.Logger.Add(1, "Received response: " + shortpath);

			SystemEvents.UpdateTimerEnabled = false;


			var json = JsonObject.Parse(data.Substring(7));        //remove "svdata="

			int result = (int)json.api_result;
			if (result != 1)
			{
				if (result == 201)
				{
					Utility.Logger.Add(3,
						"Error code 201 was received. It may be triggered by macro detection " +
						"or by starting KanColle from another device.");
				}
				else
				{
					throw new InvalidOperationException($"Error code {result} was received from the server.");
				}
			}


			if (shortpath == "api_get_member/ship2")
			{
				ResponseReceived(shortpath, json);
				APIList.OnResponseReceived(shortpath, json);
			}
			else if (shortpath.Contains("api_req_ranking"))
			{
				shortpath = "api_req_ranking/getlist";
				ResponseReceived(shortpath, json.api_data);
				APIList.OnResponseReceived(shortpath, json.api_data);
			}
			else if (json.IsDefined("api_data"))
			{
				ResponseReceived(shortpath, json.api_data);
				APIList.OnResponseReceived(shortpath, json.api_data);
			}
			else
			{
				ResponseReceived(shortpath, null);
				APIList.OnResponseReceived(shortpath, null);
			}

		}
		catch (Exception ex)
		{

			ErrorReporter.SendErrorReport(ex,
				"There was an error while receiving the response.",
				shortpath, data);

		}
		finally
		{

			SystemEvents.UpdateTimerEnabled = true;

		}

	}


	private void SaveRequest(string url, string body)
	{

		try
		{

			string tpath = string.Format("{0}\\{1}Q@{2}.json", Utility.Configuration.Config.Connection.SaveDataPath, DateTimeHelper.GetTimeStamp(), url.Substring(url.LastIndexOf("/kcsapi/") + 8).Replace("/", "@"));

			using (var sw = new System.IO.StreamWriter(tpath, false, Encoding.UTF8))
			{
				sw.Write(body);
			}


		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, LoggerRes.FailedSaveAPI);

		}
	}


	private void SaveResponse(string url, string body)
	{

		try
		{

			string tpath = string.Format("{0}\\{1}S@{2}.json", Utility.Configuration.Config.Connection.SaveDataPath, DateTimeHelper.GetTimeStamp(), url.Substring(url.LastIndexOf("/kcsapi/") + 8).Replace("/", "@"));

			using (var sw = new System.IO.StreamWriter(tpath, false, Encoding.UTF8))
			{
				sw.Write(body);
			}

		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, LoggerRes.FailedSaveAPI);

		}



	}

}
