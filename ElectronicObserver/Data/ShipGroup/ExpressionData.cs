using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.ShipGroup
{

	/// <summary>
	/// 艦船フィルタの式データ
	/// </summary>
	[DataContract(Name = "ExpressionData")]
	public class ExpressionData : ICloneable
	{

		public enum ExpressionOperator
		{
			Equal,
			NotEqual,
			LessThan,
			LessEqual,
			GreaterThan,
			GreaterEqual,
			Contains,
			NotContains,
			BeginWith,
			NotBeginWith,
			EndWith,
			NotEndWith,
			ArrayContains,
			ArrayNotContains,
		}


		[DataMember]
		public string LeftOperand { get; set; }

		[DataMember]
		public ExpressionOperator Operator { get; set; }

		[DataMember]
		public object RightOperand { get; set; }


		[DataMember]
		public bool Enabled { get; set; }


		[IgnoreDataMember]
		private static readonly Regex regex_index = new Regex(@"\.(?<name>\w+)(\[(?<index>\d+?)\])?", RegexOptions.Compiled);

		[IgnoreDataMember]
		public static readonly Dictionary<string, string> LeftOperandNameTable = new Dictionary<string, string>() {
            { ".MasterID", "Unique ID" },
            { ".ShipID", "Ship ID" },
            { ".MasterShip.NameWithClass", "Ship Name" },
            { ".MasterShip.ShipType", "Ship Type" },
            { ".Level", "Level" },
            { ".ExpTotal", "Exp" },
            { ".ExpNext", "Exp needed to level up" },
            { ".ExpNextRemodel", "Exp needed to remodel" },
            { ".HPCurrent", "Current HP" },
            { ".HPMax", "Max HP" },
            { ".HPRate", "HP Percentage" },
            { ".Condition", "Condition" },
            { ".AllSlotMaster", "Equipments" },
            { ".SlotMaster[0]", "Equipment #1" },	//checkme: 要る?
			{ ".SlotMaster[1]", "Equipment #2" },
            { ".SlotMaster[2]", "Equipment #3" },
            { ".SlotMaster[3]", "Equipment #4" },
            { ".SlotMaster[4]", "Equipment #5" },
            { ".ExpansionSlotMaster", "Ex.Equipment" },
            { ".Aircraft[0]", "Aircraft #1" },
            { ".Aircraft[1]", "Aircraft #2" },
            { ".Aircraft[2]", "Aircraft #3" },
            { ".Aircraft[3]", "Aircraft #4" },
            { ".Aircraft[4]", "Aircraft #5" },
            { ".AircraftTotal", "Aircraft Total" },		//要る？
			{ ".MasterShip.Aircraft[0]", "Max Aircraft #1" },
            { ".MasterShip.Aircraft[1]", "Max Aircraft #2" },
            { ".MasterShip.Aircraft[2]", "Max Aircraft #3" },
            { ".MasterShip.Aircraft[3]", "Max Aircraft #4" },
            { ".MasterShip.Aircraft[4]", "Max Aircraft #5" },
            { ".MasterShip.AircraftTotal", "Max Aircraft Total" },
            { ".AircraftRate[0]", "Aircraft #1 Percentage" },
            { ".AircraftRate[1]", "Aircraft #2 Percentage" },
            { ".AircraftRate[2]", "Aircraft #3 Percentage" },
            { ".AircraftRate[3]", "Aircraft #4 Percentage" },
            { ".AircraftRate[4]", "Aircraft #5 Percentage" },
            { ".AircraftTotalRate", "Aircraft Total Percentage" },
            { ".Fuel", "Fuel" },
            { ".Ammo", "Ammo" },
            { ".FuelMax", "Max Fuel" },
            { ".AmmoMax", "Max Ammo" },
            { ".FuelRate", "Fuel Percentage" },
            { ".AmmoRate", "Ammo Percentage" },
            { ".SlotSize", "Slots Count" },
            { ".RepairingDockID", "Dock ID" },
            { ".RepairTime", "Repair Time" },
            { ".RepairSteel", "Repair Steel" },
            { ".RepairFuel", "Repair Fuel" },
			//強化値シリーズは省略
			{ ".FirepowerBase", "Firepower Base" },
            { ".TorpedoBase", "Torpedo Base" },
            { ".AABase", "AA Base" },
            { ".ArmorBase", "Armor Base" },
            { ".EvasionBase", "Evasion Base" },
            { ".ASWBase", "ASW Base" },
            { ".LOSBase", "LOS Base" },
            { ".LuckBase", "Luck Base" },
            { ".FirepowerTotal", "Firepower Total" },
            { ".TorpedoTotal", "Torpedo Total" },
            { ".AATotal", "AA Total" },
            { ".ArmorTotal", "Armor Total" },
            { ".EvasionTotal", "Evasion Total" },
            { ".ASWTotal", "ASW Total" },
            { ".LOSTotal", "LOS Total" },
            { ".LuckTotal", "Luck Total" },
            { ".BomberTotal", "Bomber Total" },
            { ".FirepowerRemain", "Firepower Remain" },
            { ".TorpedoRemain", "Torpedo Remain" },
            { ".AARemain", "AA Remain" },
            { ".ArmorRemain", "Armor Remain" },
            { ".LuckRemain", "Luck Remain" },
            { ".Range", "Range" },		//現在の射程
			{ ".Speed", "Speed" },
            { ".MasterShip.Speed", "Speed Base" },
            { ".MasterShip.Rarity", "Rarity" },
            { ".IsLocked", "Locked" },
            { ".IsLockedByEquipment", "Locked by Equipment" },
            { ".SallyArea", "Event Tag" },
            { ".FleetWithIndex", "Fleet Index" },
            { ".IsMarried", "Married" },
            { ".AirBattlePower", "Aerial Power" },
            { ".ShellingPower", "Shelling Power" },
            { ".AircraftPower", "Aircraft Power" },
            { ".AntiSubmarinePower", "ASW Power" },
            { ".TorpedoPower", "Torpedo Power" },
            { ".NightBattlePower", "Night Battle Power" },
            { ".MasterShip.AlbumNo", "Album No." },
            { ".MasterShip.NameReading", "Name Reading" },
            { ".MasterShip.RemodelBeforeShipID", "Ship ID Before Remodel" },
            { ".MasterShip.RemodelAfterShipID", "Ship ID After Remodel" },
			//マスターのパラメータ系もおそらく意味がないので省略
			{ ".MasterShip.EquippableCategories", "Equippable Categories" },
		};

		private static Dictionary<string, Type> ExpressionTypeTable = new Dictionary<string, Type>();


		[IgnoreDataMember]
		public static readonly Dictionary<ExpressionOperator, string> OperatorNameTable = new Dictionary<ExpressionOperator, string>() {
            { ExpressionOperator.Equal, "==" },
            { ExpressionOperator.NotEqual, "!=" },
            { ExpressionOperator.LessThan, "<" },
            { ExpressionOperator.LessEqual, "<=" },
            { ExpressionOperator.GreaterThan, ">" },
            { ExpressionOperator.GreaterEqual, ">=" },
            { ExpressionOperator.Contains, "Contains" },
			{ ExpressionOperator.NotContains, "Not Contains" },
			{ ExpressionOperator.BeginWith, "Begin With" },
			{ ExpressionOperator.NotBeginWith, "Not Begin With" },
			{ ExpressionOperator.EndWith, "End With" },
			{ ExpressionOperator.NotEndWith, "Not End With" },
			{ ExpressionOperator.ArrayContains, "Array Contains" },
			{ ExpressionOperator.ArrayNotContains, "Array Not Contains" },

		};



		public ExpressionData()
		{
			Enabled = true;
		}

		public ExpressionData(string left, ExpressionOperator ope, object right)
			: this()
		{
			LeftOperand = left;
			Operator = ope;
			RightOperand = right;
		}


		public Expression Compile(ParameterExpression paramex)
		{

			Expression memberex = null;
			Expression constex = Expression.Constant(RightOperand, RightOperand.GetType());

			{
				Match match = regex_index.Match(LeftOperand);
				if (match.Success)
				{

					do
					{

						if (memberex == null)
						{
							memberex = Expression.PropertyOrField(paramex, match.Groups["name"].Value);
						}
						else
						{
							memberex = Expression.PropertyOrField(memberex, match.Groups["name"].Value);
						}

						if (int.TryParse(match.Groups["index"].Value, out int index))
						{
							memberex = Expression.Property(memberex, "Item", Expression.Constant(index, typeof(int)));
						}

					} while ((match = match.NextMatch()).Success);

				}
				else
				{
					memberex = Expression.PropertyOrField(paramex, LeftOperand);
				}
			}

			if (memberex.Type.IsEnum)
				memberex = Expression.Convert(memberex, typeof(int));

			Expression condex;
			switch (Operator)
			{
				case ExpressionOperator.Equal:
					condex = Expression.Equal(memberex, constex);
					break;
				case ExpressionOperator.NotEqual:
					condex = Expression.NotEqual(memberex, constex);
					break;
				case ExpressionOperator.LessThan:
					condex = Expression.LessThan(memberex, constex);
					break;
				case ExpressionOperator.LessEqual:
					condex = Expression.LessThanOrEqual(memberex, constex);
					break;
				case ExpressionOperator.GreaterThan:
					condex = Expression.GreaterThan(memberex, constex);
					break;
				case ExpressionOperator.GreaterEqual:
					condex = Expression.GreaterThanOrEqual(memberex, constex);
					break;
				case ExpressionOperator.Contains:
					condex = Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotContains:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.BeginWith:
					condex = Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotBeginWith:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.EndWith:
					condex = Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotEndWith:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.ArrayContains:  // returns Enumerable.Contains<>( memberex )
					condex = Expression.Call(typeof(Enumerable), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex);
					break;
				case ExpressionOperator.ArrayNotContains:   // returns !Enumerable.Contains<>( memberex )
					condex = Expression.Not(Expression.Call(typeof(Enumerable), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex));
					break;

				default:
					throw new NotImplementedException();
			}

			return condex;
		}



		public static Type GetLeftOperandType(string left)
		{

			if (ExpressionTypeTable.ContainsKey(left))
			{
				return ExpressionTypeTable[left];

			}
			else if (KCDatabase.Instance.Ships.Count > 0)
			{

				object obj = KCDatabase.Instance.Ships.Values.First();

				Match match = regex_index.Match(left);
				if (match.Success)
				{

					do
					{

						if (int.TryParse(match.Groups["index"].Value, out int index))
						{
							obj = ((dynamic)obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null))[index];
						}
						else
						{
							object obj2 = obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null);
							if (obj2 == null)
							{   //プロパティはあるけどnull
								var type = obj.GetType().GetProperty(match.Groups["name"].Value).GetType();
								ExpressionTypeTable.Add(left, type);
								return type;
							}
							else
							{
								obj = obj2;
							}
						}

					} while (obj != null && (match = match.NextMatch()).Success);


					if (obj != null)
					{
						ExpressionTypeTable.Add(left, obj.GetType());
						return obj.GetType();
					}
				}

			}

			return null;
		}

		public Type GetLeftOperandType()
		{
			return GetLeftOperandType(LeftOperand);
		}



		public override string ToString() => $"{LeftOperandToString()} は {RightOperandToString()} {OperatorToString()}";



		/// <summary>
		/// 左辺値の文字列表現を求めます。
		/// </summary>
		public string LeftOperandToString()
		{
			if (LeftOperandNameTable.ContainsKey(LeftOperand))
				return LeftOperandNameTable[LeftOperand];
			else
				return LeftOperand;
		}

		/// <summary>
		/// 演算子の文字列表現を求めます。
		/// </summary>
		public string OperatorToString()
		{
			return OperatorNameTable[Operator];
		}

		/// <summary>
		/// 右辺値の文字列表現を求めます。
		/// </summary>
		public string RightOperandToString()
		{

			if (LeftOperand == ".MasterID")
			{
				var ship = KCDatabase.Instance.Ships[(int)RightOperand];
				if (ship != null)
					return $"{ship.MasterID} ({ship.NameWithLevel})";
				else
					return $"{(int)RightOperand} (Unavailable)";

			}
			else if (LeftOperand == ".ShipID")
			{
				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if (ship != null)
					return $"{ship.ShipID} ({ship.NameWithClass})";
				else
					return $"{(int)RightOperand} (Nonexistence)";

			}
			else if (LeftOperand == ".MasterShip.ShipType")
			{
				var shiptype = KCDatabase.Instance.ShipTypes[(int)RightOperand];
				if (shiptype != null)
					return shiptype.Name;
				else
					return $"{(int)RightOperand} (Undefined)";

			}
			else if (LeftOperand.Contains("SlotMaster"))
			{
				if ((int)RightOperand == -1)
				{
					return "(Empty)";
				}
				else
				{
					var eq = KCDatabase.Instance.MasterEquipments[(int)RightOperand];
					if (eq != null)
						return eq.Name;
					else
						return $"{(int)RightOperand} (Undefined)";
				}
			}
			else if (LeftOperand == ".MasterShip.EquippableCategories")
			{
				var cat = KCDatabase.Instance.EquipmentTypes[(int)RightOperand];
				if (cat != null)
					return cat.Name;
				else
					return $"{(int)RightOperand} (未定義)";

			}
			else if (LeftOperand.Contains("Rate") && RightOperand is double)
			{
				return ((double)RightOperand).ToString("P0");

			}
			else if (LeftOperand == ".RepairTime")
			{
				return DateTimeHelper.ToTimeRemainString(DateTimeHelper.FromAPITimeSpan((int)RightOperand));

			}
			else if (LeftOperand == ".Range")
			{
				return Constants.GetRange((int)RightOperand);

			}
			else if (LeftOperand == ".Speed" || LeftOperand == ".MasterShip.Speed")
			{
				return Constants.GetSpeed((int)RightOperand);

			}
			else if (LeftOperand == ".MasterShip.Rarity")
			{
				return Constants.GetShipRarity((int)RightOperand);

			}
			else if (LeftOperand == ".MasterShip.AlbumNo")
			{
				var ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault(s => s.AlbumNo == (int)RightOperand);
				if (ship != null)
					return $"{(int)RightOperand} ({ship.NameWithClass})";
				else
					return $"{(int)RightOperand} (Nonexistence)";

			}
			else if (LeftOperand == ".MasterShip.RemodelAfterShipID")
			{

				if (((int)RightOperand) == 0)
					return "Fully Remodeled";

				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if (ship != null)
					return $"{ship.ShipID} ({ship.NameWithClass})";
				else
					return $"{(int)RightOperand} (Nonexistence)";

			}
			else if (LeftOperand == ".MasterShip.RemodelBeforeShipID")
			{

				if (((int)RightOperand) == 0)
					return "Unremodeled";

				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if (ship != null)
					return $"{ship.ShipID} ({ship.NameWithClass})";
				else
					return $"{(int)RightOperand} (Nonexistence)";

			}
			else if (RightOperand is bool)
			{
				return ((bool)RightOperand) ? "○" : "×";

			}
			else
			{
				return RightOperand.ToString();

			}

		}


		public ExpressionData Clone()
		{
			var clone = MemberwiseClone();      //checkme: 右辺値に参照型を含む場合死ぬ
			return (ExpressionData)clone;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}




}
