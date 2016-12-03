using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Detail {

	public static class BattleDetailDescriptor {

		public static string GetBattleDetail( BattleManager bm ) {
			var sb = new StringBuilder();

			if ( bm.IsPractice ) {
				sb.AppendLine( ConstantsRes.Practice );

			} else {
				sb.AppendFormat( "{0} ({1}-{2})", bm.Compass.MapInfo.Name, bm.Compass.MapAreaID, bm.Compass.MapInfoID );
				if ( bm.Compass.MapInfo.EventDifficulty > 0 )
					sb.AppendFormat( " [{0}]", Constants.GetDifficulty( bm.Compass.MapInfo.EventDifficulty ) );
				sb.Append( ConstantsRes.BattleDetail_Node ).Append( bm.Compass.Destination.ToString() );
				if ( bm.Compass.EventID == 5 )
					sb.Append( ConstantsRes.BattleDetail_Boss );
				sb.AppendLine();
			}
			if ( bm.Result != null ) {
				sb.AppendLine( bm.Result.EnemyFleetName );
			}
			sb.AppendLine();


			sb.AppendFormat( "◆ {0} ◆\r\n", bm.FirstBattle.BattleName ).AppendLine( GetBattleDetail( bm.FirstBattle ) );
			if ( bm.SecondBattle != null )
				sb.AppendFormat( "◆ {0} ◆\r\n", bm.SecondBattle.BattleName ).AppendLine( GetBattleDetail( bm.SecondBattle ) );


			if ( bm.Result != null ) {
				sb.AppendLine( GetBattleResult( bm ) );
			}

			return sb.ToString();
		}


		public static string GetBattleDetail( BattleData battle ) {

			var sbmaster = new StringBuilder();
			bool isBaseAirRaid = ( battle.BattleType & BattleData.BattleTypeFlag.BaseAirRaid ) != 0;


			foreach ( var phase in battle.GetPhases() ) {

				var sb = new StringBuilder();

				if ( phase is PhaseBaseAirRaid ) {
					var p = phase as PhaseBaseAirRaid;

					sb.AppendLine( ConstantsRes.BattleDetail_AirAttackUnits );
					sb.Append( "　" ).AppendLine( string.Join( ", ", p.Squadrons.Where( sq => sq.EquipmentInstance != null ).Select( sq => sq.ToString() ) ) );

					GetBattleDetailPhaseAirBattle( sb, p );

				} else if ( phase is PhaseAirBattle ) {
					var p = phase as PhaseAirBattle;

					GetBattleDetailPhaseAirBattle( sb, p );


				} else if ( phase is PhaseBaseAirAttack ) {
					var p = phase as PhaseBaseAirAttack;

					foreach ( var a in p.AirAttackUnits ) {
						sb.AppendFormat( ConstantsRes.BattleDetail_AirAttackWave + "\r\n", a.AirAttackIndex + 1 );

						sb.AppendLine( ConstantsRes.BattleDetail_AirAttackUnits );
						sb.Append( "　" ).AppendLine( string.Join( ", ", a.Squadrons.Where( sq => sq.EquipmentInstance != null ).Select( sq => sq.ToString() ) ) );

						GetBattleDetailPhaseAirBattle( sb, a );
						sb.Append( a.GetBattleDetail() );
					}


				} else if ( phase is PhaseInitial ) {
					var p = phase as PhaseInitial;

					if ( p.FriendFleetEscort != null )
						sb.AppendLine( ConstantsRes.BattleDetail_FriendMainFleet );
					else
						sb.AppendLine( ConstantsRes.BattleDetail_FriendFleet );

					if ( isBaseAirRaid )
						OutputFriendBase( sb, p.InitialHPs.Take( 6 ).ToArray(), p.MaxHPs.Take( 6 ).ToArray() );
					else
						OutputFriendData( sb, p.FriendFleet, p.InitialHPs.Take( 6 ).ToArray(), p.MaxHPs.Take( 6 ).ToArray() );

					if ( p.FriendFleetEscort != null ) {
						sb.AppendLine();
						sb.AppendLine( ConstantsRes.BattleDetail_FriendEscortFleet );

						OutputFriendData( sb, p.FriendFleetEscort, p.InitialHPs.Skip( 12 ).Take( 6 ).ToArray(), p.MaxHPs.Skip( 12 ).Take( 6 ).ToArray() );
					}

					sb.AppendLine();

					if ( p.EnemyMembersEscort != null )
						sb.Append( ConstantsRes.BattleDetail_EnemyMainFleet );
					else
						sb.Append( ConstantsRes.BattleDetail_EnemyFleet );

					if ( p.IsBossDamaged )
						sb.Append( ConstantsRes.BattleDetail_IsBossDamaged );
					sb.AppendLine();

					OutputEnemyData( sb, p.EnemyMembersInstance, p.EnemyLevels, p.InitialHPs.Skip( 6 ).Take( 6 ).ToArray(), p.MaxHPs.Skip( 6 ).Take( 6 ).ToArray(), p.EnemySlotsInstance, p.EnemyParameters );


					if ( p.EnemyMembersEscort != null ) {
						sb.AppendLine();
						sb.AppendLine( ConstantsRes.BattleDetail_EnemyEscortFleet );

						OutputEnemyData( sb, p.EnemyMembersEscortInstance, p.EnemyLevelsEscort, p.InitialHPs.Skip( 18 ).Take( 6 ).ToArray(), p.MaxHPs.Skip( 18 ).Take( 6 ).ToArray(), p.EnemySlotsEscortInstance, p.EnemyParametersEscort );
					}

					sb.AppendLine();

					if ( battle.GetPhases().Where( ph => ph is PhaseBaseAirAttack || ph is PhaseBaseAirRaid ).Any( ph => ph != null && ph.IsAvailable ) ) {
						sb.AppendLine( ConstantsRes.BattleDetail_AirBase );
						GetBattleDetailBaseAirCorps( sb, KCDatabase.Instance.Battle.Compass.MapAreaID );		// :(
						sb.AppendLine();
					}

					if ( p.RationIndexes.Length > 0 ) {
						sb.AppendLine( "〈戦闘糧食補給〉" );
						foreach ( var index in p.RationIndexes ) {
							ShipData ship;

							if ( index < 6 )
								ship = p.FriendFleet.MembersInstance[index];
							else
								ship = p.FriendFleetEscort.MembersInstance[index - 6];

							if ( ship != null ) {
								sb.AppendFormat( "　{0} #{1}\r\n", ship.NameWithLevel, index );
							}
						}
						sb.AppendLine();
					}


				} else if ( phase is PhaseNightBattle ) {
					var p = phase as PhaseNightBattle;
					int length = sb.Length;

					{
						var eq = KCDatabase.Instance.MasterEquipments[p.TouchAircraftFriend];
						if ( eq != null ) {
							sb.Append( ConstantsRes.BattleDetail_FriendlyNightContact ).AppendLine( eq.Name );
						}
						eq = KCDatabase.Instance.MasterEquipments[p.TouchAircraftEnemy];
						if ( eq != null ) {
							sb.Append( ConstantsRes.BattleDetail_EnemyNightContact ).AppendLine( eq.Name );
						}
					}

					{
						int searchlightIndex = p.SearchlightIndexFriend;
						if ( searchlightIndex != -1 ) {
							sb.AppendFormat( ConstantsRes.BattleDetail_FriendlySearchlight + "\r\n", p.FriendFleet.MembersInstance[searchlightIndex].Name, searchlightIndex + 1 );
						}
						searchlightIndex = p.SearchlightIndexEnemy;
						if ( searchlightIndex != -1 ) {
							sb.AppendFormat( ConstantsRes.BattleDetail_EnemySearchlight + "\r\n", p.EnemyMembersInstance[searchlightIndex].NameWithClass, searchlightIndex + 1 );
						}
					}

					if ( p.FlareIndexFriend != -1 ) {
						sb.AppendFormat( ConstantsRes.BattleDetail_FriendlyStarshell + "\r\n", p.FriendFleet.MembersInstance[p.FlareIndexFriend].Name, p.FlareIndexFriend + 1 );
					}
					if ( p.FlareIndexEnemy != -1 ) {
						sb.AppendFormat( ConstantsRes.BattleDetail_EnemyStarshell + "\r\n", p.FlareEnemyInstance.NameWithClass, p.FlareIndexEnemy + 1 );
					}

					if ( sb.Length > length )		// 追加行があった場合
						sb.AppendLine();


				} else if ( phase is PhaseSearching ) {
					var p = phase as PhaseSearching;

					sb.Append( ConstantsRes.BattleDetail_FormationFriend ).Append( Constants.GetFormation( p.FormationFriend ) );
					sb.Append( ConstantsRes.BattleDetail_FormationEnemy ).AppendLine( Constants.GetFormation( p.FormationEnemy ) );
					sb.Append( ConstantsRes.BattleDetail_EngagementForm ).AppendLine( Constants.GetEngagementForm( p.EngagementForm ) );
					sb.Append( ConstantsRes.BattleDetail_SearchingFriend ).Append( Constants.GetSearchingResult( p.SearchingFriend ) );
					sb.Append( ConstantsRes.BattleDetail_SearchingEnemy ).AppendLine( Constants.GetSearchingResult( p.SearchingEnemy ) );

					sb.AppendLine();
				}


				if ( !( phase is PhaseBaseAirAttack ) )		// 通常出力と重複するため
					sb.Append( phase.GetBattleDetail() );

				if ( sb.Length > 0 ) {
					sbmaster.AppendFormat( ConstantsRes.BattleDetail_TitleBrackets + "\r\n", phase.Title ).Append( sb );
				}
			}


			{
				sbmaster.AppendLine( ConstantsRes.BattleDetail_BattleEnd );

				var friend = battle.Initial.FriendFleet;
				var friendescort = battle.Initial.FriendFleetEscort;
				var enemy = battle.Initial.EnemyMembersInstance;
				var enemyescort = battle.Initial.EnemyMembersEscortInstance;

				if ( friendescort != null )
					sbmaster.AppendLine( ConstantsRes.BattleDetail_FriendMainFleet );
				else
					sbmaster.AppendLine( ConstantsRes.BattleDetail_FriendFleet );

				if ( isBaseAirRaid ) {

					for ( int i = 0; i < 6; i++ ) {
						if ( battle.Initial.MaxHPs[i] <= 0 )
							continue;

						OutputResultData( sbmaster, i, string.Format( ConstantsRes.BattleDetail_Base, i + 1 ),
							battle.Initial.InitialHPs[i], battle.ResultHPs[i], battle.Initial.MaxHPs[i] );
					}

				} else {
					for ( int i = 0; i < friend.Members.Count(); i++ ) {
						var ship = friend.MembersInstance[i];
						if ( ship == null )
							continue;

						OutputResultData( sbmaster, i, ship.Name,
							battle.Initial.InitialHPs[i], battle.ResultHPs[i], battle.Initial.MaxHPs[i] );
					}
				}

				if ( friendescort != null ) {
					sbmaster.AppendLine().AppendLine( ConstantsRes.BattleDetail_FriendEscortFleet );

					for ( int i = 0; i < friendescort.Members.Count(); i++ ) {
						var ship = friendescort.MembersInstance[i];
						if ( ship == null )
							continue;

						OutputResultData( sbmaster, i + 6, ship.Name,
							battle.Initial.InitialHPs[i + 12], battle.ResultHPs[i + 12], battle.Initial.MaxHPs[i + 12] );
					}

				}


				sbmaster.AppendLine();
				if ( enemyescort != null )
					sbmaster.AppendLine( ConstantsRes.BattleDetail_EnemyMainFleet );
				else
					sbmaster.AppendLine( ConstantsRes.BattleDetail_EnemyFleet );

				for ( int i = 0; i < enemy.Length; i++ ) {
					var ship = enemy[i];
					if ( ship == null )
						continue;

					OutputResultData( sbmaster, i,
						ship.NameWithClass,
						battle.Initial.InitialHPs[i + 6], battle.ResultHPs[i + 6], battle.Initial.MaxHPs[i + 6] );
				}

				if ( enemyescort != null ) {
					sbmaster.AppendLine().AppendLine( ConstantsRes.BattleDetail_EnemyEscortFleet );

					for ( int i = 0; i < enemyescort.Length; i++ ) {
						var ship = enemyescort[i];
						if ( ship == null )
							continue;

						OutputResultData( sbmaster, i + 6, ship.NameWithClass,
							battle.Initial.InitialHPs[i + 18], battle.ResultHPs[i + 18], battle.Initial.MaxHPs[i + 18] );
					}
				}

				sbmaster.AppendLine();
			}

			return sbmaster.ToString();
		}


		private static void GetBattleDetailBaseAirCorps( StringBuilder sb, int mapAreaID ) {
			foreach ( var corps in KCDatabase.Instance.BaseAirCorps.Values.Where( corps => corps.MapAreaID == mapAreaID ) ) {
				sb.AppendFormat( "{0} [{1}]\r\n　{2}\r\n",
					corps.Name, Constants.GetBaseAirCorpsActionKind( corps.ActionKind ),
					string.Join( ", ", corps.Squadrons.Values
						.Where( sq => sq.State == 1 && sq.EquipmentInstance != null )
						.Select( sq => sq.EquipmentInstance.NameWithLevel ) ) );
			}
		}

		private static void GetBattleDetailPhaseAirBattle( StringBuilder sb, PhaseAirBattle p ) {

			if ( p.IsStage1Available ) {
				sb.Append( "Stage 1: " ).AppendLine( Constants.GetAirSuperiority( p.AirSuperiority ) );
				sb.AppendFormat("　" + Window.GeneralRes.FriendlyAir + ": -{0}/{1}\r\n　" + Window.GeneralRes.EnemyAir + ": -{2}/{3}\r\n",
					p.AircraftLostStage1Friend, p.AircraftTotalStage1Friend,
					p.AircraftLostStage1Enemy, p.AircraftTotalStage1Enemy );
				if ( p.TouchAircraftFriend > 0 )
					sb.AppendFormat("　" + ConstantsRes.BattleDetail_AirBaseFriendlyContact + ": {0}\r\n", KCDatabase.Instance.MasterEquipments[p.TouchAircraftFriend].Name );
				if ( p.TouchAircraftEnemy > 0 )
					sb.AppendFormat("　" + ConstantsRes.BattleDetail_AirBaseEnemyContact + ": {0}\r\n", KCDatabase.Instance.MasterEquipments[p.TouchAircraftEnemy].Name );
			}
			if ( p.IsStage2Available ) {
				sb.Append( "Stage 2: " );
				if ( p.IsAACutinAvailable ) {
					sb.AppendFormat( Window.GeneralRes.AACutIn + " ( {0}, {1}({2}) )", p.AACutInShip.NameWithLevel, Constants.GetAACutinKind( p.AACutInKind ), p.AACutInKind );
				}
				sb.AppendLine();
				sb.AppendFormat( "　" + Window.GeneralRes.FriendlyAir + ": -{0}/{1}\r\n　" + Window.GeneralRes.EnemyAir + ": -{2}/{3}\r\n",
					p.AircraftLostStage2Friend, p.AircraftTotalStage2Friend,
					p.AircraftLostStage2Enemy, p.AircraftTotalStage2Enemy );
			}
			sb.AppendLine();
		}


		private static void OutputFriendData( StringBuilder sb, FleetData fleet, int[] initialHPs, int[] maxHPs ) {

			for ( int i = 0; i < fleet.MembersInstance.Count; i++ ) {
				var ship = fleet.MembersInstance[i];

				if ( ship == null )
					continue;

				sb.AppendFormat( ConstantsRes.BattleDetail_ShipStats + "\r\n",
					i + 1,
					ship.MasterShip.ShipTypeName, ship.NameWithLevel,
					initialHPs[i], maxHPs[i],
					ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase );

				sb.Append( "　" );
				for ( int k = 0; k < ship.SlotInstance.Count; k++ ) {
					var eq = ship.SlotInstance[k];
					if ( eq != null ) {
						if ( k > 0 )
							sb.Append( ", " );
						sb.Append( eq.ToString() );
					}
				}
				sb.AppendLine();
			}
		}

		private static void OutputFriendBase( StringBuilder sb, int[] initialHPs, int[] maxHPs ) {

			for ( int i = 0; i < initialHPs.Length; i++ ) {
				if ( maxHPs[i] <= 0 )
					continue;

				sb.AppendFormat( "#{0}: 陸上施設 第{1}基地 HP: {2} / {3}\r\n\r\n",
					i + 1,
					i + 1,
					initialHPs[i], maxHPs[i] );
			}

		}

		private static void OutputEnemyData( StringBuilder sb, ShipDataMaster[] members, int[] levels, int[] initialHPs, int[] maxHPs, EquipmentDataMaster[][] slots, int[][] parameters ) {

			for ( int i = 0; i < members.Length; i++ ) {
				if ( members[i] == null )
					continue;

				sb.AppendFormat( "#{0}: {1} {2} Lv. {3} HP: {4} / {5}",
					i + 1,
					members[i].ShipTypeName, members[i].NameWithClass,
					levels[i],
					initialHPs[i], maxHPs[i] );

				if ( parameters != null ) {
					sb.AppendFormat( ConstantsRes.BattleDetail_EnemyStats,
					parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3] );
				}

				sb.AppendLine().Append( "　" );
				for ( int k = 0; k < slots[i].Length; k++ ) {
					var eq = slots[i][k];
					if ( eq != null ) {
						if ( k > 0 )
							sb.Append( ", " );
						sb.Append( eq.ToString() );
					}
				}
				sb.AppendLine();
			}
		}


		private static void OutputResultData( StringBuilder sb, int index, string name, int initialHP, int resultHP, int maxHP ) {
			sb.AppendFormat( "#{0}: {1} HP: ({2} → {3})/{4} ({5})\r\n",
				index + 1, name,
				Math.Max( initialHP, 0 ),
				Math.Max( resultHP, 0 ),
				Math.Max( maxHP, 0 ),
				resultHP - initialHP );
		}


		private static string GetBattleResult( BattleManager bm ) {
			var result = bm.Result;

			var sb = new StringBuilder();


			sb.AppendLine( ConstantsRes.BattleDetail_Result );
			sb.AppendFormat( ConstantsRes.BattleDetail_ResultRank + "\r\n", result.Rank );

			if ( bm.IsCombinedBattle ) {
				sb.AppendFormat( ConstantsRes.BattleDetail_ResultMVPMain + "\r\n",
					result.MVPIndex == -1 ? "(なし)" : bm.FirstBattle.Initial.FriendFleet.MembersInstance[result.MVPIndex - 1].NameWithLevel );
				sb.AppendFormat( ConstantsRes.BattleDetail_ResultMVPEscort + "\r\n",
					result.MVPIndexCombined == -1 ? "(なし)" : bm.FirstBattle.Initial.FriendFleetEscort.MembersInstance[result.MVPIndexCombined - 1].NameWithLevel );

			} else {
				sb.AppendFormat( "MVP: {0}\r\n",
					result.MVPIndex == -1 ? "(なし)" : bm.FirstBattle.Initial.FriendFleet.MembersInstance[result.MVPIndex - 1].NameWithLevel );
			}

			sb.AppendFormat( ConstantsRes.BattleDetail_AdmiralExp + "\r\n" + ConstantsRes.BattleDetail_ShipExp + "\r\n",
				result.AdmiralExp, result.BaseExp );


			if ( !bm.IsPractice ) {
				sb.AppendLine().AppendLine( ConstantsRes.BattleDetail_Drop );


				int length = sb.Length;

				var ship = KCDatabase.Instance.MasterShips[result.DroppedShipID];
				if ( ship != null ) {
					sb.AppendFormat( "　{0} {1}\r\n", ship.ShipTypeName, ship.NameWithClass );
				}

				var eq = KCDatabase.Instance.MasterEquipments[result.DroppedEquipmentID];
				if ( eq != null ) {
					sb.AppendFormat( "　{0} {1}\r\n", eq.CategoryTypeInstance.Name, eq.Name );
				}

				var item = KCDatabase.Instance.MasterUseItems[result.DroppedItemID];
				if ( item != null ) {
					sb.Append( "　" ).AppendLine( item.Name );
				}

				if ( length == sb.Length ) {
					sb.AppendLine( "　(なし)" );
				}
			}


			return sb.ToString();
		}

	}
}
